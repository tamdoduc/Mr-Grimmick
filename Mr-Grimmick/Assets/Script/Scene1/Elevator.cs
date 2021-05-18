using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    bool isActive;
    bool isMoving;
    bool isActiveTrap;
    bool isBelowPlayer;

    [SerializeField] Animator animator;
    [SerializeField] BoxCollider2D boxCollider2D;

    [SerializeField] Player player;

    [SerializeField] float timeActived = 0;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        isMoving = false;
        animator.SetBool("isMoving", false);
        timeActived = 0;
        isActiveTrap = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (timeActived < 2)
            {
                timeActived += Time.deltaTime;
                float range = Time.deltaTime * 5 /2;
                this.transform.position += new Vector3(range, 0, 0);
                if (isBelowPlayer)
                    player.transform.position += new Vector3(range, 0, 0);
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
    public void Active()
    {
        isActive = true;
        isMoving = true;
        animator.SetBool("IsMoving", true);
        isBelowPlayer = true;
    }
    public void BelowPlayer()
    {
        isBelowPlayer = true;
    }  
    public void UnBelowPlayer()
    {
        isBelowPlayer = false;
    }
    public bool GetActive()
    {
        return isActive;
    }
}
