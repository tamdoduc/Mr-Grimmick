using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEdgeCheck : MonoBehaviour
{
    [SerializeField] Worm worm;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        worm.isOnEdge = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay");
        worm.isOnEdge = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        worm.isOnEdge = true;
    }
}
