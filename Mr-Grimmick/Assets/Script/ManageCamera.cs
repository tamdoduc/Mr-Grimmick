using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    public GameObject Player;
    public float limLeft,limTop,limRight,limBottom;
    void Start()
    {
        cam = Camera.main;
        cam.transform.position = new Vector3(limLeft, limBottom, cam.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBasePosision(Player.transform.position);
    }
     void MoveBasePosision(Vector2 pos)
    {
        Vector3 posCam = new Vector3();
        posCam.z = cam.transform.position.z;
        if (pos.x <= limLeft) // limited Left
        {
            posCam.x = limLeft;
        }
        else if (pos.x >= limRight) // limited right
        {
            posCam.x = limRight;
        }
        else
        {
            posCam.x = pos.x;
        }


        if (pos.y <= (limBottom+limTop)/2) // limBottom
        {
            posCam.y = limBottom;
        } 
        else if (pos.y>= limTop) // limited Top
        {
            posCam.y = limTop;
        }
        else
        {
            posCam.y = pos.y;
        }

        cam.transform.position = posCam;
    }
}
