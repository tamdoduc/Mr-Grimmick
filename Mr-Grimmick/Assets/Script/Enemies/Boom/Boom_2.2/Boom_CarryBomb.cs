using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_CarryBomb : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckWall;
    [SerializeField] LayerMask GroundLayer;

    private int healPoint = 1;
    public float range = 0, handleTime = 0;
    private bool faceRight = true;
    private Vector2 vel;
    private const float MaxVelocityX = 3.5f, turnTime = 2f, eps = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        vel = new Vector2(MaxVelocityX, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (healPoint)
        {
            case 1:
                Debug.Log(IsColliderWall());
                if (IsColliderWall())
                    CheckMove();
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
    void Detect()
    {
        if (transform.position.x >= target.transform.position.x)
            faceRight = false;
        else
            faceRight = true;
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
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckWall.bounds.center, colliderCheckWall.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
}
