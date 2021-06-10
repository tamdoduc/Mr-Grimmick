using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    Vector3 velocity;

    [SerializeField] Rigidbody2D body;

    Player player;

    Vector3 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = this.gameObject.transform.position;
        velocity = new Vector3(-5, 0, 0);
        player = GameObject.Find("Player").GetComponent<Player>();

    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y*Time.deltaTime, 0);
        if (this.gameObject.transform.position.y<=defaultPos.y-5)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString() == "8" && collision.gameObject.name!="Gun") // Ground
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.gravityScale = 1;
            body.AddForce(new Vector2(0.05f, 0.15f));
            velocity = Vector3.zero;
        }
    }
}
