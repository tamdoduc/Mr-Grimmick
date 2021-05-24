using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEdgeChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Boom boom;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        boom.isOnEdge = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (boom.grounded)
        {
            Debug.Log("Edge");
            boom.isOnEdge = true;
        }         
    }
}
