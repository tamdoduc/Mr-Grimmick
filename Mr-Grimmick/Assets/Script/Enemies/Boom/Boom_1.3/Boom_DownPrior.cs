using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_DownPrior : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Transform target;
=======
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
>>>>>>> Stashed changes
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckThorn;
    [SerializeField] Collider2D colliderCheckNearThorn;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ThornLayer;
    [SerializeField] float distance, wakerange;
<<<<<<< Updated upstream
    [SerializeField] bool isJump = false;
    [SerializeField] int healPoint = 1;

    public bool grounded = true;
    private Vector2 vel;
    private float MaxVelocityXRight = 3.5f, MaxVelocityXLeft = -3.5f, MaxVelocityY = 7.5f, MaxGravity = -8f;
    private float detectTime = 0, jumpTime = 0;
    private bool isActive = false;
    private bool faceRight = true;
=======
    [SerializeField] float posX, posY, posZ;

    private int healPoint = 1;
    private float turnTime = 0, jumpTime = 0;
    private bool isActive = false, faceRight = true, grounded = true, isJump = false;
    private Vector2 vel;
    private const float MaxVelocityXRight = 3.5f, MaxVelocityXLeft = -3.5f, MaxVelocityY = 7.5f, MaxGravity = -8f;
>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        vel = new Vector2(MaxVelocityXRight, 0);
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        if (IsColliderThorn())
            GameObject.Destroy(this.gameObject);
        CheckRange();
        if (isActive)
        {
            body.velocity = vel;
            CheckMove();
            CheckJump();
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
=======
        switch (healPoint)
        {
            case 0:
                break;
            case 1:
                if (IsColliderThorn())
                    GameObject.Destroy(this.gameObject);
                CheckRange();
                if (isActive)
                {
                    body.velocity = vel;
                    CheckMove();
                    CheckJump();
                    if (turnTime < 2)
                    {
                        turnTime += Time.deltaTime;
                    }
                    else
                    {
                        CheckFace();
                        turnTime = 0;
                    }
                }
                break;
        }   
>>>>>>> Stashed changes
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
    void CheckJump()
    {
        if (IsGrounded())
            grounded = true;
        else
            grounded = false;
        animator.SetBool("Grounded", grounded);
        //Gravity
        if (grounded && IsNearThorn())
            isJump = true;

        if (isJump)
            Jump();
        //Gravity
        if (jumpTime == 0)
            if (IsGrounded())
            {
                vel.y = -1f;
            }
            else
            if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }

        //Setstate
        if (IsNearGrounded())
        {
            animator.SetBool("IsJump", false);
        }
        else
        {
            animator.SetBool("IsJump", true);
        }
    }
    void Jump()
    {
        jumpTime += Time.deltaTime;
        if (vel.x > 0 && vel.x >= MaxVelocityXRight)
            vel.x -= 1;
        if (vel.x < 0 && vel.x <= MaxVelocityXLeft)
            vel.x += 1;
        vel.y = MaxVelocityY;
        if (jumpTime >= 0.25f)
        {
            Debug.Log("e");
            isJump = false;
            jumpTime = 0;
        }
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
    bool IsNearThorn()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckNearThorn.bounds.center, colliderCheckNearThorn.bounds.size, 0, Vector2.down, 0.3f, ThornLayer);
        return hit2D.collider != null;
    }
    bool IsColliderThorn()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckThorn.bounds.center, colliderCheckThorn.bounds.size, 0, Vector2.down, 0.3f, ThornLayer);
        return hit2D.collider != null;
    }
}
