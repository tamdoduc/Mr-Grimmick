using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_CarryBullet : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckWall;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderBigBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float groundLim;

    private int healPoint = 1;
    private float range = 7, handleTime = 0, dieVelocity = 0.7f;
    public bool faceRight = false;
    private Vector2 vel;
    private const int score = 300;
    private const float MaxVelocityX = 3f, turnTime = 3f, eps = 0.3f, dieTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        vel = new Vector2(MaxVelocityX, -5);
        animator.SetBool("OnGround", true);
        CheckMove();
    }

    // Update is called once per frame
    void Update()
    {
        switch (healPoint)
        {
            case -1:
                dieState();
                break;
            case 0:
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + score);
                if (target.transform.position.x > transform.position.x)
                    dieVelocity = -dieVelocity;
                handleTime = 0;
                healPoint--;
                if (dieVelocity < 0)
                {
                    body.velocity = new Vector2(0, 0);
                    body.AddForce(Vector2.left * 3f, ForceMode2D.Impulse);
                    body.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
                }
                else
                {
                    body.velocity = new Vector2(0, 0);
                    body.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
                    body.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
                }
                break;
            case 1:
                CheckHit();
                if (IsColliderWall() || !IsNotEdge())
                {
                    vel.x = 0;
                    CheckMove();
                }
                else
                {
                    handleTime += Time.deltaTime;
                    if (handleTime > turnTime)
                    {
                        if (Mathf.Abs(transform.position.y - target.transform.position.y) < eps)
                        {
                            Detect();
                            CheckMove();
                        }
                        else
                        if (IsOutRange() && !IsGetClose())
                            CheckMove();
                        handleTime = 0;
                    }
                }
                body.velocity = vel;
                animator.SetFloat("Speed", Mathf.Abs(vel.x));
                break;
        }
    }
    void CheckHit()
    {
        if (IsColliderSkill())
        {
            healPoint--;
        }
    }
    void dieState()
    {
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
        animator.SetBool("Dead", true);
        transform.eulerAngles = new Vector3(180, 0, 0);
        colliderHead.isTrigger = true;
        colliderBody.isTrigger = true;
        handleTime += Time.deltaTime;
        if (handleTime > dieTime)
            if (dieVelocity < 0)
            {
                body.velocity = new Vector2(dieVelocity, 0);
                body.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
            }
            else
            {
                body.velocity = new Vector2(dieVelocity, 0);
                body.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
            }
        if (transform.position.y < groundLim)
            GameObject.Destroy(this.gameObject);
    }
    void Detect()
    {
        if (transform.position.x >= target.transform.position.x)
            faceRight = false;
        else
            faceRight = true;
    }
    void CheckMove()
    {
        faceRight = !faceRight;
        if (faceRight)
        {
            vel.x = MaxVelocityX;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            vel.x = -MaxVelocityX;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    bool IsOutRange()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) > range)
            return true;
        else
            return false;
    }
    bool IsGetClose()
    {
        bool closeLeft = (transform.position.x >= target.transform.position.x) && (body.velocity.x <= 0);
        bool closeRight = (transform.position.x <= target.transform.position.x) && (body.velocity.x >= 0);
        if (closeLeft || closeRight)
            return true;
        else
            return false;
    }
    bool IsColliderWall()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckWall.bounds.center, colliderCheckWall.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBigBody.bounds.center, colliderBigBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
}
