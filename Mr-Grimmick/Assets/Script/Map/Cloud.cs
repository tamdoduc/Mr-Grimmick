using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float minHorizontal, maxHorizontal, minVertical, maxVertical;
    [SerializeField] int minSpeed, maxSpeed;
    SpriteRenderer sprite;
    int speed;
    int size = 0;
    Animator animator;
    void Start()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
        sprite.sortingOrder = Random.Range(0, 2) * 4;
        size = Random.Range(0, 2);
        animator.SetBool("isSmallSize", (size==0) );
        
        speed = Random.Range(minSpeed, maxSpeed);   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.gameObject.transform.position;
        if (minHorizontal < pos.x)
        {
            pos.x -= Time.deltaTime * speed;
            this.transform.position = pos;
        }
        else
        {
            Reset();
        }
    }
    private void Reset()
    {
        float posY = Random.Range(minVertical, maxVertical);
        this.gameObject.transform.position = new Vector3(maxHorizontal, posY,0);
        sprite.sortingOrder = Random.Range(0, 2) * 4;
        size = Random.Range(0, 2);
        animator.SetBool("isSmallSize", (size == 0));
    }
}
