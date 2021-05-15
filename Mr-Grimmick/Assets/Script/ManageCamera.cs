using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    float widthCam,heightCam;

    [SerializeField] float limBottom;
    [SerializeField] float limTop;
    [SerializeField] float limLeft;
    [SerializeField] float limRight;

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
        if (pos.x <= limLeft) // limited Left X
        {
            posCam.x = limLeft;
        }
        else
        if (pos.x >= 53.5f) // limited right X
        {
            posCam.x = limRight;
        }
        else
            posCam.x = pos.x;
        if (pos.y <= (limBottom+limTop)/2) // less than midle
        {
            posCam.y = limBottom;
        } 
        else 
        {
            posCam.y = limTop;
        }
        cam.transform.position = posCam;

    }
}
