using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHUB : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] SelfDestruct selfDestruct;
    [SerializeField] AudioSource audio;
    bool active;
    private void Start()
    {
        active = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player"&&active)
        {
            active = false;
            int countItem = PlayerPrefs.GetInt("countItem") -1;
            if (countItem < 2)
            {
                audio = AudioSource.Instantiate(audio);
                Destroy(audio.gameObject, 1);
                PlayerPrefs.SetInt("countItem", countItem + 2);
                PlayerPrefs.SetInt("item" + (countItem + 1).ToString(), id);
                Debug.Log(("item" + (countItem + 1).ToString()));
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.layer.ToString() == "12")
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            Destroy(this.gameObject);
        }

    }
}
