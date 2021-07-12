using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    float timeLoop = 0;

    [SerializeField] Animator animator;

    [SerializeField] SelfDestruct selfDestruct;

    [SerializeField] Bullet bullet;
    Bullet bullets;

    [SerializeField] GameObject itemBom;
    GameObject cloneitem;

    [SerializeField] AudioSource audio;
    AudioSource clone;

    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        timeLoop = 0;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLoop += Time.deltaTime;
        if (timeLoop>2)
        {
            if (this.gameObject.transform.position.x > camera.transform.position.x - 7 && this.gameObject.transform.position.x < camera.transform.position.x + 7)
                if (this.gameObject.transform.position.x > camera.transform.position.y - 7 && this.gameObject.transform.position.x < camera.transform.position.y + 7)
                {
                    clone = AudioSource.Instantiate(audio);
                    Destroy(clone.gameObject, 1);
                }
            int r = Random.Range(0, 20);
            if (r > 0)
            {
                timeLoop = 0;
                bullets = GameObject.Instantiate(bullet);
                bullets.transform.position = this.gameObject.transform.position + new Vector3(1, 0, 0);
                bullets.SetSpeed(new Vector2(5, -5));
            }
            else
            {
                timeLoop = 0;
                cloneitem = GameObject.Instantiate(itemBom);
                cloneitem.transform.position = this.gameObject.transform.position + new Vector3(1, 0, 0);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.ToString() == "12") //Thorn Trap
        {
            selfDestruct = GameObject.Instantiate(selfDestruct);
            selfDestruct.transform.position = this.gameObject.transform.position;
            GameObject.Destroy(this.gameObject);
        }
    }
}
