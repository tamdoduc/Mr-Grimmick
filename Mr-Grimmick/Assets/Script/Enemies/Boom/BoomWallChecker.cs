using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomWallChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Boom boom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boom.colliderWall = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        boom.colliderWall = false;
    }
}
