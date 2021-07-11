using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<Vector3> listPos ;
    GameObject Object;
    bool active = false;
    int id=0;
    void Start()
    {
        
    }
    public void SetGameObject(GameObject O)
    {
        this.Object = O;
    }
    public void Active()
    {
        active = true;
    }
    public void AddPos(Vector3 pos)
    {
        listPos.Add(pos);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (active && id <listPos.Count && Object!=null)
        {
            Debug.Log(id);
            Vector3 temp = Object.gameObject.transform.position;
            Debug.Log(temp);
            if (temp.y == listPos[id].y && temp.x == listPos[id].x)
            {
                id++;
                return;
               
            }
            float speed = 5;
            if (temp.x < listPos[id].x)
                temp.x = Mathf.Min(listPos[id].x, temp.x + Time.deltaTime * speed);

            if (temp.x > listPos[id].x)
                temp.x = Mathf.Max(listPos[id].x, temp.x - Time.deltaTime * speed);

            if (temp.y < listPos[id].y)
                temp.y = Mathf.Min(listPos[id].y, temp.y + Time.deltaTime * speed);
            else
            if (temp.y > listPos[id].y)
                temp.y = Mathf.Max(listPos[id].y, temp.y - Time.deltaTime * speed);


            Object.gameObject.transform.position = temp;
        } 
    }
}
