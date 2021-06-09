using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Jump : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] LayerMask GroundLayer;
    
   
    public float activeDistanceX, dis = 0;
    public float posX, posY, posZ;
    public bool isNotEdge = false;

    private int healPoint = 1;
    private float detectTime = 0.001f, handleTime = 0;
    private bool isActive = false, faceRight = true, isJump = false;
    private Vector2 vel;
    private const float MaxVelocityXRight = 3.5f, MaxVelocityXLeft = -3.5f, MaxVelocityY = 7.5f, MaxGravity = -8f, resetTime = 1.7f, jumpTime = 0.37f ;
    // Start is called before the first frame update
    void Start()
    {
        vel = new Vector2(MaxVelocityXRight, -1);
    }

    // Update is called once per frame
    void Update()
    {
        switch (healPoint)
        {
            case 0:
                break;
            case 1:
                if (isActive)
                {
                    if (IsGrounded()) // Check state jump
                    {
                        animator.SetBool("IsJump", false);
                        vel.y = -1;
                    }
                    else
                    {
                        animator.SetBool("IsJump", true);
                    }
                    body.velocity = vel;
                    if (!IsNotEdge())
                    {
                        if (!isJump && IsGrounded())
                        {
                            Debug.Log("Jump");
                            isJump = true;
                        }
                        else
                            CheckJump();
                    }
                    else
                    {
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
                }
                else
                    CheckRange();
                break;
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
    void CheckRange()
    {
        dis = Mathf.Abs(this.transform.position.x - target.transform.position.x);
        if (activeDistanceX >= Mathf.Abs(this.transform.position.x - target.transform.position.x))
        {
            CheckFace();
            Debug.Log("?");
            isActive = true;
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
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
}