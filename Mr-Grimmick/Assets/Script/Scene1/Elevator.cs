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

    [SerializeField] GameObject player;

    [SerializeField] float timeActived = 0;
    // Start is called before the first frame update
    void Start()
    {

        isActive = false;
        isMoving = false;
        animator.SetBool("IsMoving", false);
        timeActived = 0;
        isActiveTrap = false;
        isBelowPlayer = false;
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
                if (isBelowPlayer)
                {
                    player.transform.position += new Vector3(range, 0, 0);
                    Debug.Log(range);
                }
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
        timeActived = 0;
    }
    public void BelowPlayer()
    {
        isBelowPlayer = true;
    }  
    public void UnBelowPlayer()
    {
        isBelowPlayer = false;
        Debug.Log(false);
    }
    public bool GetActive()
    {
        return isActive;
    }
    public void Reset()
    {
        isActive = false;
        isMoving = false;
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsActiveTrap", false);
        timeActived = 0;
        isActiveTrap = false;
        this.gameObject.transform.position = new Vector3(-3.11f, 0.68f, 0);
        boxCollider2D.enabled = true;
    }
}
