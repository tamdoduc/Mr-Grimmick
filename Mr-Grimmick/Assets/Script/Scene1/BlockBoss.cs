using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBoss : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Collider2D colliderBlockBoss;
    [SerializeField] float triggerPoint;
    [SerializeField] bool blockLeftSide = true; //true is block left
    [SerializeField] AudioSource bossSE;
    AudioSource cloneAudio;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blockLeftSide)
        {
            if (target.transform.position.x > triggerPoint)
            {
                colliderBlockBoss.isTrigger = false;
                if (cloneAudio == null)
                    cloneAudio = AudioSource.Instantiate(bossSE);
            }
        }
        else
        {
            if (target.transform.position.x < triggerPoint)
            {
                colliderBlockBoss.isTrigger = false;
                if (cloneAudio == null)
                    cloneAudio = AudioSource.Instantiate(bossSE);
            }
        }

    }
}
