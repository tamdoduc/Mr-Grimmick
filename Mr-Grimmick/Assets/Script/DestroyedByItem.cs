using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedByItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bomb(Clone)" || collision.gameObject.name == "EnergyBall(Clone)")
            Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bomb(Clone)" || collision.gameObject.name == "EnergyBall(Clone)")
            Destroy(this.gameObject);
    }
}
