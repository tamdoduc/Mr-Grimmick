using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckWall;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool faceRight;
    [SerializeField] float triggerPosX;

    private Vector2 vel;
    private bool isActive = false;
    private const float MaxVelocityX = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (!IsNotEdge() || IsColliderWall())
                CheckMove();
            body.velocity = vel;
        }
        else
        {
            CheckActive();
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

        faceRight = !faceRight;

    }
    void CheckActive()
    {
        if (transform.position.x < triggerPosX)
        {
            isActive = true;
            if (faceRight)
                vel = new Vector2(MaxVelocityX, 0);
            else
                vel = new Vector2(-MaxVelocityX, 0);
        }
    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
    bool IsColliderWall()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckWall.bounds.center, colliderCheckWall.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
}
