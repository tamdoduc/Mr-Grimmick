using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string name;
    [SerializeField] int score;
    void Start()
    {
        PlayerPrefs.SetInt("treasure" + name, 0);
        if (PlayerPrefs.GetInt("treasure" + name) == 1)
            GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] AudioSource audio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            audio = AudioSource.Instantiate(audio);
            Destroy(audio, 10);
            PlayerPrefs.SetInt("treasure" + name,1);
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + score);
            GameObject.Destroy(this.gameObject);
        }
    }
}
