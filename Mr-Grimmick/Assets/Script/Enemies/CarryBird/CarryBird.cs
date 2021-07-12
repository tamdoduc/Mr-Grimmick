using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryBird : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float wakeRange, resetRange, startPositionX, endPositionX, posX, posY, camRange, posYEnd;
    [SerializeField] int goRight; //1 = gotoright -1 = gotoleft
    [SerializeField] EffectFlash effectPlash;
    EffectFlash cloneEffectFLash;
    [SerializeField] SelfDestruct selfDestruct;

    private int healPoint = 3, status = -1;
    private float handleTime = 0, maxVelocityY = 1f, eps = 0.1f;
    private bool isActive = false, moveToYLine = true;
    private const float prepareTime = 3.7f, flyTime = 2f, turnVelocityYTime = 1f, maxVelocityX = 2f;
    // Start is called before the first frame update
    void Start()
    {
        body.velocity = new Vector2(0, 0);
        target = GameObject.Find("Player").GetComponent<Transform>();
        posX = transform.position.x;
        posY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (healPoint)
        {
            case 0:
                selfDestruct = GameObject.Instantiate(selfDestruct);
                selfDestruct.transform.position = this.gameObject.transform.position;
                GameObject.Destroy(this.gameObject);
                break;
            default:
                if (isActive)
                {
                    CheckHit();
                    CheckOutRange();
                    animator.SetInteger("Status", status);
                    switch (status)
                    {
                        case 0:
                            Prepare();
                            break;
                        case 1:
                            Flying();
                            break;
                        case 2:
                            Waiting();
                            break;
                        case 3:
                            Carrying();
                            break;
                        case 4:
                            PreFlyAway();
                            break;
                    }
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
            if (healPoint > 0)
                if (cloneEffectFLash == null)
                {
                    cloneEffectFLash = GameObject.Instantiate(effectPlash);
                    cloneEffectFLash.SetTimeMax(0.5f);
                    cloneEffectFLash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                    cloneEffectFLash.Active();
                }
        }
    }
    void CheckInRange()
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) < wakeRange)
        {
            status = 0;
            isActive = true;
        }
    }
    void CheckOutRange()
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) > resetRange)
        {
            //check out cam
            if (Mathf.Abs(target.transform.position.x - startPositionX) < camRange)
                status = -1;
            else
                ResetBird();
        }
    }
    void Prepare()
    {
        handleTime += Time.deltaTime;
        if (handleTime > prepareTime)
        {
            status = 1;
            handleTime = 0;
        }
    }
    void Flying()
    {
        colliderHead.isTrigger = true;
        colliderBody.isTrigger = true;
        //fly to start posX
        if (goRight * transform.position.x < goRight * startPositionX)
            body.velocity = new Vector2(goRight * maxVelocityX, maxVelocityY);
        else
        {
            body.velocity = new Vector2(0, maxVelocityY);
            status = 2;
        }
        //turn velocityY
        handleTime += Time.deltaTime;
        if (handleTime > turnVelocityYTime)
        {
            maxVelocityY = -maxVelocityY;
            handleTime = 0;
        }
        if (transform.position.y + eps > posYEnd && transform.position.y - eps < posYEnd)
            moveToYLine = false;
        if (moveToYLine)
            if (transform.position.y > posYEnd)
            {
                transform.position -= new Vector3(0, 0.1f, 0);
            }
            else
            {
                transform.position += new Vector3(0, 0.1f, 0);
            }
    }
    void Waiting()
    {
        colliderHead.isTrigger = false;
        colliderBody.isTrigger = false;
        //turn velocityY
        handleTime += Time.deltaTime;
        if (handleTime > turnVelocityYTime)
        {
            maxVelocityY = -maxVelocityY;
            body.velocity = new Vector2(0, maxVelocityY);
            handleTime = 0;
        }
        if (IsCarry())
        {
            colliderBody.isTrigger = true;
            status = 3;
        }
    }
    void Carrying()
    {
        body.velocity = new Vector2(goRight * maxVelocityX, 0);
        maxVelocityY = -maxVelocityY;
        //fly to end posX
        if (goRight * transform.position.x >= goRight * endPositionX)
        {
            colliderHead.isTrigger = true;
            status = 4;
        }
    }
    void PreFlyAway()
    {
        handleTime += Time.deltaTime;
        if (handleTime > flyTime)
        {
            maxVelocityY = Mathf.Abs(maxVelocityY);
            body.velocity = new Vector2(goRight * maxVelocityX * 2, maxVelocityY * 2);
            status = 5;
        }
    }
    void ResetBird()
    {
        transform.position = new Vector3(posX, posY, 0);
        isActive = false;
        status = -1;
        animator.SetInteger("Status", status);
        body.velocity = new Vector2(0, 0);
        colliderHead.isTrigger = false;
        handleTime = 0;
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
}
