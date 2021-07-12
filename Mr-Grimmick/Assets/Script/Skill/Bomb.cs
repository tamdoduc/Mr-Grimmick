using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : SkillTemp
{
    float timeSelfDestruct;
    [SerializeField] LayerMask layerEnemy;
    [SerializeField] Collider2D boxBody;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        isExist = true;
        timeSelfDestruct = 0;
    }

    void Update()
    {
        if (isExist)
        { 
            if (!isShooted)
            {
                this.transform.position = player.gameObject.transform.position + new Vector3(0, 1, 0);
            }
            else
            {
                velocity.y -= Time.deltaTime * 30;
                body.velocity = velocity;
                timeShooted += Time.deltaTime;
                if (timeShooted >= 2f ||Eneemy())
                    SelfDestruct();
            }
        } else
        {
            timeSelfDestruct += Time.deltaTime;
            if (timeSelfDestruct >= 0.5f)
                GameObject.Destroy(this.gameObject);
        }
    }
    bool Eneemy()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(boxBody.bounds.center, boxBody.bounds.size, 0, Vector2.down, 0.05f, layerEnemy);
        return hit2D.collider != null ;
    }
    override public void SelfDestruct()
    {
        isExist = false;
        animator.SetBool("IsActive", true);
        Debug.Log("animation");
        this.gameObject.GetComponent<Rigidbody2D>().Sleep();
      //  Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString() == "12") //Thorn Trap
        {
            SelfDestruct();
        }
    }
}
