using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3[] pos;

    [SerializeField] Player player;

    [SerializeField] bool isActived;
    [SerializeField] bool isUp;

    [SerializeField] int indexPos;
    void Start()
    {
        isActived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActived)
        {
            Vector3 temp = player.transform.position;
            if (isUp)
            {
                if (temp.x==pos[indexPos].x  && temp.y == pos[indexPos].y ) 
                indexPos++;
                if (indexPos == pos.Length)
                {
                    player.SetActive(true);
                    isActived = false;
                }
            }
            else
            {
                if (temp.x == pos[indexPos].x && temp.y == pos[indexPos].y) 
                    indexPos--;
                if (indexPos == -1)
                {
                    player.SetActive(true);
                    isActived = false;
                }
            }
        }
        if (isActived)
        {
            Vector3 temp = player.transform.position;
            if (temp.x < pos[indexPos].x)
                temp.x = Mathf.Min(pos[indexPos].x, temp.x + Time.deltaTime * 5);

            if (temp.x > pos[indexPos].x)
                temp.x = Mathf.Max(pos[indexPos].x, temp.x - Time.deltaTime * 5);


            if (temp.y < pos[indexPos].y)
                temp.y = Mathf.Min(pos[indexPos].y, temp.y + Time.deltaTime * 5);
            else
            if (temp.y > pos[indexPos].y)
                temp.y = Mathf.Max(pos[indexPos].y, temp.y - Time.deltaTime * 5);
            player.transform.position = temp;
        }
    }
    public bool GetActive()
    {
        return isActived;
    }
    public void Active(bool isUp)
    {
        isActived = true;
        player.SetActive(false);
        this.isUp = isUp;
        if (isUp)
        {
            indexPos = 1;
        }
        else
        {
            indexPos = pos.Length - 2;
        }
    }
}
