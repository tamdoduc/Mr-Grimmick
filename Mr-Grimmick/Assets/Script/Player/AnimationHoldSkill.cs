using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHoldSkill : MonoBehaviour
{
    // Start is called before the first frame update
    Player Player;
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = Player.transform.position + new Vector3(0, 1, 0);
    }
}
