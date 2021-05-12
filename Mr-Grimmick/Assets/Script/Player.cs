using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    Vector2 vel;

    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D collider2D;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask GroundLayer;

    [SerializeField] bool IsPressJump;
    [SerializeField] float timePressJump;

    // Start is called before the first frame update
    void Start()
    {
        IsPressJump = false;
        timePressJump = 0;
    }
    // Update is called once per frame
    void Update()
    {
        CheckCommand();
    }
    void CheckCommand()
    {
       // vel = new Vector2(0, 0);
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0)
        {
            vel.x = 7;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (h < 0)
        {
            vel.x = -7;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else vel.x = 0;

        if (h != 0)
            animator.SetFloat("Speed", Mathf.Abs(vel.x));
        else
            animator.SetFloat("Speed", 0);


        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsPressJump = false;
            timePressJump = 0;
        }

        if (!IsPressJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                IsPressJump = true;
            }
        }
        else
        {
            timePressJump+=0.03f;
             body.AddForce(new Vector2(0, 200));
            //body.velocity += new Vector2(0, 1000);
            if (timePressJump >= 2)
            {
                timePressJump = 0;
                IsPressJump = false;
            }
        }
        if (IsGrounded())
        {
            animator.SetBool("IsJumpt", false);
        }
        else
        {
            animator.SetBool("IsJumpt", true);
        }




        body.velocity = vel;
    }     
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
}


