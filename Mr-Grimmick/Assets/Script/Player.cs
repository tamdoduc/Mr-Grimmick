
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Vector2 vel;

    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ScrollBarLeft;
    [SerializeField] LayerMask ScrollBarRight;

    [SerializeField] bool IsPressJump;
    [SerializeField] float timePressJump;
    [SerializeField] Skill skill;
    Skill CloneSkill;

    [SerializeField] int countCollision;

    // Start is called before the first frame update
    void Start()
    {
        body = this.gameObject.GetComponent<Rigidbody2D>();
        IsPressJump = false;
        timePressJump = 0;
        vel = new Vector2(0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        CheckCommand();
        if (CloneSkill != null)
            countCollision = CloneSkill.CountCollision();
    }
    void CheckCommand()
    {

        CheckMove();
        CheckJumpt();
        CheckSkill();
        CheckScrollBar();

        body.velocity = vel;
    }     
    void CheckSkill()
    {
        if (Input.GetKeyDown(KeyCode.V))
        if (CloneSkill==null)
        {
            CloneSkill = GameObject.Instantiate(skill);
        }
        if (Input.GetKeyUp(KeyCode.V))
        if (CloneSkill!=null)
        {
            if (CloneSkill.GetTime()< 1.0f)
                DestroySkill();
            else if (!CloneSkill.IsShot())
                CloneSkill.Shot();
        }
    }
    public void DestroySkill()
    {
        GameObject.Destroy(CloneSkill.gameObject);
    }
    void CheckMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            vel.x = 6;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (h < 0)
        {
            vel.x = -6;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            vel.x = 0;
        }
        if (h != 0)
            animator.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        else
            animator.SetFloat("Speed", 0);
    }
    void CheckJumpt()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsPressJump = false;
            timePressJump = 0;
            vel.y = -7f;
        }

        if (!IsPressJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() ||  IsOnScrollBarLeft() || IsOnScrollBarRight()))
            {
                IsPressJump = true;
            }
        }
        else
        {
            timePressJump += Time.deltaTime;  //  1/1000 giaay
            vel.y = 15;
            if (timePressJump >= 0.25f) // time
            {
                timePressJump = 0;
                IsPressJump = false;
                vel.y = -7f;
            }
        }
        if (timePressJump == 0)
            if (IsGrounded() || IsOnScrollBarLeft() || IsOnScrollBarRight())
            {
                vel.y = -3f;
                this.transform.position += new Vector3(0, 0.0000005f);
            }
            else
                vel.y = -7;
        if (IsGrounded()||IsOnScrollBarLeft()||IsOnScrollBarRight())
        {
            animator.SetBool("IsJumpt", false);
        }
        else
        {
            animator.SetBool("IsJumpt", true);
        }
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, GroundLayer);
        return hit2D.collider != null;
    }
    void CheckScrollBar()
    {
        if (IsOnScrollBarLeft())
            vel.x -= 4f;
        if (IsOnScrollBarRight())
            vel.x += 4f;
    }
    bool IsOnScrollBarLeft()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, ScrollBarLeft);
        return hit2D.collider != null;
    }
    bool IsOnScrollBarRight()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, ScrollBarRight);
        return hit2D.collider != null;
    }
}


