using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D body;
    public void AddForce(Vector2 force)
    {
        body.AddForce(force);
    }
}
