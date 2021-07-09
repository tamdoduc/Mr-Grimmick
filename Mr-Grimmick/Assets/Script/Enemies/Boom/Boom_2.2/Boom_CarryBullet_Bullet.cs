using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_CarryBullet_Bullet : MonoBehaviour
{
    [SerializeField] Transform boom_CarryBullet;
    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderCheckFront;
    [SerializeField] Collider2D colliderBigBody;
    [SerializeField] CheckTop checkTop;
    [SerializeField] LayerMask skillLayer;
    [SerializeField] Boom_CarryBullet target;
    [SerializeField] Boom_CarryBullet_Bullet boom;
    Boom_CarryBullet_Bullet clone;
    [SerializeField] SelfDestruct selfDestruct;

    private float handleTime = 0, bounceTime = 2f;
    private bool bounce = false, directLeft = false;
    Vector3 posBefore;
    // Start is called before the first frame update
    void Start()
    {
        checkTop.SetActive(true);
        posBefore = boom_CarryBullet.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bounce)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.position = boom_CarryBullet.transform.position + new Vector3(0, 0.9f, 0);
            if (IsColliderSkill())
            {
                bounce = true;
                clone = GameObject.Instantiate(boom);
                clone.transform.position = this.gameObject.transform.position + new Vector3(0, 0, 0);
                clone.SetValue(bounce, directLeft);
                GameObject.Destroy(this.gameObject);

            }
        }
        else
        {
            handleTime += Time.deltaTime;
            if (handleTime > bounceTime)
            {
                selfDestruct = GameObject.Instantiate(selfDestruct);
                selfDestruct.transform.position = this.gameObject.transform.position;
                GameObject.Destroy(this.gameObject);
            }
        }
    }
    public void SetValue(bool bou, bool dir)
    {
        bounce = bou;
        directLeft = dir;
        if (directLeft)
        {
            body.AddForce(Vector2.left * 3f, ForceMode2D.Impulse);
        }
        else
            body.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
    }
    bool IsColliderSkill()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderBigBody.bounds.center, colliderBigBody.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        if (hit2D.collider)
        {
            Debug.Log(IsColliderFront() + " " + target.faceRight);
            if (IsColliderFront())
                directLeft = !target.faceRight;
            else
                directLeft = target.faceRight;
        }
        return hit2D.collider != null;
    }
    bool IsColliderFront()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckFront.bounds.center, colliderCheckFront.bounds.size, 0, Vector2.up, 0.1f, skillLayer);
        return hit2D.collider != null;
    }
}
