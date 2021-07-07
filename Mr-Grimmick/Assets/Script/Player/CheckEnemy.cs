using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Player player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() == "13" ) //enemy
        {
            if (player.IsFainting() == false)
                player.BeActack();
        }
        if (collision.gameObject.layer.ToString() == "12") //thorTrap
        {
            Debug.LogWarning("Die");
            player.Die();
        }
    }
}
