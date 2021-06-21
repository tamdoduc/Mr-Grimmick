using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] gameObjects;
    bool[] b = new bool[2];
    Animator animator;
    int index=0;
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Application.LoadLevel("Menu");

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Intro2"))
        {
            if (!b[0])
            {
                gameObjects[0] = GameObject.Instantiate<GameObject>(gameObjects[0]);
                gameObjects[0].transform.position = new Vector3(-0.5f, 3.2f, 0);
                b[0] = true;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Intro3"))
        {
            GameObject.Destroy(gameObjects[0]);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Intro4"))
        {
            if (!b[1])
            {
                gameObjects[1] = GameObject.Instantiate<GameObject>(gameObjects[1]);
                gameObjects[1].transform.position = new Vector3(-1.8f, 2.2f, 0);    
                gameObjects[2] = GameObject.Instantiate<GameObject>(gameObjects[2]);
                gameObjects[2].transform.position = new Vector3(-1.1f, 0.2f, 0);
                b[1] = true;
            }
        }

    }
}
