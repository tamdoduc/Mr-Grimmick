using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    IDLELEFT,
    IDLERIGHT,
    WALKLEFT,
    WALKRIGHT,
    JUMPTLEFT,
    JUMPRIGHT,
    FAINT,
    DIE
}
struct InfoJumpt
{
    public float timeJumpt;
    const float maxTimeJumpt = 1;
}

public class Player : MonoBehaviour
{
    int state;
    Vector2 vel;
    Rigidbody2D body;
    public Animator animator;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        body = this.gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckMove();
    }
    void CheckMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            body.velocity = new Vector2(5, 0);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (h < 0)
        {
            body.velocity = new Vector2(-5, 0);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (h != 0)
            animator.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        else
            animator.SetFloat("Speed", 0);
    }     
}


