using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 posBefore;

    [SerializeField] Animator animator;
    int id;
    void Start()
    {
        id = 0;
        posBefore = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 range = this.gameObject.transform.position - posBefore;
        if (Mathf.Abs(range.x)>0.1f)
        {
            ChangeAnimator();
            posBefore = this.gameObject.transform.position;
        }
        if (range.y > 0)
            posBefore = this.gameObject.transform.position;
    }
    void ChangeAnimator()
    {
        id++;
        id = id % 4;
        animator.SetInteger("id", id);
    }
}
