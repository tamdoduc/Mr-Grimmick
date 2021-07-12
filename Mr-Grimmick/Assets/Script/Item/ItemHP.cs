using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHP : MonoBehaviour
{
    bool isUsed = false;

    [SerializeField] AudioSource audio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !isUsed)
        {
            isUsed = true;
            int maxHp = PlayerPrefs.GetInt("maxHp");
            if (maxHp < 4)
            {
                Debug.Log("+1");
                PlayerPrefs.SetInt("maxHp", maxHp + 1);
                PlayerPrefs.SetInt("currentHp", PlayerPrefs.GetInt("currentHp") + 1);
                PlayerPrefs.SetInt("currentHp", maxHp + 1);
            }
            audio = AudioSource.Instantiate(audio);
            Destroy(audio.gameObject, 1);
            GameObject.Destroy(this.gameObject);
        }
    }
}
