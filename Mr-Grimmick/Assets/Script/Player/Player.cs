
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Vector2 vel;

    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckTop;
    [SerializeField] Animator animator;

    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ScrollBarLeftLayer;
    [SerializeField] LayerMask ScrollBarRightLayer;
    [SerializeField] LayerMask SkillLayer;

    [SerializeField] bool IsPressJump;
    [SerializeField] float timePressJump;
    [SerializeField] Skill skill;
    Skill CloneSkill;

    [SerializeField] bool isActive;

    float MaxVelocityXRight = 5f;
    float MaxVelocityXLeft = -5f;
    float MaxVelocityY = 7.5f;
    float MaxGravity = -8f;
    float VelocityXIdle = 0;


    public void SetActive(bool active)
    {
        isActive = active;
        if (!isActive)
            Stop();    
    }
    private void Stop()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("IsJumpt", false);
        animator.SetBool("IsFaint", false);
        body.velocity = new Vector2(0, 0);
    }
    void Start()
    {
        //PlayerPrefs.SetFloat("posXStart", 1);
        //PlayerPrefs.SetFloat("posYStart", -17);
        //PlayerPrefs.SetFloat("posZStart", 0);
        Vector3 newPos = new Vector3(PlayerPrefs.GetFloat("posXStart"), PlayerPrefs.GetFloat("posYStart"), PlayerPrefs.GetFloat("posZStart"));
        //this.transform.position = newPos;
        body = this.gameObject.GetComponent<Rigidbody2D>();
        IsPressJump = false;
        timePressJump = 0;
        vel = new Vector2(0, 0);
        isActive = true;
    }

    void Update()
    {
       // Debug.Log(Time.deltaTime);
        if (isActive)
            CheckCommand();
    }
    void ReSetPerFrame()
    {
        VelocityXIdle = 0;
        MaxVelocityXLeft = -5;
        MaxVelocityXRight = 5;
    }
    void CheckCommand()
    {
        ReSetPerFrame();
        CheckScrollBar();
        CheckMove();
        CheckJump();
        CheckSkill();

        //this.transform.position += new Vector3(0, 0.00001f *Time.deltaTime);
        body.velocity = vel;
    }     
    void CheckSkill()
    {
        if (Input.GetKeyDown(KeyCode.V) && IsGrounded())
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
            if (vel.x<MaxVelocityXRight)
            {
                vel.x += (Time.deltaTime * MaxVelocityXRight) / (0.5f);  //    0.5 là thời gian cần để đạt max     
            }
            else
            if (vel.x >MaxVelocityXRight)
            {
                vel.x = MaxVelocityXRight;
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (h < 0)
        {
            if (vel.x > MaxVelocityXLeft)
            {
                vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.5f);  //  0.5 là thời gian cần để đạt max       
            }
            else
            if (vel.x < MaxVelocityXLeft)
            {
                vel.x = MaxVelocityXLeft;
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (h==0)
        {
            if (vel.x > VelocityXIdle)
            {
                vel.x -= (Time.deltaTime * MaxVelocityXRight) / (0.2f);  //  0.2 là thời gian cần để đạt max     
            }
            if (vel.x < VelocityXIdle)
            {
                vel.x -= (Time.deltaTime * MaxVelocityXLeft) / (0.2f);  //   0.2 là thời gian cần để đạt max      
            }
            if (Mathf.Abs(vel.x) <Mathf.Abs(VelocityXIdle) + 0.2f )  //0.2 là chỉ số làm tròn
                vel.x = VelocityXIdle;
        }
        animator.SetFloat("Speed",Mathf.Abs( vel.x - VelocityXIdle));
    }
    void CheckJump()
    {
        if (Input.GetKeyUp(KeyCode.Space) || IsColliderTop())
        {
            IsPressJump = false;
            timePressJump = 0;
            if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }
        }

        if (!IsPressJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && (IsNearGrounded() ||  IsOnScrollBarLeftLayer() || IsOnScrollBarRightLayer()||IsOnSkillLayer()))
            {
                IsPressJump = true;
            }
        }
        else
        {
            timePressJump += Time.deltaTime; //Time last frame
            vel.y = MaxVelocityY;
            if (timePressJump >= 0.4f) // time (s)
            {
                timePressJump = 0;
                IsPressJump = false;
            }
        }
        if (timePressJump == 0)
            if (IsGrounded() || IsOnScrollBarLeftLayer() || IsOnScrollBarRightLayer())
            {
                vel.y = -1f;
            }
            else if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }
        if (IsNearGrounded()||IsOnScrollBarLeftLayer()||IsOnScrollBarRightLayer())
        {
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }
    }
    bool IsColliderTop()
    {
        RaycastHit2D hit1 = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckTop.bounds.size, 0, Vector2.up, 0.1f, GroundLayer);
        RaycastHit2D hit2 = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.up, 0.18f, ScrollBarRightLayer);
        RaycastHit2D hit3 = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.up, 0.18f, ScrollBarLeftLayer);
        return hit1.collider != null ||hit2.collider!=null ||hit3.collider!=null;
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.05f, GroundLayer);
        return hit2D.collider != null;
    } 
    bool IsNearGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.3f, GroundLayer);
        return hit2D.collider != null;
    }
    void CheckScrollBar()
    {
        if (IsOnScrollBarLeftLayer())
        {
            VelocityXIdle = -3;
            MaxVelocityXRight = 3;
            MaxVelocityXLeft = -8;
        }
        if (IsOnScrollBarRightLayer())
        {
            VelocityXIdle = 3;
            MaxVelocityXRight = 8;
            MaxVelocityXLeft = -3;
        }
    }
    bool IsOnScrollBarLeftLayer()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, ScrollBarLeftLayer);
        return hit2D.collider != null;
    }
    bool IsOnScrollBarRightLayer()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, ScrollBarRightLayer);
        return hit2D.collider != null;
    }  
    bool IsOnSkillLayer()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, SkillLayer);
        return hit2D.collider != null;
    }
}


