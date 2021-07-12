using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2 : MonoBehaviour
{
    [SerializeField] Transform target; 
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderSword;
    [SerializeField] Collider2D colliderSwordHit;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float groundLim, leftLim, rightLim;
    [SerializeField] Boss_2_Sword Sword;
    Boss_2_Sword sword;
    GameObject g; 
    private int healPoint = 5, score = 8000;
    private float handleTime = 0, dieVelocity = 0.7f, imCount = 0, murderCount = 0;
    private float rangeHit = 2f, rangeJump = 3.5f, rangeWalk = 5f;
    public float distance;
    public bool isActive = false, murderMode = false, isIM = false, hit = false, hurt = false, jump = false, goRight = false, faceRight = true, isJump = false;
    private Vector2 vel;
    private const float MaxVelocityXRight = 5f, MaxVelocityXLeft = -5f, MaxVelocityY = 12f, MaxGravity = -8f, imTime = 0.2f, murderTime = 2f, jumpTime = 0.3f, dieTime = 0.2f, eps = 1f;
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        g = gameObject.transform.GetChild(0).gameObject;
        vel.y = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
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
                    sword = GameObject.Instantiate(Sword);
                    if (faceRight)
                    {
                        sword.transform.eulerAngles = new Vector3(0, 180, 0);
                        sword.transform.position = this.gameObject.transform.position + new Vector3(1.5f, 1, 0);
                    }
                    else
                    {
                        sword.transform.position = this.gameObject.transform.position + new Vector3(-1.5f, 1, 0);
                    }
                    if (sword.transform.position.x > (leftLim + rightLim) / 2)
                        sword.MaxVelocity *= -1;
                    break;
                case 1:
                    murderMode = true;
                    MutualMode();
                    break;
                case 2:
                    murderMode = true;
                    MutualMode();
                    break;
                case 3:
                    murderMode = false;
                    MutualMode();
                    if (!isIM && IsGrounded())
                    {
                        MurderMode();
                        if (jump)
                        {
                            goRight = faceRight;
                            isJump = true;
                            return;
                        }
                    }
                    break;
                default:
                    MutualMode();
                    if (!isIM && !hit && !murderMode && IsGrounded())
                    {
                        Debug.Log(Mathf.Abs(body.velocity.x));
                        if (Mathf.Abs(body.velocity.x) > 1f)
                            murderCount = 0;
                        murderCount += Time.deltaTime;
                        if (murderCount > murderTime)
                        {
                            murderMode = true;
                            return;
                        }
                        if (transform.position.x < leftLim || transform.position.x > rightLim)
                        {
                            goRight = faceRight;
                            isJump = true;
                            return;
                        }
                        if (distance > rangeWalk)
                        {
                            goRight = faceRight;
                        }
                        else
                        {
                            goRight = !faceRight;
                        }
                        CheckMove();
                        if (distance > rangeWalk && distance < rangeWalk - eps)
                            vel.x = 0;
                        body.velocity = vel;
                    }
                    break;
            }
        }
        else
            if (target.position.x > leftLim - 1)
            isActive = true;
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
                hurt = true;
            }
        }
    }
    [SerializeField] UnlockStage unlock;
    void DieState()
    {
        unlock =  GameObject.Instantiate(unlock);
        unlock.SetStage(2);
        animator.SetBool("Dead", true);
        gameObject.layer = 17;
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
        handleTime += Time.deltaTime;
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
        animator.SetBool("Ground", IsGrounded());
        animator.SetBool("Hit", hit);
        animator.SetBool("Hurt", hurt);
    }
    void CheckRange()
    {
        distance = Mathf.Abs(target.transform.position.x - transform.position.x);
        if (distance < rangeHit)
        {
            hit = true;
            jump = false;
        }
        else
        {
            hit = false;
            if (distance < rangeJump)
                jump = true;
            else
            {
                jump = false;
            }
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
        if (handleTime == 0)
            if (IsGrounded())
            {
                vel.y = -1f;
            }
            else if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }
    }
    void CheckMove()
    {
        if (goRight)
        {
            if (body.velocity.x < 0)
                vel.x = 0;
            if (vel.x < MaxVelocityXRight)
            {
                vel.x += (Time.deltaTime * MaxVelocityXRight) / (0.8f);
            }
            else
            if (vel.x > MaxVelocityXRight)
            {
                vel.x = MaxVelocityXRight;
            }
        }
        else
        {
            if (body.velocity.x > 0)
                vel.x = 0;
            if (vel.x > MaxVelocityXLeft)
            {
                vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.8f);
            }
            else
           if (vel.x < MaxVelocityXLeft)
            {
                vel.x = MaxVelocityXLeft;
            }
        }
    }
    void CheckFace()
    {
        if (target.transform.position.x > transform.position.x)
        {
            faceRight = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (target.transform.position.x < transform.position.x)
        {
            faceRight = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    void MutualMode()
    {
        CheckRange();
        HitMode();
        if (hit)
            body.velocity = new Vector2(0, -1);
        if (!isIM)
            CheckHit();
        else
        {
            murderCount = 0;
            imCount += Time.deltaTime;
            if (imCount > imTime)
            {
                isIM = false;
                hurt = false;
            }
            body.velocity = new Vector2(0, -2);
            return;
        }
        CheckFace();

        //jump
        if (isJump)
        {
            CheckMove();
            CheckJump();
            vel.x *= 1.25f;
            body.velocity = vel;
        }
        else
        {
            FallDown();
            body.velocity = vel;
        }

        SetState();
        if (murderMode)
            MurderMode();

    }
    void HitMode()
    {
        var swordhit = gameObject.transform.GetChild(2).gameObject;
        if (!isIM && IsGrounded())
        {
            if (hit || IsNearSkill())
            {
                g.layer = 18;
                swordhit.layer = 13;
            }
            else
            {
                g.layer = 17;
                swordhit.layer = 17;
            }
        }
        else
        {
            g.layer = 17;
            swordhit.layer = 17;
        }
    }
    void MurderMode()
    {
        CheckFace();
        goRight = faceRight;
        CheckMove();
        body.velocity = vel;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsNearSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderSword.bounds.center, colliderSword.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
}
