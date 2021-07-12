using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Boom : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] CheckTop checkTop;
    [SerializeField] float camRange, groundLim, limX;
    [SerializeField] AudioSource dieSE;
    AudioSource cloneAudio;

    private int healPoint = 1;
    private float detectTime = 0, handleTime = 0, posX, posY, dieVelocity = 0.7f, MaxVelocityY = 5f, MaxGravity = -8f, MaxVelocityXRight = 3.5f, MaxVelocityXLeft = -3.5f;
    private bool isActive = false, faceRight = false, isJump = false;
    private Vector2 vel;
    private const int score = 0;
    private const float resetTime = 1.5f, jumpTime = 0.35f, dieTime = 0.2f;

    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
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
                cloneAudio = AudioSource.Instantiate(dieSE);
                Destroy(cloneAudio.gameObject, 1);
                break;
            default:
                CheckHit();
                if (isActive)
                {
                    SetState();
                    CheckOutRange();
                    body.velocity = vel;
                    if (IsGrounded())
                        ResetVel();
                    NormalMode();
                }
                else
                    CheckInRange();
                break;
        }
    }
    void CheckHit()
    {
        if (IsColliderSkill())
        {
            healPoint--;
        }
    }
    void DieState()
    {
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
        animator.SetBool("Dead", true);
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
        if (Mathf.Abs(target.transform.position.y - transform.position.y) > camRange)
            GameObject.Destroy(this.gameObject);
    }
    void SetState()
    {
        animator.SetFloat("Speed", Mathf.Abs(vel.x));
        animator.SetBool("OnGround", IsGrounded());
    }
    public void SetStartVel(float x, float y)
    {
        if (x > 1)
            body.AddForce(Vector2.right * x, ForceMode2D.Impulse);
        else
            body.AddForce(Vector2.left * x, ForceMode2D.Impulse);
        body.AddForce(Vector2.up * y, ForceMode2D.Impulse);
    }
    private void ResetVel()
    {
        MaxVelocityY = 5f;
        MaxGravity = -8f; 
        MaxVelocityXRight = 3.5f;
        MaxVelocityXLeft = -3.5f;
    }
    void CheckInRange()
    {
        CheckFace();
        handleTime += Time.deltaTime;
        if (handleTime > resetTime)
        {
            isActive = true;
            handleTime = 0;
        }
    }
    void CheckOutRange()
    {
        if (transform.position.x > limX || transform.position.y < groundLim)
            GameObject.Destroy(this.gameObject);
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
    }
    void CheckJump()
    {
        //  Jumping
        if (isJump)
        {
            handleTime += Time.deltaTime;
            vel.y = MaxVelocityY;
            if (handleTime >= jumpTime)
            {
                handleTime = 0;
                isJump = false;
            }
        }
    }
    void FallDown()
    {
        if (handleTime == 0) // Fall down
            if (IsGrounded())
            {
                vel.y = -1f;
            }
            else if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.8f;
            }
    }
    void NormalMode()
    {
        if (IsGrounded())
        {
            vel.y = -1f;
        }
        if (!IsNotEdge())
        {
            if (!isJump && IsGrounded())
            {
                isJump = true;
            }
            else
                CheckJump();
        }
        CheckMove();
        if (!isJump)
            FallDown();
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
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.2f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() == "12") //thorTrap
        {
            GameObject.Destroy(this.gameObject);
        }
    }

}
