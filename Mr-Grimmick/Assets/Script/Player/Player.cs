
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
    [SerializeField] LayerMask HeadEnemyLayer;
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

    [SerializeField] AudioSource Jump;
    [SerializeField] AudioSource HoldSkill;
    [SerializeField] AudioSource ShootSkill,EnergyBall,Bomb;
    [SerializeField] AudioSource audioFainting;
    [SerializeField] AudioSource audioDie;
    AudioSource cloneAudio;

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
        vel = new Vector2(0, 0);
        body.velocity = new Vector2(0, 0);
        if (cloneAnimationSkill != null)
            GameObject.Destroy(cloneAnimationSkill);
        if (cloneSkill != null)
            cloneSkill.SelfDestruct();
        timeHoldSkill = 0;
        isHoldingSkill = false;
    }
    void SetPosStart()
    {
        Vector3 posStart;
        if (PlayerPrefs.GetInt("isRevive") == 0)
        {
           // PlayerPrefs.SetInt("currentHp", PlayerPrefs.GetInt("maxHp"));
            posStart = new Vector3(PlayerPrefs.GetFloat("posXStart"), PlayerPrefs.GetFloat("posYStart"), PlayerPrefs.GetFloat("posZStart"));
        }
        else
        {
            PlayerPrefs.SetInt("currentHp", PlayerPrefs.GetInt("maxHp"));
            PlayerPrefs.SetInt("isRevive", 0);
            posStart = new Vector3(PlayerPrefs.GetFloat("posXRevive"), PlayerPrefs.GetFloat("posYRevive"), PlayerPrefs.GetFloat("posZRevive"));
        }
        this.transform.position = posStart;
        Debug.Log(posStart);
    }

    float timeAppear;
    [SerializeField] GameObject Gate;
    [SerializeField] Vector3 PosGate;
    void Start()
    {
        timeAppear = PlayerPrefs.GetFloat("timeAppear");
        if (timeAppear>0)
        {
            Gate = GameObject.Instantiate(Gate);
            Gate.transform.position = this.gameObject.transform.position + new Vector3(0, 0.5f, 0); 
        }
        SetPosStart();
        body = this.gameObject.GetComponent<Rigidbody2D>();
        IsPressJump = false;
        timePressJump = 0;
        vel = new Vector2(0, 0);
        isActive = true;
        isHoldingSkill = false;

        HP = PlayerPrefs.GetInt("currentHp");
       // Res = PlayerPrefs.GetInt("res");
    }

    bool untouchAble = false;
    float timeUntouchAble = 0;
    void Update()
    {
        if (timeAppear > 0)
        {
            timeAppear -= Time.deltaTime;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            if (timeAppear <= 0)
            {
                PlayerPrefs.SetFloat("timeAppear",0);
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                body.AddForce(new Vector2(0, 0.3f));
                GameObject.Destroy(Gate);
            }
            return;
        }
        if (isActive)
        {
            CheckCommand();
            FallDown();
        } 
        if (untouchAble)
        {
            timeUntouchAble += Time.deltaTime;
            if (timeUntouchAble >= 5)
            {
                this.gameObject.layer = 16; //player
                timeUntouchAble = 0;
                untouchAble = false;
            }
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        if (isFainting)
        {
            FallDown();
            timeFainting += Time.deltaTime;
            if (timeFainting >= 1f) 
            {
                isFainting = false;
                untouchAble = true;
                SetActive(true);
                timeFainting = 0;
                animator.SetBool("IsFainting", false);
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
    int typeSkill;
    void CheckSkill()
    {
        if (isHoldingSkill)
            timeHoldSkill += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.V) && !IsCollisionAreaSkill() && !isHoldingSkill && cloneSkill == null )
        {
            cloneAudio = AudioSource.Instantiate(HoldSkill);
            Destroy(cloneAudio.gameObject, 5);
            if (Input.GetKey(KeyCode.UpArrow) && PlayerPrefs.GetInt("item0") == 4)
                return;
            typeSkill = PlayerPrefs.GetInt("IDSkill");
            if (!Input.GetKey(KeyCode.UpArrow))
                typeSkill = 0;
            if (cloneAnimationSkill == null)
            {
                cloneAnimationSkill = GameObject.Instantiate(animationSkill);
                isHoldingSkill = true;
            }
        }
        if (cloneSkill == null && timeHoldSkill >= 1f)
        {
            InstanceSkill(typeSkill);
        }
        if (Input.GetKeyUp(KeyCode.V) && isHoldingSkill == true)
        {
            Shoot(typeSkill);
        }
    }
    void InstanceSkill(int typeSKill)
    { 
        if (cloneAnimationSkill!=null)
            GameObject.Destroy(cloneAnimationSkill.gameObject);
            switch (typeSKill)
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
        cloneSkill.gameObject.layer = 19;
    }
    public bool FollowAble()
    {
        return this.gameObject.layer == 16;
    }
    void Shoot(int typeSkill)
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
            cloneSkill.gameObject.layer = 11;
            if (this.gameObject.transform.rotation.y != 0)
                directory = -1;
            else
                directory = 1;
            switch (typeSkill)
            {
                case 0:
                    cloneAudio = AudioSource.Instantiate(ShootSkill);
                    Destroy(cloneAudio.gameObject, 1);
                    if (IsCollisionAreaSkill())
                        cloneSkill.SelfDestruct();
                    else
                        cloneSkill.Shot(directory, new Vector2(10, -15));
                    break;
                case 1:
                    cloneAudio = AudioSource.Instantiate(EnergyBall);
                    Destroy(cloneAudio.gameObject, 1);
                    PlayerPrefs.SetInt("item0", 0);
                    if (IsCollisionAreaSkill())
                        cloneSkill.SelfDestruct();
                    else
                        cloneSkill.Shot(directory, new Vector2(10, 10));
                    break;
                case 2:
                    cloneAudio = AudioSource.Instantiate(Bomb);
                    Destroy(cloneAudio.gameObject, 1);
                    PlayerPrefs.SetInt("item0", 0);
                    cloneSkill.Shot(directory, new Vector2(10, 0));
                    break;
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
                cloneAudio = AudioSource.Instantiate(Jump);
                Destroy(cloneAudio.gameObject, 1);
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
        if (isActive==false)
            body.velocity = vel;
    }
    bool IsColliderTop()
    {
        RaycastHit2D hit1 = Physics2D.BoxCast(colliderCheckTop.bounds.center, colliderCheckTop.bounds.size, 0, Vector2.up, 0.1f, GroundLayer);
        return hit1.collider != null;
    }
    bool IsGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.05f, GroundLayer);
        RaycastHit2D hit2D1 = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.05f, HeadEnemyLayer);
        return hit2D.collider != null || hit2D1.collider != null;
    }
    bool IsNearGrounded()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.3f, GroundLayer);
        RaycastHit2D hit2D1 = Physics2D.BoxCast(colliderCheckGround.bounds.center, colliderCheckGround.bounds.size, 0, Vector2.down, 0.3f, HeadEnemyLayer);
        return hit2D.collider != null || hit2D1.collider != null;
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
        HP = PlayerPrefs.GetInt("currentHp");
        if (this.gameObject.layer.ToString() == "16")
        {
            Debug.Log("actackkkkkkkkkkkkkkkkkkkkkkkkkkkk      "+ HP);
            gameObject.layer = 19; //immortal
            Shoot(PlayerPrefs.GetInt("IDSkill"));
            HP--; 
            HP = Mathf.Max(HP, 0);
            PlayerPrefs.SetInt("currentHp", HP);
            if (HP > 0)
            {
                cloneAudio = AudioSource.Instantiate(audioFainting);
                Destroy(cloneAudio.gameObject, 5);
                timeFainting = 0;
                isFainting = true;
                SetActive(false);
                timePressJump = 0;
                gameObject.layer = 19; //immortal
                animator.SetBool("IsFainting", true);
                IsPressJump = false;
                if (cloneEffectFLash == null)
                {
                    cloneEffectFLash = GameObject.Instantiate(effectPlash);
                    cloneEffectFLash.SetTimeMax(5);
                    cloneEffectFLash.SetSpriteRender(this.gameObject.GetComponent<SpriteRenderer>());
                    cloneEffectFLash.Active();
                }
            }
            if (HP == 0)
            {
                if (cloneSkill!=null)
                cloneSkill.SelfDestruct();
                if (cloneAnimationSkill!=null)
                Destroy(animationSkill);
                Die();
            }
        }
    }
    public bool IsFainting()
    {
        return isFainting;
    }
    public void Die()
    {
        timeHoldSkill = 0;
        isHoldingSkill = false;
        if (cloneAnimationSkill != null)
        {
            GameObject.Destroy(cloneAnimationSkill.gameObject);
        }
        if (cloneSkill!=null)
            cloneSkill.SelfDestruct();
        Res = PlayerPrefs.GetInt("res");
        Res--;
        Res = Mathf.Max(Res, -1);
        PlayerPrefs.SetInt("res", Res);
        if (eventPlayerDie!=null)
        GameObject.Instantiate(eventPlayerDie);
        cloneAudio = AudioSource.Instantiate(audioDie);
        Destroy(cloneAudio.gameObject, 5);
        if (afterDie != null)
            clone = GameObject.Instantiate(afterDie);
        GameObject.Destroy(this.gameObject);
    }
    public void FallDownWater()
    {
        timeHoldSkill = 0;
        isHoldingSkill = false;
        if (cloneAnimationSkill != null)
        {
            GameObject.Destroy(cloneAnimationSkill.gameObject);
        }
        if (cloneSkill != null)
            cloneSkill.SelfDestruct();
        Res = PlayerPrefs.GetInt("res");
        Res--;
        Res = Mathf.Max(Res, -1);
        PlayerPrefs.SetInt("res", Res);
        if (afterDie!=null)
        clone = GameObject.Instantiate(afterDie);
        GameObject.Destroy(this.gameObject);
    }
    [SerializeField] AterDie afterDie;
    AterDie clone;
}


