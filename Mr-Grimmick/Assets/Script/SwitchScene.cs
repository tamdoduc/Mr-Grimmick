using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string nextScene;
    [SerializeField] Vector3 NewPosInNextScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Application.LoadLevel(nextScene);
            PlayerPrefs.SetFloat("posXStart", NewPosInNextScene.x);
            PlayerPrefs.SetFloat("posYStart", NewPosInNextScene.y);
            PlayerPrefs.SetFloat("posZStart", NewPosInNextScene.z);
            Debug.Log(nextScene);
        }
    }
}
