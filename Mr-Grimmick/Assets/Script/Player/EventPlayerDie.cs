using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayerDie : MonoBehaviour
{
    // Start is called before the first frame update
    float time;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time>5)
        {
            if (PlayerPrefs.GetInt("res") >=0)
            {
                PlayerPrefs.SetInt("isRevive", 1);
                PlayerPrefs.SetString("sceneRevive","Scene 1.1");
                PlayerPrefs.SetFloat("posXRevive", 1);
                PlayerPrefs.SetFloat("posYRevive", 1);
                PlayerPrefs.SetFloat("posZRevive", 0);

                Application.LoadLevel(PlayerPrefs.GetString("sceneRevive"));

            } else
            {
                Application.LoadLevel("Map");
            }
        }
    }
}
