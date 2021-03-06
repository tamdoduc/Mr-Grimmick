using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBar : MonoBehaviour
{
    [SerializeField] bool directory_IsLeft ;
    [SerializeField] float force;

    List<GameObject> currentCollisions;
    void Start()
    {
        currentCollisions = new List<GameObject>();
    }
    void Update()
    {
        int direction;
        if (directory_IsLeft) 
            direction = -1;
        else direction = 1;
        
        for (int i = 0; i < currentCollisions.Count; i++) 
        {
            currentCollisions[i].transform.position += new Vector3(Time.deltaTime * direction * force, 0, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentCollisions.Add(collision.gameObject);
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        currentCollisions.Remove(collision.gameObject);
    }
}
