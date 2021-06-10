using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGUp : MonoBehaviour
{
    // Start is called before the first frame update
    float timeActive = 0;
    bool isActive = false;
    [SerializeField] BulletTrap BulletTrap;
    BulletTrap cloneBulletTrap;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (cloneBulletTrap == null)
            {
                cloneBulletTrap = GameObject.Instantiate(BulletTrap);
                cloneBulletTrap.transform.position = this.gameObject.transform.position + new Vector3(0.5f, 0, 0);
                cloneBulletTrap.AddForce(new Vector2(0, 0.04f));
            }
            timeActive += Time.deltaTime;
            if (timeActive >= 0.2f)
                GameObject.Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            isActive = true;
    }
}
