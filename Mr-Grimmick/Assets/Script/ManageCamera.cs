using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    float widthCam,heightCam;

    [SerializeField] float[] limLeft;
    [SerializeField] float[] limRight;
    [SerializeField] float[] posY;


    [SerializeField] Player player;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBasePosision(player.transform.position);
    }
    void MoveBasePosision(Vector2 pos)
    {
        Vector3 posCam = new Vector3(0,0,-10);
        int index = 0;
        

        posCam.y = (posY[0] + posY[1]) / 2;

        for (int i=0;i<posY.Length-1;i++)
        {
            if (posY[i]<=pos.y && pos.y<=posY[i+1])
            {
                posCam.y = (posY[i] + posY[i + 1]) / 2;
                index = i;
                break;
            }
        }
        if (pos.y > posY[posY.Length - 1])
        {
            posCam.y = (posY[posY.Length - 2] + posY[posY.Length - 1]) / 2;
            index = posY.Length - 1;
        }

        if (pos.x <= limLeft[index])
        {
            posCam.x = limLeft[index];
        }
        else
        if (pos.x >= limRight[index]) 
        {
            posCam.x = limRight[index];
        }
        else
            posCam.x = pos.x;

        cam.transform.position = posCam;

    }
}
