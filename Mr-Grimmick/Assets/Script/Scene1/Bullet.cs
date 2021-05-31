using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Player player;

    bool isBelowPlayer;

    Vector3 PosBefore;

    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D body;

    [SerializeField] SelfDestruct selfDestruct;

    void Start()
    {
        PosBefore = this.gameObject.transform.position;
    }
    public void SetBelowPlayer(bool b)
    {
        isBelowPlayer = b;
    }
    // Update is called once per frame
    void Update()
    {
        if (isBelowPlayer)
        {
            player.transform.position += this.gameObject.transform.position - PosBefore;
        }
        PosBefore = this.gameObject.transform.position;
    }
    public void SetSpeed(Vector2 velocity)
    {
        body.velocity = velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString()=="12") //Thorn Trap
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            GameObject.Destroy(this.gameObject);
        }
    }
}
