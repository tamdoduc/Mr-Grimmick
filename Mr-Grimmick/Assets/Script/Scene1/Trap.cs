using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trap : MonoBehaviour
{
    [SerializeField] Animator animator;

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
            this.transform.position += new Vector3(0,- Time.deltaTime *10, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name =="Player")
        {
            isActived = true;
            animator.SetBool("IsActived", true);
        }    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name != "Player")
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            GameObject.Destroy(this.gameObject);
        }
    }
}
