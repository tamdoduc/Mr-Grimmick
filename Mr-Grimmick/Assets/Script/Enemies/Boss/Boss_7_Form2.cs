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
    [SerializeField] Boss_7_Skill2 skill2;
    Boss_7_Skill2 cloneSkill2;
    [SerializeField] Boss_7_Die dieEffect;
    Boss_7_Die cloneDieEffect;
    [SerializeField] Boss_7_Star starEffect;
    Boss_7_Star cloneStarEffect;

    public bool isActive = false, isIM = false, isActing = false, faceRight = false, usedSkill = false, isJump = false;
    public int healPoint = 4, randomAction = 0, randomWalk = 0;
    private float randomCount = 0, imCount = 0, actingCount = 0, transformCount = 0, jumpCount = 0, dieCount = 0;
    private const float randomTime = 1.5f, skillTime = 0.5f, prepareTime = 2f, imTime = 0.5f, walkTime = 2f, jumpTime = 0.35f, dieTime = 3f;
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
            case -1:
                DieState();
                break;
            case 0:
                dieCount += Time.deltaTime;
                if (dieCount > imTime)
                {
                    animator.SetBool("Dead", true);
                    dieCount += Time.deltaTime;
                    healPoint--;
                    DieEffect();
                }
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
                                randomAction = Random.Range(1, 6);
                                if (randomAction < 3)
                                    isJump = true;
                                if (randomAction == 3)
                                    randomWalk = Random.Range(1, 3);
                                randomCount = 0;
                                isActing = true;
                            }
                        }
                        else
                        {
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
        target = GameObject.Find("Player").GetComponent<Transform>();
        transform.position = pos;
        if (transform.position.x < target.position.x)
        {
            faceRight = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    void DieEffect()
    {
        cloneDieEffect = GameObject.Instantiate(dieEffect);
        cloneDieEffect.SetPos(transform.position + new Vector3(-0.5f, -0.5f, 0), dieTime - imTime);

        cloneDieEffect = GameObject.Instantiate(dieEffect);
        cloneDieEffect.SetPos(transform.position + new Vector3(0.5f, 0.5f, 0), dieTime - imTime);

        cloneDieEffect = GameObject.Instantiate(dieEffect);
        cloneDieEffect.SetPos(transform.position + new Vector3(-0.5f, 0.5f, 0), dieTime - imTime);

        cloneDieEffect = GameObject.Instantiate(dieEffect);
        cloneDieEffect.SetPos(transform.position + new Vector3(0.5f, -0.5f, 0), dieTime - imTime);
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
        dieCount += Time.deltaTime;
        if (dieCount > dieTime)
        {
            cloneStarEffect = GameObject.Instantiate(starEffect);
            cloneStarEffect.SetPos(transform.position);
            GameObject.Destroy(this.gameObject);
        }
    }
    void CheckHit()
    {
        if (IsColliderSkill() && !isIM)
        {
            imCount = 0;
            healPoint--;
            if (healPoint >= 0)
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
        if (randomWalk == 1)
        {
            body.velocity = new Vector2(3, -8);
        }
        else
        {
            body.velocity = new Vector2(-3, -8);

        }
        if (Mathf.Abs(target.transform.position.x - transform.position.x) > 3f)
        {
                animator.SetBool("WalkBack", false);
        }
        else
        {
            animator.SetBool("WalkBack", true);
            body.velocity = new Vector2(0, -8);
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
                posStart += new Vector3(1.5f, -1f, 0);
                cloneSkill1 = GameObject.Instantiate(skill1);
                cloneSkill1.SetStart(posStart, velX, rightLim);
            }
            else
            {
                posStart -= new Vector3(1.5f, 1f, 0);
                cloneSkill1 = GameObject.Instantiate(skill1);
                cloneSkill1.SetStart(posStart, -velX, leftLim);
            }
            return;
        }
    }
    void Skill2()
    {
        animator.SetBool("Skill2", true);
        actingCount += Time.deltaTime;
        if (actingCount > skillTime)
        {
            actingCount = 0;
            animator.SetBool("Skill2", false);
            isActing = false;
            randomAction = 0;
            float velX = 4f;
            Vector3 posStart = transform.position;
            if (faceRight)
            {
                for (int i = 1; i < 6; i++)
                {
                    posStart += new Vector3(0.1f * i, 0.1f * i , 0);
                    cloneSkill2 = GameObject.Instantiate(skill2);
                    cloneSkill2.SetStart(posStart + new Vector3(0.5f, 0, 0), velX + (0.5f * i));
                }
            }
            else
            {
                for (int i = 1; i < 6; i++)
                {
                    posStart += new Vector3(-0.1f * i, 0.1f * i, 0);
                    cloneSkill2 = GameObject.Instantiate(skill2);
                    cloneSkill2.SetStart(posStart - new Vector3(0.5f, 0, 0), -velX - (0.5f * i));
                }
            }
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
