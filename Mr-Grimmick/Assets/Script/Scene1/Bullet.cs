using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Player player;

    bool isBelowPlayer;

    Vector3 PosBefore;

    float timeExist;

    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D body;

    [SerializeField] SelfDestruct selfDestruct;

    void Start()
    {
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
        if (timeExist >= 3f)
            SelfDestruct();

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
    void SelfDestruct()
    {
        selfDestruct = GameObject.Instantiate(selfDestruct);
        selfDestruct.transform.position = this.gameObject.transform.position;
        GameObject.Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString()=="12") //Thorn Trap
        {
            SelfDestruct();
        }
    }
}
