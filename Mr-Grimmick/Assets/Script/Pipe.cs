using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3[] pos;

    [SerializeField] Player player;

    bool isActived;
    bool isUp;

    int indexPos;
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
            int numberOfRound = 100;
            if (isUp)
            {
                if (Mathf.Round(temp.x * numberOfRound) == Mathf.Round(pos[indexPos].x * numberOfRound) && Mathf.Round(temp.y * numberOfRound) == Mathf.Round(pos[indexPos].y * numberOfRound)) 
                indexPos++;
                if (indexPos == pos.Length)
                {
                    player.SetActive(true);
                    isActived = false;
                }
            }
            else
            {
                if (Mathf.Round(temp.x * numberOfRound) == Mathf.Round(pos[indexPos].x * numberOfRound) && Mathf.Round(temp.y * numberOfRound) == Mathf.Round(pos[indexPos].y * numberOfRound)) 
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
            int numberOfRound = 100;
            if (Mathf.Round(temp.x *numberOfRound) < Mathf.Round(pos[indexPos].x * numberOfRound))
                temp.x += 0.01f;
            else
            if (Mathf.Round(temp.x * numberOfRound) > Mathf.Round(pos[indexPos].x *numberOfRound))
                temp.x -= 0.01f;
            if (Mathf.Round(temp.y * numberOfRound) < Mathf.Round(pos[indexPos].y * numberOfRound))
                temp.y += 0.01f;
            else
            if (Mathf.Round(temp.y * numberOfRound) > Mathf.Round(pos[indexPos].y * numberOfRound))
                temp.y -= 0.01f;
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
            indexPos = pos.Length - 1;
        }
    }
}
