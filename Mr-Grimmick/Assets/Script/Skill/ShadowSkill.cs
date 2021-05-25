using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSkill : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Animator animator;
    void Start()
    {
        animator.SetBool("IsNull", true);
    }
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }
    public void Run()
    {
        animator.SetBool("IsNull", false);
    }  
    public void Destroy()
    {
        animator.SetBool("IsNull", true);
    }
    public bool IsNull()
    {
        return animator.GetBool("IsNull");
    }
}
