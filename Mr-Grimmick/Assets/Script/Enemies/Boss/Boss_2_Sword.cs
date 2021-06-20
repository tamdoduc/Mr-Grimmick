using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2_Sword : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] LayerMask groundLayer;
    public float MaxVelocity = 5;
    private bool isJump = false;
    private float rotation = 0, handleTime;
    private Vector2 vel = new Vector2(0, 0);
    private Vector3 rol;
    private const float MaxVelocityY = 12f, MaxGravity = -8f, jumpTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        isJump = true;
        rol = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (isJump)
            CheckJump();
        else
            FallDown();
        if (IsGrounded())
        {
            vel.x = 0; vel.y = 0;
            transform.eulerAngles = new Vector3(0, rol.y, 180);
        }
        else
        {
            CheckMove();
            rotation += 10;
            transform.eulerAngles = new Vector3(0, rol.y, rotation);
        }
        body.velocity = vel;
    }
    void CheckMove()
    {
        if (MaxVelocity > 0)
        {
            if (vel.x < MaxVelocity)
            {
                vel.x += (Time.deltaTime * MaxVelocity) / (0.8f);
            }
            else
            if (vel.x > MaxVelocity)
            {
                vel.x = MaxVelocity;
            }
        }
        else
        {
            if (vel.x > MaxVelocity)
            {
                vel.x += (Time.deltaTime * MaxVelocity) / (0.8f);
            }
            else
           if (vel.x < MaxVelocity)
            {
                vel.x = MaxVelocity;
            }
        }
    }
    void CheckJump()
    {
        //  Jumping
        if (isJump)
        {
            handleTime += Time.deltaTime;
            vel.y = MaxVelocityY;
            if (handleTime >= jumpTime)
            {
                handleTime = 0;
                isJump = false;
            }
        }
    }
    void FallDown()
    {
        if (handleTime == 0)
            if (IsGrounded())
            {
                vel.y = -1f;
            }
            else if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hit2D.collider != null;
    }
}
