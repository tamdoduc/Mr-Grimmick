using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_7 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float groundLim, leftLim, rightLim, leftPosX, rightPosX;
    [SerializeField] EffectFlash effectFlash;
    EffectFlash cloneEffectFlash;
    [SerializeField] Boss_7_Skill1 skill1;
    Boss_7_Skill1 cloneSkill1;
    [SerializeField] Boss_7_Form2 form2;
    Boss_7_Form2 cloneForm2;

    private bool isActive = false, isLeft = false, isSkilling = false, isIM = false;
    private int healPoint = 3;
    private float posY, changeSideCount = 0.1f, skillCount = 0, imCount = 0, skillDelay = 0, transformCount = 0;
    private const float changeSideTime = 4f, skillTime = 2f, prepareSkillTime = 0.8f, imTime = 0.5f, transformTime = 2.5f;
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        posY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (isSkilling)
            {
                float velX = 8f;
                Vector3 posStart = transform.position;
                skillDelay += Time.deltaTime;
                if (skillDelay > 0.08f && skillCount < 4)
                {
                    if (isLeft)
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
                    skillDelay = 0;
                    skillCount++;
                }
            }
            switch (healPoint)
            {
                case 0:
                    DieState();
                    break;
                default:
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
                    }


                    if (changeSideCount == 0)
                    {
                        if (isLeft)
                            transform.position = new Vector3(leftPosX, posY, 0);
                        else
                            transform.position = new Vector3(rightPosX, posY, 0);

                        if (cloneEffectFlash == null)
                        {
                            cloneEffectFlash = GameObject.Instantiate(effectFlash);
                            cloneEffectFlash.SetTimeMax(0.8f);
                            cloneEffectFlash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                            cloneEffectFlash.Active();
                        }
                    }

                    if (changeSideCount > prepareSkillTime)
                    {
                        isSkilling = true;
                    }

                    if (!isIM) changeSideCount += Time.deltaTime;

                    if (changeSideCount > skillTime + prepareSkillTime)
                    {
                        transform.position = new Vector3(leftLim, groundLim - 10, 0);
                        isSkilling = false;
                        skillCount = 0;
                        skillDelay = 0;
                    }
                    if (changeSideCount > changeSideTime)
                    {
                        isLeft = !isLeft;
                        if (isLeft)
                            transform.eulerAngles = new Vector3(0, 0, 0);
                        else
                            transform.eulerAngles = new Vector3(0, 180, 0);
                        changeSideCount = 0;
                    }
                    break;
            }
        }
        else
            CheckInRange();
    }

    void CheckInRange()
    {
        if (target.transform.position.x > leftLim)
        {
            isActive = true;
        }
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
            {
                cloneForm2 = GameObject.Instantiate(form2);
                cloneForm2.SetStartPos(transform.position);
                if (cloneEffectFlash == null)
                {
                    cloneEffectFlash = GameObject.Instantiate(effectFlash);
                    cloneEffectFlash.SetTimeMax(3f);
                    cloneEffectFlash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                    cloneEffectFlash.Active();
                }
            }
        }
    }
    void DieState()
    {
        transformCount += Time.deltaTime;
        if (transformCount > transformTime)
            GameObject.Destroy(this.gameObject);
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
}
