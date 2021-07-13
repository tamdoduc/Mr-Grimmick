using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] Collider2D colliderHead;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] float camRange, groundLim;
    [SerializeField] AudioSource dieSE;
    AudioSource cloneAudio;

    private int healPoint = 1, score = 120;
    private float handleTime = 0, posX, posY, dieVelocity = 0.7f;
    private bool faceRight = true;
    private Vector2 vel;
    private const float MaxVelocityX = 1.5f, dieTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        vel = new Vector2(MaxVelocityX, -1);
        posX = transform.position.x;
        posY = transform.position.y;
        transform.position = new Vector3(posX, posY, 0);
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
                if (IsColliderSkill())
                    healPoint--;
                if (!IsNotEdge())
                    CheckMove();
                body.velocity = vel;
                break;
        }
    }
    void DieState()
    {
        animator.SetBool("Dead", true);
        GameObject g = gameObject.transform.GetChild(1).gameObject;
        g.layer = 8;
        handleTime += Time.deltaTime;
        colliderHead.isTrigger = true;
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
