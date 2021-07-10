using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFlash : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer sprite;
    float timeMax = 10;
    float time = 0;
    float timeLoop = 0;
    bool isActive = false;
    void Start()
    {

    }
    void Update()
    {
        if (isActive && sprite!=null)
        {
            time += Time.deltaTime;
            timeLoop += Time.deltaTime;
            if (time >= timeMax)
            {
                sprite.enabled = true;
                GameObject.Destroy(this.gameObject);
            }
            if (timeLoop >= 0.1f)
            {
                timeLoop = 0;
                sprite.enabled = !sprite.enabled;
            }
        }
    }
    public void SetTimeMax(float time)
    {
        this.time = 0;
        timeMax = time;
    }
    public void SetSpriteRender(SpriteRenderer sprite)
    {
        this.sprite = sprite;
    }
    public void Active()
    {
        isActive = true;
    }
}
