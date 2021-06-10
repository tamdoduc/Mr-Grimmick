using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trap : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject trap;
    bool isActived ;

    [SerializeField] SelfDestruct selfDestruct;
    // Start is called before the first frame update
    void Start()
    {
        isActived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActived) 
        {
            trap.transform.position += new Vector3(0,- Time.deltaTime *10, 0);
        }
    }
    public void Active()
    {
        isActived = true;
        animator.SetBool("IsActived", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() =="8")
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            GameObject.Destroy(trap.gameObject);
        }
    }
}
