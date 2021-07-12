using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_7_Skill2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SelfDestruct selfDestruct;
    [SerializeField] Rigidbody2D body;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetStart(Vector3 posStart, float velX)
    {
        transform.position = posStart;
        body.velocity = new Vector2(velX, -4);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer.ToString() == "8") //Ground
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            GameObject.Destroy(this.gameObject);
        }
    }
}
