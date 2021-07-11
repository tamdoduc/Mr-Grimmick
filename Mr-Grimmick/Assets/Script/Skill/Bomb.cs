using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : SkillTemp
{
    float timeSelfDestruct;

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
                if (timeShooted >= 2f)
                    SelfDestruct();
            }
        } else
        {
            timeSelfDestruct += Time.deltaTime;
            if (timeSelfDestruct >= 0.66f)
                GameObject.Destroy(this.gameObject);
        }
    }
    override public void SelfDestruct()
    {
        isExist = false;
        animator.SetBool("IsActive", true);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString() == "12") //Thorn Trap
        {
            SelfDestruct();
        }
    }
}
