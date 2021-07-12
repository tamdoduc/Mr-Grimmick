
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckTop;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckWall;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] LayerMask pipeLayer;
    [SerializeField] CheckTop checkTop;
    [SerializeField] float wakeRange, resetRange, camRange, groundLim;
    [SerializeField] bool ready = false;
    [SerializeField] AudioSource dieSE;
    [SerializeField] AudioSource flyModeSE;
    AudioSource cloneAudio;
    private int healPoint = 1, score = 120, jumpCount = 12;
    private float detectTime = 0, handleTime = 0, posX, posY, dieVelocity = 0.7f;
    private bool isActive = false, faceRight = false, isJump = false, flyMode = false, outRange = false;
    private Vector2 vel;
    private const float MaxVelocityXRight = 4.5f, MaxVelocityXLeft = -4.5f, MaxVelocityY = 5f, MaxGravity = -8f, resetTime = 1.5f, jumpTime = 0.35f, eps = 0.15f, dieTime = 0.2f;

    void Start()
    {
        body.velocity = new Vector2(0, 0);
        target = GameObject.Find("Player").GetComponent<Transform>();
        posX = transform.position.x;
        posY = transform.position.y;
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
                if (!flyMode)
                    PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + score);
                else
                    PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + score * 2);
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
                CheckOutRange();
                if (isActive)
                {
                    if (IsColliderPipe())
                    {
                        body.velocity = new Vector2(0, 0);
                        return;
                    }
                    SetState();
                    CheckHit();
                    if (!flyMode)
                    {
                        Debug.Log(flyMode + " " + jumpCount);
                        body.velocity = vel;
                        if (jumpCount > 0)
                        {
                            NormalMode();
                        }
                        else
                        {
                            handleTime = 0;
                            checkTop.SetActive(false);
                            colliderHead.isTrigger = true;
                            flyMode = true;
                        }
                    }
                    else FlyMode();
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
            healPoint = 0;
        }
    }
    void DieState()
    {
        animator.SetBool("Dead", true);
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
        transform.eulerAngles = new Vector3(180, 0, 0);
        handleTime += Time.deltaTime;
        colliderHead.isTrigger = true;
        colliderBody.isTrigger = true;
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
    void SetState()
    {
        animator.SetFloat("Speed", Mathf.Abs(vel.x));
        animator.SetBool("OnGround", IsGrounded());
    }
    void CheckInRange()
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) < wakeRange)
        {
            CheckFace();
            isActive = true;
        }
    }
    void CheckOutRange()
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) > resetRange
            || Mathf.Abs(target.transform.position.y - transform.position.y) > resetRange
            || transform.position.y < groundLim
            || target.transform.position.y < groundLim)
        {
            outRange = true;
            if (cloneAudio != null)
                GameObject.Destroy(cloneAudio);
        }
        if (outRange)
        {
            isActive = false;
            transform.position = new Vector3(posX, posY + resetRange * 2, 0);
            body.velocity = new Vector2(0, 0);
            if (Mathf.Abs(target.transform.position.x - posX) > camRange * 1.5f
                || Mathf.Abs(target.transform.position.y - posY) > camRange * 1.5f
                || target.transform.position.y < groundLim)
                ResetBoom();
        }
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
            if (handleTime >= jumpTime || IsColliderTop())
            {
                handleTime = 0;
                jumpCount--;
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
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }
    }
    void NormalMode()
    {
        if (IsGrounded())
        {
            vel.y = -1f;
        }
        if (!IsNotEdge() || IsColliderWall())
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
    void FlyMode()
    {
        if (!ready)
        {
            vel = new Vector2(0, -8);
            handleTime += Time.deltaTime;
            if (handleTime > resetTime * 2)
            {
                ready = true;
            }
        }
        else
        {
            animator.SetBool("Fly", flyMode);
            if (cloneAudio == null)
            cloneAudio = AudioSource.Instantiate(flyModeSE);
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
            if (target.transform.position.y <= transform.position.y && !IsGrounded())
            {
                if (vel.y >= MaxGravity)
                {
                    vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
                }
            }
            else
                if (target.transform.position.y - eps <= transform.position.y)
                vel.y = -1f;
            else
            {
                if (vel.y < MaxVelocityY)
                {
                    vel.y += (Time.deltaTime * MaxVelocityY) / (0.4f);
                }
                else
                if (vel.y > MaxVelocityY)
                {
                    vel.y = MaxVelocityY;
                }
            }
        }
        body.velocity = vel;
    }
    void ResetBoom()
    {
        isActive = false;
        flyMode = false;
        isJump = false;
        animator.SetBool("Fly", false);
        body.velocity = new Vector2(0, 0);
        animator.SetFloat("Speed", 0);
        outRange = false;
        checkTop.SetActive(true);
        detectTime = 0;
        handleTime = 0;
        ready = false;
        jumpCount = 12;
        transform.position = new Vector3(posX, posY, 0);
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
    bool IsColliderWall()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckWall.bounds.center, colliderCheckWall.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderTop()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckTop.bounds.size, 0, Vector2.up, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsColliderPipe()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, pipeLayer);
        return hit2D.collider != null;
    }

}