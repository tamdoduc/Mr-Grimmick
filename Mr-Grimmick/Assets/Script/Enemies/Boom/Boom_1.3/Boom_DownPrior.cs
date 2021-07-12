using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_DownPrior : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckTop;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckWall;
    [SerializeField] Collider2D colliderCheckThorn;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] LayerMask thornLayer;
    [SerializeField] CheckTop checkTop;
    [SerializeField] SelfDestruct selfDestruct;
    [SerializeField] float resetRange, camRange, groundLim, topLim;
    [SerializeField] AudioSource dieSE;
    AudioSource cloneAudio;

    private int healPoint = 1, score = 120;
    private float detectTime = 0, handleTime = 0, posX, posY, dieVelocity = 0.7f;
    private bool isActive = false, faceRight = true, isJump = false;
    private Vector2 vel;
    private const float MaxVelocityXRight = 3.5f, MaxVelocityXLeft = -3.5f, MaxVelocityY = 5f, MaxGravity = -8f, resetTime = 1.5f, jumpTime = 0.35f, dieTime = 0.2f;

    void Start()
    {
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
                if (target.transform.position.y < groundLim || target.transform.position.y > topLim)
                {
                    ResetBoom();
                    return;
                }
                if (isActive)
                {
                    SetState();
                    CheckHit();
                    body.velocity = vel;
                    NormalMode();
                    CheckOutRange();
                }
                else
                    CheckInRange();
                break;
        }
    }
    void CheckHit()
    {
        if (IsColliderThorn())
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            GameObject.Destroy(this.gameObject);
        }
        if (IsColliderSkill())
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
    void SetState()
    {
        animator.SetFloat("Speed", Mathf.Abs(vel.x));
        animator.SetBool("OnGround", IsGrounded());
    }
    void CheckInRange()
    {
        if (target.transform.position.y > groundLim)
        {
            transform.position = new Vector3(posX, posY, 0);
            isActive = true;
        }
    }
    void CheckOutRange()
    {
        //move out stageRange
        if (transform.position.y < groundLim)
        {
            body.velocity = new Vector2(0, 0);
            transform.position = new Vector3(posX - resetRange - 1, posY - resetRange - 1, 0);
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
    void NormalMode()
    {
        if (IsGrounded())
        {
            vel.y = -1f;
        }
        if (IsNearThorn() || IsColliderWall())
        {
            if (!isJump && IsGrounded())
            {
                isJump = true;
            }
        }
        if (isJump)
            CheckJump();
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
    void ResetBoom()
    {
        isActive = false;
        isJump = false;
        faceRight = true;
        checkTop.SetActive(true);
        detectTime = 0;
        handleTime = 0;
        transform.position = new Vector3(posX, posY, 0);
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsNearThorn()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckThorn.bounds.center, colliderCheckThorn.bounds.size, 0, Vector2.down, 0.1f, thornLayer);
        return hit2D.collider != null;
    }
    bool IsColliderWall()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckWall.bounds.center, colliderCheckWall.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderTop()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckTop.bounds.size, 0, Vector2.up, 0.2f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsColliderThorn()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.down, 0.1f, thornLayer);
        return hit2D.collider != null;
    }

}