using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryBird : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckCarry;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float wakeRange, resetRange, startPositionX, endPositionX, posX, posY, camRange;
    [SerializeField] int goRight; //1 = gotoright -1 = gotoleft

    private int healPoint = 3, status = -1;
    private float handleTime = 0, maxVelocityY = 1f;
    private bool isActive = false;
    private const float prepareTime = 3.7f, flyTime = 2f, turnVelocityYTime = 0.5f, maxVelocityX = 2f;
    // Start is called before the first frame update
    void Start()
    {
        body.velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (healPoint)
        {
            case 0:
                break;
            default:
                if (isActive)
                {
                    Debug.Log(body.velocity.y);
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
    }
    void Waiting()
    {
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
            status = 3;
        }
    }
    void Carrying()
    {
        body.velocity = new Vector2(goRight * maxVelocityX, maxVelocityY);
        maxVelocityY = -maxVelocityY;
        //fly to end posX
        if (goRight * transform.position.x >= goRight * endPositionX)
        {
            colliderCheckCarry.isTrigger = true;
            status = 4;
        }
    }
    void PreFlyAway()
    {
        handleTime += Time.deltaTime;
        if (handleTime > flyTime)
        {
            maxVelocityY = Mathf.Abs(maxVelocityY);
            body.velocity = new Vector2(goRight * maxVelocityX, maxVelocityY);
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
        colliderCheckCarry.isTrigger = false;
        handleTime = 0;
    }
    bool IsCarry()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckCarry.bounds.center, colliderCheckCarry.bounds.size, 0, Vector2.down, 0.1f, playerLayer);
        return hit2D.collider != null;
    }
}
