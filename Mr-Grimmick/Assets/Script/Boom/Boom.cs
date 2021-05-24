using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform target;
    [SerializeField] Animator anim;
    [SerializeField] Boom boom;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckTop;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ScrollBarLeft;
    [SerializeField] LayerMask ScrollBarRight;
    [SerializeField] float wakerange;

    public float distance;
    bool isActive = false, fly = false, delayTime = false, faceRight = true;
    public bool colliderWall = false, isOnEdge = false, wallJump = false, grounded = true, downGround = true, edgeJump = false;
    private float detectTime = 0, jumpTime = 0;
    private Vector2 vel = new Vector2(0, 0);
    int triggerCount = 15;

    float MaxVelocityXRight = 4f;
    float MaxVelocityXLeft = -4f;
    float MaxVelocityY = 7.5f;
    float MaxGravity = -8f;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        boom = gameObject.GetComponent<Boom>();
        body = gameObject.GetComponent<Rigidbody2D>();

    }
    
    // Update is called once per frame
    void Update()
    {
        //check tam hoat dong
        CheckRange();

        if (isActive)
        {
            body.velocity = vel;
            if (!fly)
            {
                if (!downGround)
                    if (vel.y >= MaxGravity)
                    {
                        vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
                    }
                CheckMove();
                CheckJump();
            }
            else
                FlyMode();

            ReSetPerFrame();
            CheckScrollBar();
            //tim muc tieu
            if (detectTime < 2)
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
            body.velocity = new Vector2(0, 0);

    }

    void CheckMove()
    {
        if (faceRight)
        {
            if (vel.x < MaxVelocityXRight)
            {
                vel.x += (Time.deltaTime * MaxVelocityXRight) / (0.5f);  //    0.5 là thời gian cần để đạt max     
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
                vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.5f);  //  0.5 là thời gian cần để đạt max       
            }
            else
           if (vel.x < MaxVelocityXLeft)
            {
                vel.x = MaxVelocityXLeft;
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        
        anim.SetFloat("Speed",Mathf.Abs(vel.x));
    }
    void CheckJump()
    {
        if (IsGrounded() || IsOnScrollBarLeft() || IsOnScrollBarRight())
            grounded = true;
        else
            grounded = false;
        anim.SetBool("Grounded", grounded);

        if (!wallJump && !edgeJump)
        {
            if (grounded)
            {
                if (colliderWall)
                    wallJump = true;
                else
                    if (isOnEdge && distance > 3)
                    edgeJump = true;
            }
        }
        else
        if (edgeJump)
        {
            EdgeJump();
        }
        else
        {
            WallJump();
        }

        //Gravity
        if (jumpTime == 0)
            if (IsGrounded() || IsOnScrollBarLeft() || IsOnScrollBarRight())
            {
                vel.y = -1f;
            }
            else
            if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }

        //Setstate
        if (IsNearGrounded() || IsOnScrollBarLeft() || IsOnScrollBarRight())
        {
            anim.SetBool("IsJump", false);
        }
        else
        {
            anim.SetBool("IsJump", true);
        }
    }
    void WallJump()
    {
        //colliderWall jump
        {
            jumpTime += Time.deltaTime;
            vel.y = MaxVelocityY;
            Debug.Log(jumpTime);
            if (jumpTime >= 0.3f || !colliderWall)
            {
                jumpTime = 0;
                wallJump = false;
                Debug.Log(triggerCount);
                if (target.transform.position.y - transform.position.y > 0.2f)
                    triggerCount--;
                else
                    triggerCount = 15;
            }
        }
    }
    void EdgeJump()
    {
        //Edge jump
        jumpTime += Time.deltaTime;
        if (vel.x > 0 && vel.x >= MaxVelocityXRight)
            vel.x -= 1;
        if (vel.x < 0 && vel.x <= MaxVelocityXLeft)
            vel.x += 1;
        vel.y = MaxVelocityY;
        Debug.Log(jumpTime);
        if (jumpTime >= 0.25f || IsColliderTop())
        {
            Debug.Log("e");
            jumpTime = 0;
            edgeJump = false;
            Debug.Log(triggerCount);
            if (target.transform.position.y - transform.position.y > 0.2f)
                triggerCount--;
            else
                triggerCount = 15;
        }
    }
    void CheckRange()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance < wakerange)
            isActive = true;

        if (distance > wakerange)
        {
            isActive = false;
            //Respawn();
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
        //Transform
        if (triggerCount <= 0)
        {
            fly = true;
            vel.x = 0;
            vel.y = -10f;
            anim.SetFloat("Speed", 0);
            anim.SetBool("IsJump", false);
        }
    }
    void FlyMode()
    {
        if (delayTime)
        {
            if (IsGrounded() || IsOnScrollBarLeft() || IsOnScrollBarRight())
                grounded = true;
            else
                grounded = false;
            anim.SetBool("Grounded", grounded);
            Debug.Log(target.transform.position.y - 0.12f <= transform.position.y);
            CheckMove();
            if (target.transform.position.y <= transform.position.y && !grounded)
            {
                if (vel.y >= MaxGravity)
                {
                    vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
                }
            }
            else
                if (target.transform.position.y - 0.12f <= transform.position.y)
                vel.y = -1;
            else
            {
                if (vel.y < MaxVelocityY)
                {
                    Debug.Log("GetHigh");
                    vel.y += (Time.deltaTime * MaxVelocityY) / (0.4f); 
                }
                else
                if (vel.y > MaxVelocityY)
                {
                    vel.y = MaxVelocityY;
                }
            }
        }
        else
        {
            StartCoroutine(Prepare());
        }
    }
    public IEnumerator Prepare()
    {
        yield return new WaitForSeconds(4f);
        delayTime = true;
        anim.SetBool("Transform", fly);

    }
    public void Respawn()
    {
        GameObject.Destroy(boom.gameObject);
    }
    void ReSetPerFrame()
    {
        MaxVelocityXLeft = -5;
        MaxVelocityXRight = 5;
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
    bool IsNearGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.3f, GroundLayer);
        return hit2D.collider != null;
    }
    void CheckScrollBar()
    {
        if (IsOnScrollBarLeft())
        {
            MaxVelocityXRight = 3;
            MaxVelocityXLeft = -8;
        }
        if (IsOnScrollBarRight())
        {
            MaxVelocityXRight = 8;
            MaxVelocityXLeft = -3;
        }
    }
    bool IsOnScrollBarLeft()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, ScrollBarLeft);
        return hit2D.collider != null;
    }
    bool IsOnScrollBarRight()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, ScrollBarRight);
        return hit2D.collider != null;
    }
    bool IsColliderTop()
    {
        RaycastHit2D hit1 = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckTop.bounds.size, 0, Vector2.up, 0.1f, GroundLayer);
        RaycastHit2D hit2 = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.up, 0.18f, ScrollBarRight);
        RaycastHit2D hit3 = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.up, 0.18f, ScrollBarLeft);
        return hit1.collider != null || hit2.collider != null || hit3.collider != null;
    }
}
