using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Harmless : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float camRange, limLeft, limRight, groundLim;
    [SerializeField] AudioSource dieSE;
    AudioSource cloneAudio;
    private int healPoint = 1;
    private float handleTime = 0, dieVelocity = 0.7f;
    private bool faceRight = false, turn = false, stop = false;
    private Vector2 vel;
    private const int score = 800;
    private const float MaxVelocityX = 1.5f, walkTime = 3f, idleTime = 1f, resetTime = 2f, dieTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        vel = new Vector2(0, 0);
        target = GameObject.Find("Player").GetComponent<Transform>();
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
                cloneAudio = AudioSource.Instantiate(dieSE);
                Destroy(cloneAudio.gameObject, 1);
                break;
            case 1:
                CheckHit();
                body.velocity = vel;
                if (IsInRange())
                {
                    handleTime = 0;
                    vel.x = 0;
                    CheckFace();
                    animator.SetBool("Watch", true);
                }
                else
                {
                    animator.SetBool("Watch", false);
                    handleTime += Time.deltaTime;
                    if (handleTime > walkTime && !stop)
                    {
                        stop = true;
                        handleTime = 0;
                    }

                    if (stop)
                    {
                        vel.x = 0;
                        animator.SetBool("Idle", true);
                        handleTime += Time.deltaTime;
                        if (handleTime > idleTime)
                            animator.SetBool("Blink", true);
                        if (handleTime > resetTime)
                        {
                            handleTime = 0;
                            animator.SetBool("Blink", false);
                            animator.SetBool("Idle", false);
                            stop = false;
                        }
                    }
                    else
                    {
                        CheckMove();
                        if (turn)
                        {
                            faceRight = !faceRight;
                            turn = false;
                        }
                    }
                    
                }
                break;
        }
    }
    void CheckFace()
    {
        if (target.transform.position.x > transform.position.x)
        {
            faceRight = true;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            faceRight = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    void CheckMove()
    {
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
        if (transform.position.x < limLeft)
            faceRight = true;

        if (transform.position.x > limRight)
            faceRight = false;
    }
    void CheckHit()
    {
        if (IsColliderSkill())
        {
            healPoint--;
        }
    }
    void DieState()
    {
        animator.SetBool("Dead", true);
        transform.eulerAngles = new Vector3(180, 0, 0);
        var sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = 0;
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
    bool IsInRange()
    {
        if (target.transform.position.x > limLeft - 2 && target.transform.position.x < limRight + 2)
            return true;
        else
            return false;
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBody.bounds.center, colliderBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
}
