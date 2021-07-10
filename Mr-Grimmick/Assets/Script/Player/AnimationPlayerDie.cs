using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayerDie : MonoBehaviour
{
    // Start is called before the first frame update\
    Vector3 velocity;
    void Start()
    {
        //velocity = new Vector2(0, 0);
    }
    public void SetVelocity(Vector2 v)
    {
        velocity = v;
    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += new Vector3(velocity.x* Time.deltaTime,velocity.y*Time.deltaTime,0);
    }
}
