using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHP : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            int maxHp = PlayerPrefs.GetInt("maxHp");
            if (maxHp < 4)
            {
                PlayerPrefs.SetInt("maxHp", maxHp + 1);
                PlayerPrefs.SetInt("currentHp", PlayerPrefs.GetInt("currentHp") + 1);
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}
