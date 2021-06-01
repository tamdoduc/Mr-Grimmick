using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
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
        vel = new Vector2(MaxVelocityX, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnEdge)
            CheckMove();
        body.velocity = vel;
        anim.SetFloat("Speed", Mathf.Abs(vel.x));
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
}
