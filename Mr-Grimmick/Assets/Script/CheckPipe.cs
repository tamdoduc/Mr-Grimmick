using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPipe : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Player player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() == "23")
        {
            player.SetActive(false);
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() == "23")
        {
            player.SetActive(true);
        }
    }
}