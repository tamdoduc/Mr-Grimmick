using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Electric : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Transform target;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckCarry;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask PlayerLayer;
    [SerializeField] bool isCarry = false, isDSM = false;
    [SerializeField] int healPoint = 1;

    public float wakepositionX;
    public float posX, posY, posZ;
    public bool isNotEdge = false;
    private Vector2 vel;
    private float detectTime = 0.001f, handleTime = 0;
    private bool isActive = false, inRange = false, faceRight = true;
    private const float MaxVelocityXRight = 3.5f, MaxVelocityXLeft = -3.5f, resetTime = 1.7f, idleTime = 1.5f, DSMTime = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            isCarry = IsCarriedPlayer();
            animator.SetBool("CP", isCarry);
            if (isCarry)
            {
                IdleState();
            }
            else
            if (!isDSM)
            {
                body.velocity = vel;

                //handleSkill
                if (detectTime == 0 || handleTime > 0)
                    handleTime += Time.deltaTime;

                if (handleTime >= idleTime)
                {
                    handleTime = 0;
                    vel = new Vector2(0, vel.y);
                    body.velocity = vel;
                    isDSM = true;
                    animator.SetBool("DSM", true);
                    animator.SetFloat("Speed", 0);
                    return;
                }
                //
                if (!IsNotEdge())
                    faceRight = !faceRight;
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
            else
                StopState();
        }
        else
            if (inRange)
            CheckActive();
        else
            CheckRange();
    }

    void IdleState()
    {
        body.velocity = new Vector2(0, vel.y);
        animator.SetFloat("Speed", 0);
    }
    void StopState()
    {
        handleTime += Time.deltaTime;
        if (handleTime > DSMTime)
        {
            detectTime = 0.001f;
            handleTime = 0;
            isDSM = false;
            animator.SetBool("DSM", false);
        }
    }
    void DieState()
    {
        GameObject.Destroy(this.gameObject);
    }
    void CheckRange()
    {
        if (wakepositionX >= target.transform.position.x)
        {
            this.transform.position = new Vector3(posX, posY, posZ);
            CheckFace();
            inRange = true;
            body.velocity = new Vector2(0, -8f);
        }
    }
    void CheckActive()
    {
        if (IsGrounded())
        {
            isActive = true;
            vel.y = -1;
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
    bool IsCarriedPlayer()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckCarry.bounds.center, colliderCheckCarry.bounds.size, 0, Vector2.down, 0.1f, PlayerLayer);
        return hit2D.collider != null;
    }
}
