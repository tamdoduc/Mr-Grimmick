using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehod_Head : MonoBehaviour
{
    [SerializeField] bool directory_IsLeft;
    [SerializeField] float force;
    [SerializeField] Hedgehog hedgehog;

    List<GameObject> currentCollisions;
    void Start()
    {
        currentCollisions = new List<GameObject>();
    }
    void Update()
    {
        SetDirection();
        int direction;
        if (directory_IsLeft)
            direction = -1;
        else direction = 1;

        for (int i = 0; i < currentCollisions.Count; i++)
        {
            currentCollisions[i].transform.position += new Vector3(Time.deltaTime * direction * force, 0, 0);
        }
    }
    private void SetDirection()
    {
        directory_IsLeft = hedgehog.faceRight; ;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            currentCollisions.Add(collision.gameObject);
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            currentCollisions.Remove(collision.gameObject);
    }
}
