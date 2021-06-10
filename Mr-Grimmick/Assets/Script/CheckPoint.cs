using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string SceneRevive;
    [SerializeField] Vector3 posRevive;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerPrefs.SetString("sceneRevive", SceneRevive);
            PlayerPrefs.SetFloat("posXRevive", posRevive.x);
            PlayerPrefs.SetFloat("posYRevive", posRevive.y);
            PlayerPrefs.SetFloat("posZRevive", posRevive.z);
        }
    }
}
