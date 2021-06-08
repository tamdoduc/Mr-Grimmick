using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject animationDestruct;
    [SerializeField] Vector3 pos;

    GameObject clone;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() != "8" || collision.gameObject.name == "BulletFly(Clone)")
        {
            clone = GameObject.Instantiate(animationDestruct);
            pos.x = collision.gameObject.transform.position.x;
            clone.transform.position = pos;
            GameObject.Destroy(collision.gameObject);
        }
    }
}
