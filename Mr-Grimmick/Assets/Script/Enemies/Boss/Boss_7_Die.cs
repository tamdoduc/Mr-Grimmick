using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_7_Die : MonoBehaviour
{
    // Start is called before the first frame update
    private float timeMax = 0, timeCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > timeMax)
            GameObject.Destroy(this.gameObject);
    }
    
    public void SetPos(Vector3 pos, float time)
    {
        transform.position = pos;
        timeMax = time;
    }
}
