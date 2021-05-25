using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    float timeLoop = 0;

    [SerializeField] Animator animator;

    [SerializeField] SelfDestruct selfDestruct;

    [SerializeField] Bullet bullet;
    Bullet bullets;

    // Start is called before the first frame update
    void Start()
    {
        timeLoop = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        timeLoop += Time.deltaTime;
        if (timeLoop>1.2)
        {
            timeLoop = 0;
            bullets = GameObject.Instantiate(bullet);
            bullets.transform.position = this.gameObject.transform.position + new Vector3(1, 0, 0);
            bullets.SetSpeed(new Vector2(2, 0));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer.ToString());
        if (collision.gameObject.layer.ToString() == "12") //Thorn Trap
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            GameObject.Destroy(this.gameObject);
        }
    }
}
