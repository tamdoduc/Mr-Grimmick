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

    Vector3[] positionShadow = new Vector3[3];

    [SerializeField] float timeExist;
    [SerializeField] float timeChangeShadowPos;

    bool isShot;

    [SerializeField] int countCollision;

    [SerializeField] SelfDestruct selfDestruct;

    void Start()
    {
        for (int i = 0; i < 3; i++) positionShadow[i] = transform.position;
        timeExist = 0;
        timeChangeShadowPos = 0;
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
        timeChangeShadowPos += Time.deltaTime;
        if (timeChangeShadowPos >= 0.02f)
        {
            timeChangeShadowPos = 0;
            SetShadow();
        }

        if (isShot)
        {
            CheckCollision();
            body.velocity = velocity;
            if (countCollision >= 20)
            {
                SelfDestruct();
            }
        }
    }
    public void SelfDestruct()
    {
        selfDestruct = GameObject.Instantiate(selfDestruct);
        selfDestruct.transform.position = this.gameObject.transform.position;
        GameObject.Destroy(this.gameObject);
    }    
    void SetShadow()
    {
        for (int i = 2; i > 0; i--)
        {
            positionShadow[i] = positionShadow[i - 1];
        }
        positionShadow[0] = transform.position;

        if (timeExist >= 0.7f)
        {
            body.velocity = velocity;
            if (shadowSkill1.IsNull())
            {
                shadowSkill1.Run();
                shadowSkill2.Run();
            }
            shadowSkill1.SetPosition(positionShadow[1]);
            shadowSkill2.SetPosition(positionShadow[2]);
        }

    }
    public void Shot()
    {
        isShot = true;
        float directory;
        bool left = !(player.transform.rotation.y == 0);
        if (left)
            directory = -1;
        else
            directory = 1;
        velocity = new Vector2(directory * 10, -15f);
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
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderL.bounds.center, colliderL.bounds.size, 0, Vector2.left, 0.1f, GroundLayer);
        bool collisionL = hit2D.collider != null;
        hit2D = Physics2D.BoxCast(colliderR.bounds.center, colliderR.bounds.size, 0, Vector2.right, 0.1f, GroundLayer);
        bool collisionR = hit2D.collider != null;
        hit2D = Physics2D.BoxCast(colliderT.bounds.center, colliderT.bounds.size, 0, Vector2.up, 0.1f, GroundLayer);
        bool collisionU = hit2D.collider != null;
        hit2D = Physics2D.BoxCast(colliderD.bounds.center, colliderD.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        bool collisionD = hit2D.collider != null;

        float decrease = 0.7f;

        Vector3 vel = velocity;
        if (collisionD)
        {
            if (velocity.y < 0)
            {
                vel.y = decrease * Mathf.Abs(velocity.y);
                countCollision++;
            }
        }
        else if (collisionU)
        {
            if (velocity.y > 0)
            {
                vel.y = decrease * -Mathf.Abs(velocity.y);
                countCollision++;
            }
        }
    //    else
        if (collisionL)
        {
            if (velocity.x < 0)
            {
                vel.x = decrease * Mathf.Abs(velocity.x);
                countCollision++;
            }
        }
        else if (collisionR)
        {
            if (velocity.x > 0)
            {
                vel.x = decrease * -Mathf.Abs(velocity.x);
                countCollision++;
            }
        }

        velocity = vel;
        velocity.y -= Time.deltaTime *35;
    }
    public int CountCollision()
    {
        return countCollision;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.ToString() == "12") //Thorn Trap
        {
            SelfDestruct();
        }
    }
}
