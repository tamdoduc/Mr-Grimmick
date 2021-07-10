using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTop : MonoBehaviour
{
    // Start is called before the first frame update
    bool isActive;
    bool isBelowPlayer;
    Player player;

    [SerializeField] GameObject Temp;
    Vector3 posBefore;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        posBefore = Temp.gameObject.transform.position;
        isActive = true;
    }
    
    private void Update()
    {
        if ( isActive)
        {
            if (player.gameObject.layer.ToString() == "16" && isBelowPlayer && player!=null)
            {
                Debug.LogError("fffffffffffffffff");
                Vector3 range = Temp.transform.position - posBefore;
                player.transform.position += range;
                Debug.Log("f");
            }
            posBefore = Temp.transform.position;
        }
    }
    public bool IsBelowPlayer()
    {
        return isBelowPlayer;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player"
            && collision.gameObject.transform.position.y >= this.gameObject.transform.position.y )
        {
            isBelowPlayer = true;
            Debug.Log("on top");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isBelowPlayer = false;
        }
    }
    public void SetActive(bool b)
    {
        isActive = b;
    }

}
