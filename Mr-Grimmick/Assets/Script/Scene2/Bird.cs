using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Vector3 DefaultPos;

    [SerializeField] bool isFlying = false;

    int direction;

    int HP = 4;
    [SerializeField] EffectFlash effectPlash;
    EffectFlash cloneEffectFLash;
    [SerializeField] SelfDestruct selfDestruct;

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body;
    [SerializeField] CheckTop checkTop;
    
    // Start is called before the first frame update
    void Start()
    {
        DefaultPos = this.gameObject.transform.position;
        checkTop.SetActive(false);
        if (this.gameObject.transform.rotation.y == 0)
            direction = 1;
        else
            direction = -1;
    }

    // Update is called once per frame
    void Update()
    { 
        if (animator.GetInteger("state") == 1 && this.gameObject.transform.position.y <= DefaultPos.y - 3f)
            Fly();

        if (isFlying)
        {
            this.gameObject.transform.position += new Vector3(direction * Time.deltaTime * 2.5f, 0);
            if (this.gameObject.transform.position.y < DefaultPos.y-1)
                this.gameObject.transform.position += new Vector3(0, Time.deltaTime * 2);
        }
    }
    public void Fly()
    {
        animator.SetInteger("state", 2);
        body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        isFlying = true;
        checkTop.SetActive(true);
        Debug.Log("Fly");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (animator.GetInteger("state")==0 && collision.gameObject.name == "Player")
            animator.SetInteger("state",1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cloneEffectFLash == null && collision.gameObject.layer.ToString() == "11") //skill
        {
            HP--;
            if (HP == 0)
            {
                selfDestruct = GameObject.Instantiate(selfDestruct);
                selfDestruct.transform.position = this.gameObject.transform.position;
                GameObject.Destroy(this.gameObject);
            }
            if (HP > 0)
            {
                cloneEffectFLash = GameObject.Instantiate(effectPlash);
                cloneEffectFLash.SetTimeMax(2);
                cloneEffectFLash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                cloneEffectFLash.Active();
            }
        }
    }
}
