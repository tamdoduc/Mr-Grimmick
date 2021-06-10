using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    bool isMoving;
    bool isActiveTrap;

    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D boxCollider2D;

    [SerializeField] GameObject player;

    [SerializeField] float timeActived = 0;
    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        animator.SetBool("IsMoving", false);
        timeActived = 0;
        isActiveTrap = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            timeActived += Time.deltaTime;
            float range = Time.deltaTime * 5 / 2;
            if (timeActived < 1.5f)
            {
                this.transform.position += new Vector3(range, 0, 0);
            }
            else
            {
                if (isActiveTrap==false)
                {
                    isActiveTrap = true;
                    animator.SetBool("IsMoving", false);
                    animator.SetBool("IsActiveTrap", true);
                    boxCollider2D.enabled = false;
                }    
            }    
        }
            
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isMoving = true;
            animator.SetBool("IsMoving", true);
            timeActived = 0;
        }
    }
}
