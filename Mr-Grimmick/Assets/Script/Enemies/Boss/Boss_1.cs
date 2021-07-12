using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float triggerPoint, groundLim;
    [SerializeField] Boss_1_Boom boom;
    Boss_1_Boom booms;
    [SerializeField] AudioSource dieSE;
    [SerializeField] AudioSource hitSE;
    AudioSource cloneAudio;

    private int healPoint = 3, status = 2, bfstatus = 0, index = 0;
    private float handleTime = 0, dieVelocity = 0.7f, imCount = 0;
    public bool isActive = false, isIM = false, fUse = true;
    private Vector2 vel;
    private const int score = 5000;
    private const float MaxVelocityXRight = 1f, MaxVelocityXLeft = -2f, idleTime = 0.8f, prepareTime = 1.7f, hitTime = 0.8f, imTime = 2f, creTime = 2f, dieTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        vel = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch(healPoint)
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
                Destroy(cloneAudio.gameObject, 3);
                break;
            default:
                if (!isActive)
                {
                    CheckRange();
                }
                else
                {
                    if (!isIM)
                        CheckHit();
                    else
                    {
                        imCount += Time.deltaTime;
                        if (imCount > imTime)
                        {
                            isIM = false;
                            imCount = 0;
                        }
                    }
                    if (status != bfstatus)
                    {
                        handleTime = 0;
                        animator.SetInteger("Status", status);
                        bfstatus = status;
                    }
                    handleTime += Time.deltaTime;
                    body.velocity = vel;
                    switch(status)
                    {
                        case 0:
                            if (handleTime > idleTime)
                            {
                                vel.x = MaxVelocityXRight;
                                status = 1;
                            }
                            break;
                        case 1:
                            if (!IsNotEdge())
                            {
                                vel.x = 0;
                                status = 2;
                            }
                            break;
                        case 2:
                            if (handleTime > prepareTime)
                            {
                                status = 3;
                            }
                            break;
                        case 3:
                            if (handleTime > creTime)
                            {
                                handleTime = 0;
                                CreateBoom();
                            }
                            break;
                        case 4:
                            vel.x = MaxVelocityXLeft;
                            if (handleTime > hitTime)
                            {
                                vel.x = 0;
                                status = 0;
                            }
                            break;
                    }

                }
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
            status = 4;
            cloneAudio = AudioSource.Instantiate(hitSE);
            Destroy(cloneAudio.gameObject, 1);
        }
    }
    [SerializeField] UnlockStage unlock;

    void DieState()
    {
        unlock = GameObject.Instantiate(unlock);
        unlock.SetStage(1);
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
        transform.eulerAngles = new Vector3(180, 0, 0);
        handleTime += Time.deltaTime;
        colliderBody.isTrigger = true;
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
    void CheckRange()
    {
        if (target.transform.position.x < triggerPoint)
        {
            isActive = true;
        }
    }
    void CreateBoom()
    {
        float x = 3, y = 3;
        if (target.transform.position.x < transform.position.x)
            x = -3;
        while (true)
        {
            booms = GameObject.Instantiate(boom);
            booms.transform.position = this.gameObject.transform.position + new Vector3(0, 2, 0);
            booms.SetStartVel(4 + x * index / 2, y * index / 4);
            index++;
            Debug.Log(index);
            index %= 6;
            if (!fUse || index == 0)
                break;
        }
        fUse = false;

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
