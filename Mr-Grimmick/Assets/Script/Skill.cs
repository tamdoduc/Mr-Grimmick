using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Animator animator;

    [SerializeField] ShadowSkill shadowSkill1;
    [SerializeField] ShadowSkill shadowSkill2;
    [SerializeField] Player player;
    [SerializeField] Collider2D collider2D;
    [SerializeField] Collider2D colliderD;
    [SerializeField] Collider2D colliderL;
    [SerializeField] Collider2D colliderR;
    [SerializeField] Collider2D colliderT;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Rigidbody2D body;

    [SerializeField] Vector2 velocity;

    Vector3[] positionShadow = new Vector3[9];

    [SerializeField] float timeExist;

    bool isShot;

    [SerializeField] int countCollision;

    void Start()
    {
        for (int i = 0; i < 6; i++) positionShadow[i] = transform.position;
        timeExist = 0;
        isShot = false;
        countCollision = 0;
        velocity = new Vector2(0, 0);
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        timeExist += Time.deltaTime;
        if (!isShot)
            this.gameObject.transform.position = player.transform.position + new Vector3(0, 1, 0);
        SetShadow();

        CheckCollision();
        if (isShot)
        {
            body.velocity = velocity;
            if (countCollision >= 100)               player.DestroySkill();
        }
    }
    void SetShadow()
    {
        for (int i = 8; i > 0; i--)
        {
            positionShadow[i] = positionShadow[i - 1];
        }
        positionShadow[0] = transform.position;

        if (timeExist >= 1)
        {
            body.velocity = velocity;
            if (shadowSkill1.IsNull())
            {
                shadowSkill1.Run();
                shadowSkill2.Run();
            }
            shadowSkill1.SetPosition(positionShadow[4]);
            shadowSkill2.SetPosition(positionShadow[8]);
        }

    }
    public void Shot()
    {
        isShot = true;
        float directory;
        bool left = player.transform.rotation.y == -1;
        Debug.Log(player.transform.rotation.y);
        if (left)
            directory = -1;
        else
            directory = 1;
        velocity = new Vector2(directory*8, -10f);
    }
    public bool IsShot()
    {
        return isShot;
    }
    public float GetTime()
    {
        return timeExist;
    }
    void CheckCollision()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderL.bounds.center, colliderL.bounds.size, 0, Vector2.left, 0.2f, GroundLayer);
        bool collisionL = hit2D.collider != null;
        hit2D = Physics2D.BoxCast(colliderR.bounds.center, colliderR.bounds.size, 0, Vector2.right, 0.2f, GroundLayer);
        bool collisionR = hit2D.collider != null;
        hit2D = Physics2D.BoxCast(colliderT.bounds.center, colliderT.bounds.size, 0, Vector2.up, 0.2f, GroundLayer);
        bool collisionU = hit2D.collider != null;
        hit2D = Physics2D.BoxCast(colliderD.bounds.center, colliderD.bounds.size, 0, Vector2.down, 0.2f, GroundLayer);
        bool collisionD = hit2D.collider != null;

        float decrease = 0.99f;

        Vector3 vel = velocity;
        if (collisionD)
        {
            vel.y = decrease* Mathf.Abs(velocity.y);
            countCollision++;
        } else if (collisionU)
        {
            vel.y = decrease* -Mathf.Abs(velocity.y);
            countCollision++;
        }
        if (collisionL)
        {
            vel.x = decrease* Mathf.Abs(velocity.x);
            countCollision++;
        } else if (collisionR)
        {
            vel.x = decrease* -Mathf.Abs(velocity.x);
            countCollision++;
        }

        velocity = vel;
        if (!collisionD && !collisionU )
            velocity -= new Vector2(0, 0.1f);
    }
    public int CountCollision()
    {
        return countCollision;
    }
}
