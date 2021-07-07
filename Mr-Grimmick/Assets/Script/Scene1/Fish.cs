using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator animator;
    [SerializeField] bool direction_isLeft;
    [SerializeField] float limLeft, limRight;
    [SerializeField] float minSpeed, maxSpeed;

    int direction;

    float timeRoll = 0;
    void Start()
    { 
        animator.SetBool("Roll", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Roll") == true)
        {
            if (timeRoll <= 0.5f)
            {
                timeRoll+=Time.deltaTime;
            }
            else
            {
                timeRoll = 0;
                animator.SetBool("Roll", false);
            }
            return;
        }
        int randomRoll = Random.Range(0,1000);
        if (randomRoll==0)
        {
            direction_isLeft = !direction_isLeft;
            animator.SetBool("Roll", true);
        }



        if (this.gameObject.transform.position.x>=limRight)
        {
            this.gameObject.transform.position = new Vector3(limRight, this.gameObject.transform.position.y, 0);
            direction_isLeft = true;
            animator.SetBool("Roll", true);
           
        }
        if (this.gameObject.transform.position.x <= limLeft)
        {
            this.gameObject.transform.position = new Vector3(limLeft, this.gameObject.transform.position.y, 0);
            direction_isLeft = false;
            animator.SetBool("Roll", true);
        }
        if (direction_isLeft)
            direction = -1;
        else
            direction = 1;


        if (direction == -1)
            this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        else
            this.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        float speed;
        if (animator.GetBool("Roll") == false)
            speed = Random.Range(minSpeed, maxSpeed);
        else
            speed = speed = 0.01f; 
        gameObject.transform.position += new Vector3(direction * Time.deltaTime * speed, 0, 0);
    }
}
