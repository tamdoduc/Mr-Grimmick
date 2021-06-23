
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Vector2 vel;

    [SerializeField] Rigidbody2D body;
    [SerializeField] Collider2D colliderBody;
    [SerializeField] Collider2D colliderCheckGround;
    [SerializeField] Collider2D colliderCheckTop;
    [SerializeField] Collider2D colliderCheckSkillArea;
    [SerializeField] Animator animator;

    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask SkillLayer;

    [SerializeField] bool IsPressJump;
    [SerializeField] float timePressJump;

    float timeHoldSkill = 0;
    bool isHoldingSkill = false;
    [SerializeField] GameObject animationSkill;
    GameObject cloneAnimationSkill;
    [SerializeField] Skill skill;
    [SerializeField] Bomb bomb;
    [SerializeField] EnergyBall energyBall;
    SkillTemp cloneSkill;

    [SerializeField] bool isActive;
    [SerializeField] bool isFainting;
    float timeFainting = 0;
    [SerializeField] EffectFlash effectPlash;
    EffectFlash cloneEffectFLash;

    const float MaxVelocityXRight = 5f;
    const float MaxVelocityXLeft = -5f;
    const float MaxVelocityY = 8f;
    const float MaxGravity = -10f;

    int HP;
    int Res;
    [SerializeField] EventPlayerDie eventPlayerDie;


    public void SetActive(bool active)
    {
        isActive = active;
        if (!isActive)
            Stop();
    }
    void Stop()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFainting", false);
        body.velocity = new Vector2(0, 0);
    }
    void SetPosStart()
    {
        Vector3 posStart;
        if (PlayerPrefs.GetInt("isRevive") == 0)
        {
            //PlayerPrefs.SetFloat("posXStart", 62.5f);
            //PlayerPrefs.SetFloat("posYStart", -12.5f);
            //PlayerPrefs.SetFloat("posZStart", 0);

            posStart = new Vector3(PlayerPrefs.GetFloat("posXStart"), PlayerPrefs.GetFloat("posYStart"), PlayerPrefs.GetFloat("posZStart"));
        }
        else
        {
            PlayerPrefs.SetInt("isRevive", 0);
            posStart = new Vector3(PlayerPrefs.GetFloat("posXRevive"), PlayerPrefs.GetFloat("posYRevive"), PlayerPrefs.GetFloat("posZRevive"));
        }
        this.transform.position = posStart;

    }
    void Start()
    {
        body = this.gameObject.GetComponent<Rigidbody2D>();
        IsPressJump = false;
        timePressJump = 0;
        vel = new Vector2(0, 0);
        isActive = true;
        isHoldingSkill = false;

        HP = PlayerPrefs.GetInt("currentHp");
        Res = PlayerPrefs.GetInt("res");
    }

    void Update()
    {
        if (isActive)
        {
            CheckCommand();
            FallDown();
        } 
        if (isFainting)
        {
            FallDown();
            timeFainting += Time.deltaTime;
            if (timeFainting >= 0.5f) 
            {
                timeFainting = 0;
                animator.SetBool("IsFainting", false);
                isFainting = false;
                SetActive(true);
                gameObject.layer = 16; //player
            }
        }
    }
    void CheckCommand()
    {
        CheckMove();
        CheckJump();
        CheckSkill();

        body.velocity = vel;
    }
    public bool IsHoldingSkill()
    {
        return isHoldingSkill;
    }
    void CheckSkill()
    {
        int typeSkill = PlayerPrefs.GetInt("IDSkill");
        if (isHoldingSkill)
            timeHoldSkill += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.V) && !IsCollisionAreaSkill() && !isHoldingSkill && cloneSkill == null)
        {
            if (cloneAnimationSkill == null)
            {
                cloneAnimationSkill = GameObject.Instantiate(animationSkill);
                isHoldingSkill = true;
            }
        }

        if (cloneSkill==null && timeHoldSkill >= 1f)
        {
            GameObject.Destroy(cloneAnimationSkill.gameObject);
            switch (typeSkill)
            {
                case 0:
                    cloneSkill = GameObject.Instantiate(skill);
                    break;
                case 1:
                    PlayerPrefs.SetInt("item0", PlayerPrefs.GetInt("item0") + 3);
                    cloneSkill = GameObject.Instantiate(bomb);
                    break;
                case 2:
                    PlayerPrefs.SetInt("item0", PlayerPrefs.GetInt("item0") + 3);
                    cloneSkill = GameObject.Instantiate(energyBall);
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.V) && isHoldingSkill == true)
        {
            isHoldingSkill = false;
            timeHoldSkill = 0;
            if (cloneAnimationSkill != null)
            {
               GameObject.Destroy(cloneAnimationSkill.gameObject);
            }
            if (cloneSkill != null)
            {
                int directory;
                if (this.gameObject.transform.rotation.y != 0)
                    directory = -1;
                else
                    directory = 1;
                switch (typeSkill)
                {
                    case 0:
                        if (IsCollisionAreaSkill())
                            cloneSkill.SelfDestruct();
                        else
                            cloneSkill.Shot(directory, new Vector2(10, -15));
                        break;
                    case 1:
                        PlayerPrefs.SetInt("item0", 0);
                        if (IsCollisionAreaSkill())
                            cloneSkill.SelfDestruct();
                        else
                            cloneSkill.Shot(directory, new Vector2(10, 10));
                        break;
                    case 2:
                        PlayerPrefs.SetInt("item0", 0);
                        cloneSkill.Shot(directory, new Vector2(10, 0));
                        break;
                }
            }
        }
    }
    void CheckMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            if (vel.x < MaxVelocityXRight)
            {
                vel.x += (Time.deltaTime * MaxVelocityXRight) / (0.5f);  //    0.5 là thời gian cần để đạt max     
            }
            else
            if (vel.x > MaxVelocityXRight)
            {
                vel.x = MaxVelocityXRight;
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (h < 0)
        {
            if (vel.x > MaxVelocityXLeft)
            {
                vel.x += (Time.deltaTime * MaxVelocityXLeft) / (0.5f);
            }
            else
            if (vel.x < MaxVelocityXLeft)
            {
                vel.x = MaxVelocityXLeft;
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (h == 0)
        {
            if (vel.x > 0)
            {
                vel.x = Mathf.Max(vel.x - (Time.deltaTime * MaxVelocityXRight) / (0.2f), 0);
            }
            if (vel.x < 0)
            {
                vel.x = Mathf.Min(vel.x - (Time.deltaTime * MaxVelocityXLeft) / (0.2f), 0);
            }
        }
        animator.SetFloat("Speed", Mathf.Abs(vel.x));
    }
    void CheckJump()
    {
        if (Input.GetKeyUp(KeyCode.Space) || IsColliderTop()) // Cancel jump
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
            if (Input.GetKeyDown(KeyCode.Space) && (IsNearGrounded() || IsOnSkillLayer())) // Start jump
            {
                IsPressJump = true;
            }
        }
        else   //  Jumping
        {
            timePressJump += Time.deltaTime;
            vel.y = MaxVelocityY;
            if (timePressJump >= 0.4f)
            {
                timePressJump = 0;
                IsPressJump = false;
            }
        }
    }
    void FallDown()
    {
        if (timePressJump == 0) // Fall down
            if (IsGrounded())
            {
                    vel.y = -1f;
            }
            else if (vel.y >= MaxGravity)
            {
                vel.y -= Time.deltaTime * (MaxVelocityY - MaxGravity) / 0.3f;
            }
        if (IsNearGrounded()) // Check state jump
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
        return hit1.collider != null;
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
    bool IsOnSkillLayer()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.18f, SkillLayer);
        return hit2D.collider != null;
    }
    bool IsCollisionAreaSkill()
    {
        RaycastHit2D hit1 = Physics2D.BoxCast(colliderCheckSkillArea.bounds.center, colliderCheckTop.bounds.size, 0, Vector2.up, 0.1f, GroundLayer);
        return hit1.collider != null;
    }
    public void BeActack()
    {
        //HP--;
        HP = Mathf.Max(HP, 0);
        PlayerPrefs.SetInt("currentHp", HP);
        if (HP > 0)
        {
            isFainting = true;
            SetActive(false);
            gameObject.layer = 19; //immortal
            IsPressJump = false;
            timePressJump = 0;
            animator.SetBool("IsFainting", true);
            if (cloneEffectFLash == null)
            {
                cloneEffectFLash = GameObject.Instantiate(effectPlash);
                cloneEffectFLash.SetTimeMax(2);
                cloneEffectFLash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                cloneEffectFLash.Active();
            }
        }
        if (HP == 0)
        {
            Die();
        }
    }
    public bool IsFainting()
    {
        return isFainting;
    }
    public void Die()
    {
        Res--;
        Res = Mathf.Max(Res, -1);
        PlayerPrefs.SetInt("res", Res);
        GameObject.Instantiate(eventPlayerDie);
        GameObject.Destroy(this.gameObject);
    }
}


