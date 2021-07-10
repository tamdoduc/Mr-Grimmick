using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_7_Skill1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D body;
    [SerializeField] EffectFlash effectFlash;
    EffectFlash cloneEffectFlash;
    private float endPosX;
    private int direction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cloneEffectFlash == null)
        {
            cloneEffectFlash = GameObject.Instantiate(effectFlash);
            cloneEffectFlash.SetTimeMax(0.5f);
            cloneEffectFlash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
            cloneEffectFlash.Active();
        }
        if (transform.position.x * direction > endPosX * direction)
            GameObject.Destroy(this.gameObject);
    }

    public void SetStart(Vector3 posStart, float velX, float posX)
    {
        transform.position = posStart;
        body.velocity = new Vector2(velX, 0);
        endPosX = posX;
        if (velX > 0)
            direction = 1;
        else
            direction = -1;
    }
}
