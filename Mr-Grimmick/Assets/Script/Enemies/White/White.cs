using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] CheckTop checkTop;
    [SerializeField] float wakeRange, resetRange, groundLim;
    private bool isActive = false, faceRight = true, chaseMode = false, hurt = false, onHead = false;
    private float handleTime = 0, chaseCount = 0;
    private Vector2 vel = new Vector2(0, -1);
    private const float MaxVelocityXRight = 5.5f, MaxVelocityXLeft = -5.5f, walkTime = 4f, imTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        body.velocity = new Vector2(0, 0);
        target = GameObject.Find("Player").GetComponent<Transform>();
        checkTop.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckHit();
            if (IsGrounded())
                vel.y = -8f;
            if (hurt)
            {
                handleTime += Time.deltaTime;
                if (handleTime > imTime)
                    hurt = false;
                animator.SetBool("Hurt", hurt);
                return;
            }
            if (!IsColliderHead())
            {
                onHead = false;
                CheckOutRange();
                if (chaseMode)
                {
                    CheckFace();
                    CheckMove();
                }
                else
                {
                    chaseCount += Time.deltaTime;
                    if (chaseCount > walkTime)
                    {
                        chaseMode = true;
                    }
                    if (!IsNotEdge())
                    {
                        faceRight = !faceRight;
                        if (faceRight)
                            transform.eulerAngles = new Vector3(0, 0, 0);
                        else
                            transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                    if (faceRight)
                        vel.x = 1;
                    else
                        vel.x = -1;
                }
            }
            else
            {
                if (!onHead)
                    PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 10);
                if (vel.x > 0)
                    vel.x = Mathf.Max(0, vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.8f));
                else
                    vel.x = Mathf.Min(0, vel.x += (Time.deltaTime * MaxVelocityXRight) / (0.8f));
            }
            animator.SetFloat("Speed", Mathf.Abs(vel.x));
            body.velocity = vel;
        }
        else
            CheckInRange();
    }
    void CheckHit()
    {
        if (IsColliderSkill() && !hurt)
        {
            hurt = true;
            handleTime = 0;
            chaseCount = 0;
            if (faceRight)
                body.AddForce(Vector2.left * 2f, ForceMode2D.Impulse);
            else
                body.AddForce(Vector2.right * 2f, ForceMode2D.Impulse);
        }
    }
    void CheckInRange()
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) < wakeRange)
        {
            CheckFace();
            isActive = true;
        }
    }
    void CheckOutRange()
    {
        if (Mathf.Abs(target.transform.position.x - transform.position.x) > resetRange
            || Mathf.Abs(target.transform.position.y - transform.position.y) > resetRange
            || target.transform.position.y < groundLim)
        {
            GameObject.Destroy(this.gameObject);
        }
       
    }
    void CheckFace()
    {
        if (target.transform.position.x > transform.position.x)
        {
            faceRight = true;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (target.transform.position.x < transform.position.x)
        {
            faceRight = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    void CheckMove()
    {
        if (faceRight)
        {
            if (vel.x < MaxVelocityXRight)
            {
                vel.x += (Time.deltaTime * MaxVelocityXRight) / (0.9f);
            }
            else
            if (vel.x > MaxVelocityXRight)
            {
                vel.x = MaxVelocityXRight;
            }
        }
        else
        {
            if (vel.x > MaxVelocityXLeft)
            {
                vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.9f);
            }
            else
           if (vel.x < MaxVelocityXLeft)
            {
                vel.x = MaxVelocityXLeft;
            }
        }
    }
    bool IsColliderHead()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderHead.bounds.center, colliderHead.bounds.size, 0, Vector2.up, 0.1f, playerLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
}
