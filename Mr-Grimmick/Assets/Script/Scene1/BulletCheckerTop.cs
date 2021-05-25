using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCheckerTop : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            bullet.SetBelowPlayer(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            bullet.SetBelowPlayer(false);
        }
    }
}
