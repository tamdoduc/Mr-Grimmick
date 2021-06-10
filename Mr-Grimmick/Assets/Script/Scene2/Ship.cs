using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // Start is called before the first frame update
    bool isBelowPlayer;
    bool isMoving;

    [SerializeField] Player player;
    [SerializeField] float limX;
    void Start()
    {
        isBelowPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (this.gameObject.transform.position.x == limX)
                isMoving = false;
            else
            {
                float range = Time.deltaTime * 2;
                this.gameObject.transform.position = new Vector3(Mathf.Min(limX, range + this.gameObject.transform.position.x), this.gameObject.transform.position.y);
                if (isBelowPlayer)
                    player.transform.position += new Vector3(range, 0);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isBelowPlayer = true;
            if (isMoving == false)
                isMoving = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            isBelowPlayer = false;
    }
}
