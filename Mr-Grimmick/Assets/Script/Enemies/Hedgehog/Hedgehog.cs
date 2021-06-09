using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckCarry;
    [SerializeField] Collider2D colliderCheckEdge;
    [SerializeField] LayerMask GroundLayer;

    private int healPoint = 2;
    private Vector2 vel;
    private bool faceRight = true, isOnEdge = false, isFlip = false;
    private float handleTime = 0;
    private const float MaxVelocityX = 1f, turnTime = 0.2f;
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
            case 0:
                break;
            case 1:
                if (!isFlip)
                {
                    body.velocity = new Vector2(0, 0);
                    colliderCheckCarry.isTrigger = false;
                    isFlip = true;
                    anim.SetBool("Flip", true);
                }
                break;
            case 2:
                if (!IsNotEdge() && !isOnEdge)
                {
                    isOnEdge = true;
                    anim.SetBool("Edge", true);
                    CheckMove();
                }
                else
                {
                    if (isOnEdge)
                    {
                        //delay for turn
                        handleTime += Time.deltaTime;
                        if (handleTime > turnTime)
                        {
                            isOnEdge = false;
                            handleTime = 0;
                            anim.SetBool("Edge", false);
                            if (!faceRight)
                                transform.eulerAngles = new Vector3(0, 0, 0);
                            else
                                transform.eulerAngles = new Vector3(0, 180, 0);
                        }
                    }
                }
                body.velocity = vel;
                break;
        }
    }
    void CheckMove()
    {
        if (faceRight)
        {
            vel.x = MaxVelocityX;  
        }
        else
        {
            vel.x = -MaxVelocityX;  
        }

        faceRight = !faceRight;

    }
    bool IsNotEdge()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckEdge.bounds.center, colliderCheckEdge.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
}
