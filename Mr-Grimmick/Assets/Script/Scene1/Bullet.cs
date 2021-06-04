using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
   Player player;

    bool isBelowPlayer;

    Vector3 PosBefore;

    float timeExist;

    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D body;

    [SerializeField] SelfDestruct selfDestruct;

    [SerializeField] LayerMask layerGround;
    [SerializeField] Collider2D colliderCheckGround;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        PosBefore = this.gameObject.transform.position;
        timeExist = 0;
    }
    public void SetBelowPlayer(bool b)
    {
        isBelowPlayer = b;
    }
    // Update is called once per frame
    void Update()
    {
        timeExist += Time.deltaTime;
        if (IsGrounded())
            body.velocity = new Vector2(body.velocity.x, 1f);
        else
            body.velocity = new Vector2(body.velocity.x, -8f);
        Debug.Log(IsGrounded());
        if (timeExist >= 3f)
            SelfDestruct();

        if (isBelowPlayer)
        {
            player.transform.position += this.gameObject.transform.position - PosBefore;
            Debug.Log("Now " +this.gameObject.transform.position);
            Debug.Log("Before "+ PosBefore);
        }
        PosBefore = this.gameObject.transform.position;
    }
    public void SetSpeed(Vector2 velocity)
    {
        body.velocity = velocity;
    }
    void SelfDestruct()
    {
        selfDestruct = GameObject.Instantiate(selfDestruct);
        selfDestruct.transform.position = this.gameObject.transform.position;
        GameObject.Destroy(this.gameObject);
    }
    [SerializeField] bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.05f, layerGround);
        return hit2D.collider != null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString()=="12") //Thorn Trap
        {
            SelfDestruct();
        }
    }
}
