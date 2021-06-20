using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float groundLim;
    [SerializeField] CheckTop checktop;

    public bool faceRight = true;
    private int healPoint = 2;
    private Vector2 vel;
    private bool isOnEdge = false, isFlip = false, isIM = false;
    private float handleTime = 0, dieVelocity = 0.7f;
    private const int score = 380, scorefall = 3000;
    private const float MaxVelocityX = 1f, turnTime = 0.2f, imTime = 2f, flipTime = 0.2f, dieTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        vel = new Vector2(MaxVelocityX, -8);
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
                if (!isIM)
                    CheckHit();
                else
                {
                    handleTime += Time.deltaTime;
                    if (handleTime > imTime)
                    {
                        isIM = false;
                    }
                }
                if (transform.position.y < groundLim)
                {
                    PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + scorefall);
                    groundLim -= 100;
                }
                if (!isFlip)
                {
                    Flip();
                }
                break;
            default:
                if (!isIM)
                    CheckHit();
                if (!IsNotEdge() && !isOnEdge)
                {
                    isOnEdge = true;
                    animator.SetBool("Edge", true);
                    CheckMove();
                }
                else
                {
                    if (isOnEdge)
                    {
                        //delay for turn
                        handleTime += Time.deltaTime;
                        if (handleTime > turnTime)
                        {
                            isOnEdge = false;
                            handleTime = 0;
                            animator.SetBool("Edge", false);
                            if (!faceRight)
                                transform.eulerAngles = new Vector3(0, 0, 0);
                            else
                                transform.eulerAngles = new Vector3(0, 180, 0);
                        }
                    }
                }
                body.velocity = vel;
                break;
        }
    }
    void CheckHit()
    {
        if (IsColliderSkill())
        {
            healPoint--;
            isIM = true;
            handleTime = 0;
            GameObject g = gameObject.transform.GetChild(1).gameObject;
            g.layer = 8;
        }
    }
    void DieState()
    {
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
    void Flip()
    {
        if (handleTime > flipTime)
        {
            body.velocity = new Vector2(0, -4);
            isFlip = true;
        }
        else
        {
            colliderHead.isTrigger = false;
            if (faceRight)
                transform.eulerAngles = new Vector3(0, 180, 0);
            animator.SetBool("Flip", true);
            body.velocity = new Vector2(0, 2);
        }
    }
    void CheckMove()
    {
        if (faceRight)
        {
            vel.x = MaxVelocityX;  
        }
        else
        {
            vel.x = -MaxVelocityX;  
        }

        faceRight = !faceRight;

    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
}
