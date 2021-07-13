using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject animationDestruct;
    [SerializeField] Vector3 pos;

    GameObject clone;
    Player player;

    [SerializeField] AudioSource audio;
    AudioSource cloneAudio;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() != "8")
        {
            if (collision.gameObject.name != "Player")
                GameObject.Destroy(collision.gameObject);
            else player.FallDownWater();
            if (cloneAudio == null)
            {
                cloneAudio = AudioSource.Instantiate(audio);
                Destroy(cloneAudio.gameObject, 1);
            }
            if (animationDestruct!=null)
            clone = GameObject.Instantiate(animationDestruct);
            pos.x = collision.gameObject.transform.position.x;
            clone.transform.position = pos;
        }
    }
}
