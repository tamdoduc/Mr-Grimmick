using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFolow : MonoBehaviour
{
    [SerializeField] Animator animator;
    void Start()
    {
        animator.SetBool("IsActive", false);
    }
    public bool GetActive()
    {
        return animator.GetBool("IsActive");
    }
    public void Active()
    {
        animator.SetBool("IsActive", true);
    }
}
