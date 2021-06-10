using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBird : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Bird bird;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name =="Player")
        {
            bird.Fly();
            Debug.Log("Trigger");
        }
    }
}
