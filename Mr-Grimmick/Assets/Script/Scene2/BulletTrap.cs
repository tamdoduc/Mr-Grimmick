using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D body;
    [SerializeField] SelfDestruct destruct;
    public void AddForce(Vector2 force)
    {
        body.AddForce(force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() == "11")
        {
            destruct = GameObject.Instantiate(destruct);
            destruct.transform.position = this.gameObject.transform.position;
            Destroy(this.gameObject);
        }
    }
}
