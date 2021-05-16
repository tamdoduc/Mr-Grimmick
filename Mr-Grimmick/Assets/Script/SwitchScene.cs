using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string nextScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Application.LoadLevel(nextScene);
            Debug.Log(nextScene);
        }
    }
}
