using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : SkillTemp
{
    [SerializeField] StarFolow starFolow;
   
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (!isShooted)
        {
            this.transform.position = player.gameObject.transform.position + new Vector3(0, 1, 0);
        }
        else
        {
            if (!starFolow.GetActive())
            {
                starFolow.Active();
            }
            timeShooted += Time.deltaTime;
            if (timeShooted >= 2f)
                GameObject.Destroy(this.gameObject);
            starFolow.transform.position = this.gameObject.transform.position;
            body.velocity = velocity;
        }
    }
    public override void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
