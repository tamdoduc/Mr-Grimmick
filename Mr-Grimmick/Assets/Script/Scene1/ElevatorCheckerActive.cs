using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCheckerActive : MonoBehaviour
{
    [SerializeField] Elevator elevator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            elevator.BelowPlayer();
            if (!elevator.GetActive())
                elevator.Active();
        }
    }  
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            elevator.UnBelowPlayer();
        }
    }
}
