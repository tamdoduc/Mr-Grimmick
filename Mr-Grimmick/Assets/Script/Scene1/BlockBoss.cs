using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBoss : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Collider2D colliderBlockBoss;
    [SerializeField] float triggerPoint, groundLim;
    [SerializeField] bool blockLeftSide = true; //true is block left
    [SerializeField] AudioSource bossSE;
    AudioSource cloneAudio;
    AudioSource AudioBackGround;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        AudioBackGround = GameObject.Find("AudioBackGround").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blockLeftSide)
        {
            if (target.transform.position.x > triggerPoint && target.position.y > groundLim)
            {
                colliderBlockBoss.isTrigger = false;
                if (cloneAudio == null)
                    cloneAudio = AudioSource.Instantiate(bossSE);
                if (AudioBackGround!=null)
                GameObject.Destroy(AudioBackGround.gameObject);
            }
        }
        else
        {
            if (target.transform.position.x < triggerPoint && target.position.y > groundLim)
            {
                colliderBlockBoss.isTrigger = false;
                if (cloneAudio == null)
                    cloneAudio = AudioSource.Instantiate(bossSE);
                if (AudioBackGround != null)
                    GameObject.Destroy(AudioBackGround.gameObject);
            }
        }

    }
}
