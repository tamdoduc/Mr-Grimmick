using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomDownGroundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Boom boom;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        boom.downGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (boom.downGround)
        {
            boom.downGround = false;
            Debug.Log("High");
        }
    }
}
