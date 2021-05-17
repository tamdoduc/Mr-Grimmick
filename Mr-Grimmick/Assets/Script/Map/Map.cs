using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter");
            PlayerPrefs.SetFloat("posXStart", 1);
            PlayerPrefs.SetFloat("posYStart", -17);
            PlayerPrefs.SetFloat("posZStart", 0);
            Application.LoadLevel("Scene 1.1");
        }
    }
}
