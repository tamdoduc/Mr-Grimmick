using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;

    [SerializeField] float[] arrLimVertical;

    [SerializeField] string[] limHorizontal;

    float[,] arrLimHorizontal;
    int[] count;

    [SerializeField] GameObject player;
    [SerializeField] HUBManage hub;

    Vector3 posCam;

    [SerializeField] int idV = -1, idH;
    void Start()
    {
        count = new int[limHorizontal.Length];
        cam = Camera.main;
        posCam = this.gameObject.transform.position;
        arrLimHorizontal = new float[limHorizontal.Length, 10];
        SetLimHorizontal();
        Vector3 pos = player.transform.position;
        SetPosVertical(pos);
        SetIDH(pos);
        posCam.x = Mathf.Max(pos.x, arrLimHorizontal[idV,idH]+8);
        posCam.x = Mathf.Min(posCam.x, arrLimHorizontal[idV, idH + 1] - 8);
        cam.transform.position = posCam;
    }
    void Update()
    {
        posCam = this.gameObject.transform.position;
        SetPosVertical(player.transform.position);

        MoveBasePosision(player.transform.position);
    }
    void SetLimHorizontal()
    {
        Debug.Log(limHorizontal.Length);
        for (int i = 0; i < limHorizontal.Length; i++)
        {
            string tmp = "";
            count[i] = 0;
            for (int j = 0; j < limHorizontal[i].Length; j++)
            {
                if (limHorizontal[i][j] != '|')
                {
                    tmp += limHorizontal[i][j];
                }
                else
                {
                    arrLimHorizontal[i, count[i]] = float.Parse(tmp);
                    tmp = "";
                    Debug.Log(arrLimHorizontal[i, count[i]]);
                    count[i]++;
                }
            }
        }
    }
    void SetPosVertical(Vector3 pos)
    {
        int id = 0;
        posCam.y = (arrLimVertical[0] + arrLimVertical[1]) / 2;

        for (int i = 0; i < arrLimVertical.Length - 1; i++)
        {
            if (arrLimVertical[i] <= pos.y && pos.y <= arrLimVertical[i + 1])
            {
                posCam.y = (arrLimVertical[i] + arrLimVertical[i + 1]) / 2;
                id = i;
                break;
            }
        }
        if (pos.y > arrLimVertical[arrLimVertical.Length - 1])
        {
            posCam.y = (arrLimVertical[arrLimVertical.Length - 2] + arrLimVertical[arrLimVertical.Length - 1]) / 2;
            id = arrLimVertical.Length - 2;
        }
        if (id!=this.idV)
            this.idV = id;
    }
    void SetIDH(Vector3 pos)
    {
        if (pos.x < arrLimHorizontal[idV, 0] || pos.x > arrLimHorizontal[idV, count[idV] - 1])
        {
            Debug.Log("False");
            return;
        }
        for (int i = 0; i < count[idV]-1; i++) 
        {
            if (pos.x > arrLimHorizontal[idV, i])
                idH = i;
        }
    }
    // Update is called once per frame
    void MoveBasePosision(Vector2 pos)
    {
        SetPosVertical(pos);
        SetIDH(pos);
        SetPosHorizontal(pos);
        cam.transform.position = posCam;
        hub.transform.position = posCam - new Vector3(0, 6, -10);
    }
    void SetPosHorizontal(Vector2 pos)
    {
        float speed = 20;
        posCam.x = cam.transform.position.x;
        if (posCam.x > arrLimHorizontal[idV, idH + 1] - 8)
            posCam.x = Mathf.Max(posCam.x - Time.deltaTime * speed, arrLimHorizontal[idV, idH + 1] - 8);
        else
        if (posCam.x < arrLimHorizontal[idV, idH] + 8)
            posCam.x = Mathf.Min(posCam.x + Time.deltaTime * speed, arrLimHorizontal[idV, idH] + 8);
        else
        {
            posCam.x = Mathf.Max(pos.x, arrLimHorizontal[idV, idH] + 8);
            posCam.x = Mathf.Min(posCam.x, arrLimHorizontal[idV, idH + 1] - 8);
        }
    }
}
