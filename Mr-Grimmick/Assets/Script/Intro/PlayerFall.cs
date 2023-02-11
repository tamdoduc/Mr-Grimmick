using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    // Start is called before the first frame update
    float time = 0;
    int index = 0;
    [SerializeField] float[] timeChangePos;
    [SerializeField] Vector2[] tmpos ;
    [SerializeField] float timeWait;
    [SerializeField] float timeStart;


    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time>timeStart)
        if (index < tmpos.Length)
        { 
            //if (time >= timeChangePos[index])
            //{
            //    time = timeChangePos[index] - time;
            //    index++;
            //}
           
            if (index == tmpos.Length) return;
            float range = 2f * Time.deltaTime;
            Vector2 pos = this.gameObject.transform.position;
            if (pos == tmpos[index])
                index++;
            if (pos.x < tmpos[index].x)
            {
                pos.x = Mathf.Min(pos.x + range, tmpos[index].x);
            }
            if (pos.x > tmpos[index].x)
            {
                pos.x = Mathf.Max(pos.x - range, tmpos[index].x);
            }
            if (pos.y < tmpos[index].y)
            {
                pos.y = Mathf.Min(pos.y + range, tmpos[index].y);
            }
            if (pos.y > tmpos[index].y)
            {
                pos.y = Mathf.Max(pos.y - range, tmpos[index].y);
            }
            this.gameObject.transform.position = pos;
        } else
        {
            if (time>timeWait)
            GameObject.Destroy(this.gameObject);
        }
    }
}
