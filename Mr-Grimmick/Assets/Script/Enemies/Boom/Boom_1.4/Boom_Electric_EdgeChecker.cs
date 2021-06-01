using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_Electric_EdgeChecker : MonoBehaviour
{
    [SerializeField] Boom_Electric boom;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //boom.isOnEdge = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //boom.isOnEdge = true;
    }
}
