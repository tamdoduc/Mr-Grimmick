
using UnityEngine;

enum State
{
    IDLELEFT,
    IDLERIGHT,
    WALKLEFT,
    WALKRIGHT,
    JUMPTLEFT,
    JUMPRIGHT,
    FAINT,
    DIE
}
struct InfoJumpt
{
    public float timeJumpt;
    const float maxTimeJumpt = 1;
}

public class Player : MonoBehaviour
{
    [SerializeField] Vector2 vel;

    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask GroundLayer;

    [SerializeField] bool IsPressJump;
    [SerializeField] float timePressJump;
    [SerializeField] Skill skill;
    Skill CloneSkill;

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
    }
    void CheckCommand()
    {

        CheckMove();
        CheckJumpt();
        CheckSkill();

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
            vel.x = 5;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (h < 0)
        {
            vel.x = -5;
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
            if (Input.GetKeyDown(KeyCode.Space) && IsNearGrounded())
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
            if (IsGrounded())
            {
                vel.y = -0.0001f;
                this.transform.position += new Vector3(0, 0.0000005f);
            }
            else
                vel.y = -7;
        if (IsNearGrounded())
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
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.1f, GroundLayer);
        return hit2D.collider != null;
    }
    bool IsNearGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, GroundLayer);
        return hit2D.collider != null;
    }
}


