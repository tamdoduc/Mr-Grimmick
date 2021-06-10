using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTemp : MonoBehaviour
{
    protected bool isExist;
    protected bool isShooted;
    protected float timeShooted;
    protected Vector2 velocity;

    protected Player player;

    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D body;

    public SkillTemp()
    {
        isShooted = false;
        timeShooted = 0;
        velocity = new Vector2(0, 0);
    }
    void Start()
    {

    }

    void Update()
    {
        Debug.Log("1");
    }
    public void Shot(int directory, Vector2 speed)
    {
        isShooted = true;
        velocity = new Vector2(directory * speed.x, speed.y);
    }
    virtual public void SelfDestruct(){}
}
