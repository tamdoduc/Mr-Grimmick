using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBrick : MonoBehaviour
{
    [SerializeField] Vector3[] listPos;
    [SerializeField] int id;
    [SerializeField] Player player;
    bool isBelowPlayer;
    
    void Start()
    {
        id = 1;
        isBelowPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = this.transform.position;
        Vector2 minRange;
        minRange.x = Mathf.Min(Mathf.Abs(temp.x - listPos[0].x), Mathf.Abs(temp.x - listPos[1].x));
        minRange.y = Mathf.Min(Mathf.Abs(temp.y - listPos[0].y), Mathf.Abs(temp.y - listPos[1].y));
        switch (id)
        {
            case 0:
                if (temp.x <= listPos[id].x  && temp.y  <= listPos[id].y)
                    id = 1;
                else
                {
                    if (temp.x > listPos[id].x)
                        temp.x -= Time.deltaTime * (minRange.x + 0.5f) * 3;
                    if (temp.x  <listPos[id].x)
                        temp.x = listPos[id].x;
                    if (temp.y > listPos[id].y)
                        temp.y -= Time.deltaTime * (minRange.y + 0.5f) * 3;
                    if (temp.y < listPos[id].y)
                        temp.y = listPos[id].y;
                }
                break;
            case 1:
                if (temp.x >= listPos[id].x && temp.y >= listPos[id].y) 
                    id = 0;
                else
                {
                    if (temp.x < listPos[id].x)
                        temp.x += Time.deltaTime * (minRange.x + 0.5f) * 3;
                    if (temp.x > listPos[id].x)
                        temp.x = listPos[id].x;
                    if (temp.y < listPos[id].y)
                        temp.y += Time.deltaTime * (minRange.y + 0.5f) * 3;
                    if (temp.y > listPos[id].y)
                        temp.y = listPos[id].y;
                }
                break;
        }
        Debug.Log(minRange);

        if (this.isBelowPlayer)
        {
            player.transform.position += temp - this.transform.position;
        }
        this.transform.position = temp;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            isBelowPlayer = true;
    } 
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            isBelowPlayer = false;
    }
}
