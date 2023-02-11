using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AterDie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    float time = 0;
    Camera cam;
    [SerializeField] GameOver gameOver;
    GameOver clone;
    void Update()
    {
        time += Time.deltaTime;
        if (PlayerPrefs.GetInt("res") >= 0)
        {
            if (time > 5)
            {
                PlayerPrefs.SetInt("isRevive", 1);
                Application.LoadLevel(PlayerPrefs.GetString("sceneRevive"));

            }
        }
        else
        {
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
            clone = GameOver.Instantiate(gameOver);
            clone.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,0);
            Destroy(this.gameObject);
        }

    }
}
