using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockStage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int stage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerPrefs.SetInt("currentLevel", stage);
            Application.LoadLevel("Map");
        }
    }
}
