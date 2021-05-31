using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEvelator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Elevator elevator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            elevator.Reset();
    }
}
