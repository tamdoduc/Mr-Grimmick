using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Electric : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderDS;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] CheckTop checkTop;
    [SerializeField] float activeRange, groundLim;
    [SerializeField] bool isCarry = false, isDSM = false;

    private int healPoint = 1;
    private float detectTime = 0.001f, handleTime = 0, posX, posY, dieVelocity = 0.7f;
    public bool isActive = false, inRange = false, faceRight = true, bfCarry = false, isElectric = false;
    private Vector2 vel;
    private const int score = 780;
    private const float MaxVelocityXRight = 3.5f, MaxVelocityXLeft = -3.5f, resetTime = 1.7f, idleTime = 1.5f, DSMTime = 0.7f, dieTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        posX = transform.position.x;
        posY = transform.position.y;
        body.velocity = new Vector2(0, 0);
        checkTop.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (healPoint)
        {
            case -1:
                DieState();
                break;
            case 0:
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + score);
                if (target.transform.position.x > transform.position.x)
                    dieVelocity = -dieVelocity;
                handleTime = 0;
                healPoint--;
                if (dieVelocity < 0)
                {
                    body.velocity = new Vector2(0, 0);
                    body.AddForce(Vector2.left * 3f, ForceMode2D.Impulse);
                    body.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
                }
                else
                {
                    body.velocity = new Vector2(0, 0);
                    body.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
                    body.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
                }
                break;
            case 1:
                if (isActive)
                {
                    if (IsOutRange())
                        GameObject.Destroy(this.gameObject);
                    CheckHit();
                    if (IsColliderDS())
                    {
                        isElectric = true;
                        animator.SetBool("Electric", true);
                        vel = new Vector2(0, vel.y);
                        body.velocity = vel;
                        handleTime = 0;
                    }
                    if (isElectric)
                    {
                        StopState();
                    }
                    else
                    {
                        bfCarry = isCarry;
                        isCarry = IsCarry();
                        if (isCarry && isCarry != bfCarry)
                            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 10);
                        animator.SetBool("CP", isCarry);
                        if (isCarry)
                        {
                            GameObject g = gameObject.transform.GetChild(1).gameObject;
                            g.layer = 8;
                            IdleState();
                        }
                        else
                        {
                            GameObject g = gameObject.transform.GetChild(1).gameObject;
                            g.layer = 13;
                            GameObject ds = gameObject.transform.GetChild(3).gameObject;
                            ds.layer = 18;
                            if (!isDSM)
                            {
                                body.velocity = vel;

                                //handleSkill
                                if (detectTime == 0 || handleTime > 0)
                                    handleTime += Time.deltaTime;

                                if (handleTime >= idleTime)
                                {
                                    handleTime = 0;
                                    vel = new Vector2(0, vel.y);
                                    body.velocity = vel;
                                    isDSM = true;
                                    animator.SetBool("DSM", true);
                                    animator.SetFloat("Speed", 0);
                                    return;
                                }
                                //
                                if (!IsNotEdge())
                                    faceRight = !faceRight;
                                CheckMove();
                                if (detectTime < resetTime)
                                {
                                    detectTime += Time.deltaTime;
                                }
                                else
                                {
                                    CheckFace();
                                    detectTime = 0;

                                }
                            }
                            else
                                StopState();
                        }
                    }
                }
                else
                if (inRange)
                    CheckActive();
                else
                    CheckRange();
                break;
        }
       
    }
    void CheckHit()
    {
        if (IsColliderSkill() && isCarry)
        {
            healPoint--;  
        }
    }
    void DieState()
    {
        animator.SetBool("Dead", true);
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
        transform.eulerAngles = new Vector3(180, 0, 0);
        colliderHead.isTrigger = true;
        colliderBody.isTrigger = true;
        handleTime += Time.deltaTime;
        if (handleTime > dieTime)
            if (dieVelocity < 0)
            {
                body.velocity = new Vector2(dieVelocity, 0);
                body.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
            }
            else
            {
                body.velocity = new Vector2(dieVelocity, 0);
                body.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
            }
        if (transform.position.y < groundLim)
            GameObject.Destroy(this.gameObject);
    }
    void IdleState()
    {
        body.velocity = new Vector2(0, vel.y);
        animator.SetFloat("Speed", 0);
        GameObject g = gameObject.transform.GetChild(3).gameObject;
        g.layer = 8;
    }
    void StopState()
    {
        handleTime += Time.deltaTime;
        if (handleTime > DSMTime)
        {
            detectTime = 0.001f;
            handleTime = 0;
            isDSM = false;
            isElectric = false;
            animator.SetBool("Electric", false);
            animator.SetBool("DSM", false);
        }
    }
    void CheckRange()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) < 2.5f)
        {
            this.transform.position = new Vector3(posX, posY, 0);
            CheckFace();
            inRange = true;
            body.velocity = new Vector2(0, -8f);
        }
    }
    void CheckActive()
    {
        if (IsGrounded())
        {
            isActive = true;
            vel.y = -1;
        }
    }
    void CheckMove()
    {
        if (faceRight)
        {
            if (vel.x < MaxVelocityXRight)
            {
                vel.x += (Time.deltaTime * MaxVelocityXRight) / (0.5f); 
            }
            else
            if (vel.x > MaxVelocityXRight)
            {
                vel.x = MaxVelocityXRight;
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            if (vel.x > MaxVelocityXLeft)
            {
                vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.5f);     
            }
            else
           if (vel.x < MaxVelocityXLeft)
            {
                vel.x = MaxVelocityXLeft;
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        animator.SetFloat("Speed", Mathf.Abs(vel.x));
    }
    void CheckFace()
    {
        if (target.transform.position.x > transform.position.x)
        {
            faceRight = true;
        }

        if (target.transform.position.x < transform.position.x)
        {
            faceRight = false;
        }
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsCarry()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderHead.bounds.center, colliderHead.bounds.size, 0, Vector2.down, 0.1f, playerLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsColliderDS()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderDS.bounds.center, colliderDS.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsOutRange()
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) > activeRange || Mathf.Abs(target.transform.position.y - transform.position.y) > activeRange)
            return true;
        else
            return false;
    }
}
