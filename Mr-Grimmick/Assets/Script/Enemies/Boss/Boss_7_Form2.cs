using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_7_Form2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderDodge;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float groundLim, leftLim, rightLim, leftPosX, rightPosX;
    [SerializeField] EffectFlash effectFlash;
    EffectFlash cloneEffectFlash;
    [SerializeField] Boss_7_Skill1 skill1;
    Boss_7_Skill1 cloneSkill1;

    public bool isActive = false, isIM = false, isActing = false, faceRight = false, usedSkill = false, isJump = false;
    public int healPoint = 4, randomAction = 0;
    private float randomCount = 0, imCount = 0, actingCount = 0, transformCount = 0, jumpCount = 0;
    private const float randomTime = 3f, skillTime = 0.5f, prepareTime = 3f, imTime = 0.5f, transformTime = 1f, walkTime = 2f, jumpTime = 0.35f;
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        body.velocity = new Vector2(0, -8);
    }

    // Update is called once per frame
    void Update()
    {
        switch (healPoint)
        {
            case 0:
                DieState();
                break;
            default:
                if (isActive)
                {
                    if (!isIM)
                        CheckHit();
                    else
                    {
                        imCount += Time.deltaTime;
                        if (imCount > imTime)
                        {
                            isIM = false;
                            animator.SetBool("Hit", false);
                        }
                        else return;
                    }
                    CheckFace();
                    if (randomAction == 0)
                    {
                        randomCount += Time.deltaTime;
                        if (randomCount > randomTime)
                        {
                            if (!isActing)
                            {
                                randomAction = Random.Range(1, 4);
                                if (randomAction < 3)
                                    isJump = true;
                                randomCount = 0;
                                isActing = true;
                            }
                        }
                        else
                        {
                            Debug.Log(IsColliderDodge() + " " + isActing);
                            if (!isActing && IsColliderDodge())
                            {
                                randomCount = 0;
                                isActing = true;
                                randomAction = 2;
                            }
                        }
                    }

                    switch (randomAction)
                    {
                        case 1:
                            Jump();
                            break;
                        case 2:
                            Dodge();
                            break;
                        case 3:
                            Walk();
                            break;
                        case 4:
                            Skill1();
                            break;
                        case 5:
                            Skill2();
                            break;
                    }
                }
                else
                    CheckInRange();
                break;
        }
    }
    public void SetStartPos(Vector3 pos)
    {
        transform.position = pos;
    }
    void CheckInRange()
    {
        if (target.transform.position.x > leftLim)
        {
            transformCount += Time.deltaTime;
            body.velocity = new Vector2(0, -8);
            if (cloneEffectFlash == null)
            {
                cloneEffectFlash = GameObject.Instantiate(effectFlash);
                cloneEffectFlash.SetTimeMax(2f);
                cloneEffectFlash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                cloneEffectFlash.Active();
            }
            if (transformCount > prepareTime)
            {
                isActive = true;
                animator.SetBool("Prepare", true);
            }
        }
    }
    void DieState()
    {
        animator.SetBool("Dead", true);
        transformCount += Time.deltaTime;
        if (transformCount > transformTime)
            GameObject.Destroy(this.gameObject);
    }
    void CheckHit()
    {
        if (IsColliderSkill() && !isIM)
        {
            imCount = 0;
            healPoint--;
            if (healPoint > 0)
            {
                isIM = true;
                animator.SetBool("Hit", true);
            }
            else
                if (cloneEffectFlash == null)
            {
                cloneEffectFlash = GameObject.Instantiate(effectFlash);
                cloneEffectFlash.SetTimeMax(0.8f);
                cloneEffectFlash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                cloneEffectFlash.Active();
            }
        }
    }
    void Jump()
    {
        animator.SetBool("Jump", true);
        if (isJump)
        {
            jumpCount += Time.deltaTime;
            if (target.transform.position.x > transform.position.x)
                body.velocity = new Vector2(2, 10);
            else
                body.velocity = new Vector2(-2, 10);
            if (jumpCount >= jumpTime)
            {
                jumpCount = 0;
                isJump = false;
            }
        }
        if (!isJump)
        if (IsGrounded())
        {
            body.velocity = new Vector2(0, -8);
        }
        else 
        {
            if (target.transform.position.x > transform.position.x)
                body.velocity = new Vector2(1, -8);
            else
                body.velocity = new Vector2(-1, -8);
        }
        if (IsGrounded() && !isJump)
        {
            animator.SetBool("Jump", false);
            isJump = false;
            isActing = false;
            randomAction = 0;
        }
    }
    void Dodge()
    {
        animator.SetBool("Dodge", true);
        if (isJump)
        {
            jumpCount += Time.deltaTime;
            if (target.transform.position.x > transform.position.x)
                body.velocity = new Vector2(-2, 8);
            else
                body.velocity = new Vector2(2, 8);
            if (jumpCount >= jumpTime)
            {
                jumpCount = 0;
                isJump = false;
            }
        }
        if (!isJump)
            if (IsGrounded())
            {
                body.velocity = new Vector2(0, -8);
            }
            else
            {
                if (target.transform.position.x > transform.position.x)
                    body.velocity = new Vector2(-1, -8);
                else
                    body.velocity = new Vector2(1, -8);
            }
        if (IsGrounded() && !isJump)
        {
            animator.SetBool("Dodge", false);
            isJump = false;
            isActing = false;
            randomAction = 0;
        }
    }
    void Walk()
    {
        animator.SetBool("Walk", true);
        actingCount += Time.deltaTime;
        if (actingCount > walkTime)
        {
            actingCount = 0;
            body.velocity = new Vector2(0, -8);
            animator.SetBool("Walk", false);
            animator.SetBool("WalkBack", false);
            isActing = false;
            randomAction = 0;
            return;
        }

        if (Mathf.Abs(target.transform.position.x - transform.position.x) > 3f)
        {
            if (Mathf.Abs(target.transform.position.x - transform.position.x) > 3.5f)
                animator.SetBool("WalkBack", false);
            if (target.transform.position.x > transform.position.x)
                body.velocity = new Vector2(3, -8);
            else
                body.velocity = new Vector2(-3, -8);
        }
        else
        {
            animator.SetBool("WalkBack", true);
            if (target.transform.position.x < transform.position.x)
                body.velocity = new Vector2(5, -8);
            else
                body.velocity = new Vector2(-5, -8);
        }
    }
    void Skill1()
    {
        animator.SetBool("Skill1", true);
        actingCount += Time.deltaTime;
        if (actingCount > skillTime)
        {
            actingCount = 0;
            animator.SetBool("Skill1", false);
            isActing = false;
            randomAction = 0;

            float velX = 8f;
            Vector3 posStart = transform.position;
            if (faceRight)
            {
                posStart += new Vector3(1f, -0.7f, 0);
                cloneSkill1 = GameObject.Instantiate(skill1);
                cloneSkill1.SetStart(posStart, velX, rightLim);
            }
            else
            {
                posStart -= new Vector3(1f, 0.7f, 0);
                cloneSkill1 = GameObject.Instantiate(skill1);
                cloneSkill1.SetStart(posStart, -velX, leftLim);
            }
            return;
        }
    }
    void Skill2()
    {
        animator.SetBool("Skill1", true);
        actingCount += Time.deltaTime;
        if (actingCount > skillTime)
        {
            actingCount = 0;
            animator.SetBool("Skill1", false);
            isActing = false;
            randomAction = 0;
            return;
        }
    }
    void CheckFace()
    {
        if (target.transform.position.x > transform.position.x)
        {
            faceRight = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            faceRight = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsColliderDodge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderDodge.bounds.center, colliderDodge.bounds.size, 0, Vector2.left, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
}
