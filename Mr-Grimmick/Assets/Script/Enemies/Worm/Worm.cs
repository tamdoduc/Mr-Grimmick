using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    [SerializeField] Worm worm;
=======
>>>>>>> Stashed changes
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D body;

    private bool faceRight = true;

    private float MaxVelocityX = 1.5f;
    private Vector2 vel;
    public bool isOnEdge = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
<<<<<<< Updated upstream
        worm = gameObject.GetComponent<Worm>();
=======
>>>>>>> Stashed changes
        body = gameObject.GetComponent<Rigidbody2D>();
=======
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] LayerMask GroundLayer;

    private int healPoint;
    private bool faceRight = true;
    private Vector2 vel;
    private const float MaxVelocityX = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
>>>>>>> Stashed changes
        vel = new Vector2(MaxVelocityX, 0);
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        if (isOnEdge)
            CheckMove();
        body.velocity = vel;
        anim.SetFloat("Speed", Mathf.Abs(vel.x));
=======
        switch (healPoint)
        {
            case 0:
                break;
            case 1:
                if (!IsNotEdge())
                    CheckMove();
                body.velocity = vel;
                animator.SetFloat("Speed", Mathf.Abs(vel.x));
                break;
        }   
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
>>>>>>> Stashed changes
}
