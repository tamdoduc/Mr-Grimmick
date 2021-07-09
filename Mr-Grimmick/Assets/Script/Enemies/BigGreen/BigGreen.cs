using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGreen : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckWall;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderDodgeSkill;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] CheckTop checkTop;
    [SerializeField] EffectFlash effectPlash;
    EffectFlash cloneEffectFLash;
    [SerializeField] float groundLim, leftLim, rightLim, topLim, camRange;


    public float activeDistanceX, dis = 0;
    private float posX, posY;

    private int healPoint = 3, score = 1800;
    private float detectTime = 0.001f, handleTime = 0, dieVelocity = 0.7f, imCount = 0;
    private bool isActive = false, faceRight = true, isJump = false, bounce = false, isIM = false;
    private Vector2 vel;
    private const float MaxVelocityXRight = 5.2f, MaxVelocityXLeft = -5.2f, MaxVelocityY = 7.5f, MaxGravity = -8f, imTime = 2f, resetTime = 0.3f, jumpTime = 0.3f, dieTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        posX = transform.position.x;
        posY = transform.position.y;
        checkTop.SetActive(true);
        vel = new Vector2(MaxVelocityXRight, -1);
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
            default:
                if (isActive)
                {
                    if (!isIM)
                    {
                        CheckHit();
                    }
                    else
                    {
                        imCount += Time.deltaTime;
                        if (imCount > imTime)
                        {
                            isIM = false;
                        }
                    }
                    SetState();
                    body.velocity = vel;
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
            imCount = 0;
            healPoint--;
            isIM = true;
            if (healPoint > 0)
            {
                if (cloneEffectFLash == null)
                {
                    cloneEffectFLash = GameObject.Instantiate(effectPlash);
                    cloneEffectFLash.SetTimeMax(0.5f);
                    cloneEffectFLash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                    cloneEffectFLash.Active();
                }
            }
        }
    }
    void DieState()
    {
        animator.SetBool("Dead", true);
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
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
        if (IsGrounded() && !isJump) // Check state jump
        {
            animator.SetBool("IsJump", false);
            animator.SetFloat("Gravity", -1);
            vel.y = -1;
        }
        else
        {
            animator.SetBool("IsJump", true);
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
    void Bounce()
    {
        faceRight = false;
        if (vel.x > MaxVelocityXLeft)
        {
            vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.2f);
        }
        else
           if (vel.x < MaxVelocityXLeft)
        {
            vel.x = MaxVelocityXLeft;
        }
        transform.eulerAngles = new Vector3(0, 180, 0);
        if (IsGrounded() && !isJump)
            bounce = false;
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
        CheckOutRange();
        if (IsGrounded())
        {
            vel.y = -1f;
        }
        if (IsColliderWall() || !IsNotEdge() || SpecialJump() || IsNearSkill())
        {
            if (!isJump && IsGrounded())
            {
                isJump = true;
            }
        }
        if (isJump)
            CheckJump();
        if (!isJump)
            FallDown();
        if (!bounce)
        {
            if (!IsColliderHead())
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
            Bounce();
    }
    void CheckInRange()
    {
        dis = Mathf.Abs(this.transform.position.x - target.transform.position.x);
        if (activeDistanceX >= Mathf.Abs(this.transform.position.x - target.transform.position.x))
        {
            CheckFace();
            isActive = true;
        }
    }
    void CheckOutRange()
    {
        if (target.transform.position.y > topLim)
            ResetEnemy();
        else
        if (transform.position.y < groundLim)
        {
            if (Mathf.Abs(target.transform.position.x - posX) > camRange || Mathf.Abs(target.transform.position.y - posY) > camRange)
            {
                ResetEnemy();
            }
            else
                transform.position = new Vector3(posX, groundLim - 1, 0);
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
    void ResetEnemy()
    {
        isActive = false;
        isJump = false;
        handleTime = 0;
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
    bool SpecialJump()
    {
        if (transform.position.x > rightLim)
        {
            bounce = true;
            return true;
        }
        else
            return false;
    }
    bool IsColliderWall()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckWall.bounds.center, colliderCheckWall.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsColliderHead()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderHead.bounds.center, colliderHead.bounds.size, 0, Vector2.up, 0.1f, playerLayer);
        return hit2D.collider != null;
    }
    bool IsNearSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderDodgeSkill.bounds.center, colliderDodgeSkill.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
}
