using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timeMax;
    float time;
    [SerializeField] AudioSource audio;
    void Start()
    {
        time = 0;
       // audio = AudioSource.Instantiate(audio);
       // Destroy(audio.gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time>=timeMax)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
