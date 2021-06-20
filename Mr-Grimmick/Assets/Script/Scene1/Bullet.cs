using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    float timeExist;

    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D body;
    Vector3 velocity;

    [SerializeField] SelfDestruct selfDestruct;

    [SerializeField] LayerMask layerGround;
    [SerializeField] Collider2D colliderCheckGround;

    void Start()
    {
        timeExist = 0;
    }
    // Update is called once per frame
    void Update()
    {
        timeExist += Time.deltaTime;
        if (IsGrounded())
            velocity.y = 0.5f;
        else
            velocity.y = -8f;
        this.gameObject.transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime);
        if (timeExist >= 2f)
            SelfDestruct();
    }
    public void SetSpeed(Vector2 velocity)
    {
        this.velocity = velocity;
    }
    public void SetTimeExist(float time)
    {
        timeExist = time;
    }
    void SelfDestruct()
    {
        selfDestruct = GameObject.Instantiate(selfDestruct);
        selfDestruct.transform.position = this.gameObject.transform.position;
        GameObject.Destroy(this.gameObject);
    }
    [SerializeField] bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.15f, layerGround);
        return hit2D.collider != null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString() == "12" || collision.gameObject.layer.ToString() == "14" ||collision.gameObject.name == "Bullet(Clone)")  //Thorn Trap
        {
            SelfDestruct();
        }
        velocity.x = -0.5f;
        velocity.x = 0.5f;
    }
}
