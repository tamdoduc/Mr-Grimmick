using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool isUp;
    [SerializeField] Pipe pipe;
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (!pipe.GetActive())
                pipe.Active(isUp);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
