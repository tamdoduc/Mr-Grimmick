using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckTop;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckWall;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] bool isBack, faceRight, canChase;
    [SerializeField] float resetRange, groundLim, wakeRange;
    [SerializeField] int healPoint = 1;

    [SerializeField] bool isActive = false, runMode = false, isJump = false, chaseMode = false, delayed = false, hurt = false;
    private float handleTime = 0, detectTime = 0, imCount = 0;
    private Vector2 vel;

    private float MaxVelocityXRight = 4.5f, MaxVelocityXLeft = -4.5f, MaxVelocityY = 5f, MaxGravity = -8f, resetTime = 0.5f, jumpTime = 0.35f, eps = 0.15f, chaseTime = 1.5f, imTime = 0.5f, runTime = 4f;
    void Start()
    {
        body.velocity = new Vector2(0, 0);
        target = GameObject.Find("Player").GetComponent<Transform>();
        if (isBack)
            animator.SetBool("Back", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            //CheckOutRange();
            if (hurt || runMode || chaseMode)
                body.isKinematic = false;
            if (!runMode && !hurt)
                CheckHit();
            if (hurt)
            {
                imCount += Time.deltaTime;
                if (imCount > imTime)
                    hurt = false;
                return;
            }
            if (runMode && !delayed)
            {
                detectTime += Time.deltaTime;
                if (detectTime > jumpTime / 3)
                    delayed = true;
                else
                    return;
            }
            if (chaseMode || runMode)
            {
                animator.SetBool("OnGround", IsGrounded());
                if (canChase || runMode)
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
                }
                //
                if (!runMode && canChase)
                {
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
            }
            else
            {
                if (canChase)
                    handleTime += Time.deltaTime;
                if (handleTime > chaseTime)
                {
                    chaseMode = true;
                    animator.SetBool("Move", true);
                    handleTime = 0;
                }
            }
            if (runMode)
            {
                detectTime+= Time.deltaTime;
                if (detectTime > runTime)
                    GameObject.Destroy(this.gameObject);
                if (vel.x > 0)
                    body.velocity = vel + new Vector2(2, 0);
                else
                    body.velocity = vel - new Vector2(2, 0);
            }
            else
                body.velocity = vel;
        }
        else
            CheckInRange();
    }
    void CheckHit()
    {
        if (IsColliderSkill() && !hurt)
        {
            hurt = true;
            healPoint--;
            imCount = 0;
            if (healPoint == 0)
            {
                runMode = true;
                vel = new Vector2(0, 3);
                body.velocity = vel;
                animator.SetBool("Move", true);
                CheckFace();
                faceRight = !faceRight;
                if (faceRight)
                    transform.eulerAngles = new Vector3(0, 0, 0);
                else
                    transform.eulerAngles = new Vector3(0, 180, 0);
                detectTime = 0;
                handleTime = 0;
            }
        }
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
            GameObject.Destroy(this.gameObject);
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

}
