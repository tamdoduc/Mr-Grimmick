using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    float widthCam,heightCam;
    const float widthWorld = 16* 29;
    const float heightWorld = 16 *75;

    const float posCamDefaultX = 4.5f;
    const float posCamDefaultY = -14f;
    const float posCamDefaultZ = -10f;
    void Start()
    {
        cam = Camera.main;
        //widthCam = 2f * cam.orthographicSize;
        //heightCam = widthCam * cam.aspect;  


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveBasePosision(Vector2 pos)
    {
        Vector2 posCam = new Vector2();
        if (pos.x <= 4.5f) // limited Left X
        {
            posCam.x = posCamDefaultX;
        }
        if (pos.y <= -14f)
        {
            posCam.x = posCamDefaultY;
        }
        if (pos.x>= 53.5f) // limited right X
        {

        }
    }
}
