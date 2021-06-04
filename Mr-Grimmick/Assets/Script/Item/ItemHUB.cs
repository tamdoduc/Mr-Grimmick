using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHUB : MonoBehaviour
{
    [SerializeField] int id;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            int countItem = PlayerPrefs.GetInt("countItem") -1;
            if (countItem < 2)
            {
                PlayerPrefs.SetInt("countItem", countItem + 2);
                PlayerPrefs.SetInt("item" + (countItem + 1).ToString(), id);
                Debug.Log(("item" + (countItem + 1).ToString()));
                Destroy(this.gameObject);
            }
        }
    }
}
