using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 posBefore;

    [SerializeField] Animator animator;
    int id;

    [SerializeField] BulletFly bulletFly;
    BulletFly cloneBulletFly;

    [SerializeField] float time, timeLoop;
    void Start()
    {
        time = 0;
        id = 0;
        posBefore = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;
        if (velocity.x > 0)
            velocity.x = Mathf.Max(0, velocity.x - Time.deltaTime * 0.1f);
        if (velocity.x < 0)
            velocity.x = Mathf.Min(0, velocity.x + Time.deltaTime * 0.1f);
        this.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
        time += Time.deltaTime;
        if (time>=timeLoop)
        {
            time = 0;
            cloneBulletFly =  GameObject.Instantiate(bulletFly);
            cloneBulletFly.transform.position = this.gameObject.transform.position - new Vector3(1.26f, -0.5f, 0);
        }
        Vector3 range = this.gameObject.transform.position - posBefore;
        if (Mathf.Abs(range.x)>0.1f)
        {
            ChangeAnimator();
            posBefore = this.gameObject.transform.position;
        }
        if (range.y > 0)
            posBefore = this.gameObject.transform.position;
    }
    void ChangeAnimator()
    {
        id++;
        id = id % 4;
        animator.SetInteger("id", id);
    }
}
