using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayerDie : MonoBehaviour
{
    // Start is called before the first frame update
    float time;
    Vector2[] velocity;
    [SerializeField] AnimationPlayerDie animationPlayer;

    void Start()
    {
        float radian = 0.415f;
        velocity = new Vector2[15];
        velocity[0] = new Vector2(0, 7);
        for (int i = 1; i < 15; i++)
        {
            velocity[i].x = velocity[i - 1].x * Mathf.Cos(radian) - velocity[i - 1].y * Mathf.Sin(radian);
            velocity[i].y = velocity[i - 1].x * Mathf.Sin(radian) + velocity[i - 1].y * Mathf.Cos(radian);
        }
        for (int i = 0; i < 15; i++)
        {
           // Debug.Log(velocity[i]);
            animationPlayer = GameObject.Instantiate(animationPlayer);
            animationPlayer.gameObject.transform.position = GameObject.Find("Player").transform.position;
            animationPlayer.SetVelocity(velocity[i]);
        }
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
                //PlayerPrefs.SetString("sceneRevive","Scene 1.1");
                //PlayerPrefs.SetFloat("posXRevive", 1);
                //PlayerPrefs.SetFloat("posYRevive", 1);
                //PlayerPrefs.SetFloat("posZRevive", 0);

                Application.LoadLevel(PlayerPrefs.GetString("sceneRevive"));

            } else
            {
                Application.LoadLevel("Map");
            }
        }
    }
}
