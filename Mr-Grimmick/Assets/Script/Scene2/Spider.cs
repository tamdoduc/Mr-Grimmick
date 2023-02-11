using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    [SerializeField] float limLeft, limRight, limTop, limBotom;
    [SerializeField] float speed;

    [SerializeField] float timeLoop = 0,maxTime;
    int state;
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLoop < maxTime)
        {
            timeLoop += Time.deltaTime;
        }
        else
        {
            timeLoop = 0;
            int random = Random.Range(0, 5);
            state = random;
        }
        Vector3 newPos = this.gameObject.transform.position;
        switch (state)
        { 
            case 0:
                break;
            case 1:
                newPos.x = Mathf.Max(limLeft, newPos.x - Time.deltaTime * speed);
                break;
            case 2:
                newPos.x = Mathf.Min(limRight, newPos.x + Time.deltaTime * speed);
                break;
            case 3:
                newPos.y = Mathf.Max(limBotom, newPos.y - Time.deltaTime * speed);
                break;
            case 4:
                newPos.y = Mathf.Min(limTop, newPos.y + Time.deltaTime * speed);
                break;
        }
        if (this.gameObject.transform.position == newPos)
            animator.SetBool("isMoving", false);
        else
            animator.SetBool("isMoving", true);
        this.gameObject.transform.position = newPos;
    }
}
