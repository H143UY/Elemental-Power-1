using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEarthController : ObjectController
{

    private HpPlayerController hpPlayer;
    private CapsuleCollider2D capsul;
    private bool StopPlayer;
    public static PlayerEarthController Instance;
    [Header("Chỉ số người chơi")]
    public float JumpForce;
    [Header("di chuyển")]
    private float horizontal;
    private float vertical;
    private Vector2 Direction;
    private Animator anim;
    private Rigidbody2D rig;
    public bool isground;
    [Header("Roll")]
    public bool CanRoll = false;
    private float TimeRollCollDown;
    public float TimeRoll;
    public float TimeCanRoll;
    public float powerRoll = 3.5f;
    [Header("tan cong")]
    public int combo = 1;
    public bool attacking;
    public float comboTiming;
    private float comboTempo;
    private int comboNumber = 3;
    private bool stopcombo = false;
    public bool hitdame = false;
    [Header("Air Attack")]
    public bool air_attack = false;
    public float Time_air;
    private bool CanAir_Att;
    [Header("Skill")]
    public bool skill = false;
    public float TimeToSkill;
    private bool CanSkill;
    [Header("defend")]
    public bool defend = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        this.RegisterListener(EventID.CanAirAttack, (sender, param) =>
        {
            CanAir_Att = true;
        });
        this.RegisterListener(EventID.CanSkill, (sender, param) =>
        {
            CanSkill = true;
        });
        this.RegisterListener(EventID.SinhQuaiXong, (sender, param) =>
        {
            StopPlayer = false;
        });
    }
    void Start()
    {
        capsul = GetComponent<CapsuleCollider2D>();
        hpPlayer = GetComponent<HpPlayerController>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        comboTiming = 0.6f;
        comboTempo = comboTiming;
        TimeRollCollDown = 1f;
        TimeRoll = TimeRollCollDown;
        Time_air = 10f;
        TimeToSkill = 20f;
        CanAir_Att = true;
        CanSkill = true;
        StopPlayer = false;
    }

    void Update()
    {
        if (StopPlayer)
        {
            AnimPlayer();
            return;
        }
        if (hpPlayer.CurrentHp <= 0)
        {
            anim.SetTrigger("death");
        }
        if (hitdame)
        {
            anim.SetTrigger("hit");
            return;
        }
        DiChuyen();
        vertical = rig.velocity.y;
        Roll();
        Flip();
        AnimPlayer();
        ComboAttack();
        PhongThu();
        Skill();
        AirAttack();
    }
    private void DiChuyen()
    {
        horizontal = 0;
        if (Input.GetKeyDown(KeyCode.W) && isground)
        {
            Jump();
        }
        horizontal = Input.GetAxis("Horizontal");
        Direction = new Vector3(horizontal, 0);
        if (!defend)
        {
            Move(Direction);
        }
    }
    public void AirAttack()
    {
        if (Input.GetKeyDown(KeyCode.U) && !air_attack && !hitdame && CanAir_Att)
        {
            air_attack = true;
            CanAir_Att = false;
        }
    }
    public void Skill()
    {
        if (Input.GetKeyDown(KeyCode.I) && !skill && !hitdame && CanSkill)
        {
            CanSkill = false;
            skill = true;
        }
    }
    public void ComboAttack()
    {
        comboTempo -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.J) && comboTempo < 0 && !stopcombo && !hitdame && !skill && !air_attack)
        {
            attacking = true;
            anim.SetTrigger("attack" + combo);
            comboTempo = comboTiming;
        }
        else if (Input.GetKeyDown(KeyCode.J) && comboTempo > 0 && comboTempo < 0.5f && !stopcombo && !hitdame && !skill && !air_attack)
        {
            attacking = true;
            combo++;
            if (combo >= comboNumber)
            {
                stopcombo = true;
                combo = 3;
            }
            anim.SetTrigger("attack" + combo);
            comboTempo = comboTiming;
        }
        else if (comboTempo < 0 && !Input.GetKeyDown(KeyCode.J))
        {
            attacking = false;
            stopcombo = false;
        }
        if (comboTempo < 0)
        {
            combo = 1;
        }
    }
    public void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hitdame && isground)
        {
            if (TimeCanRoll > 1.5f)
            {
                capsul.enabled = false;
                CanRoll = true;
                rig.gravityScale = 0;
            }
        }
        if (CanRoll)
        {
            rig.velocity = new Vector2(transform.localScale.x * powerRoll, 0f);
            TimeRoll -= Time.deltaTime;
            if (TimeRoll <= 0)
            {
                CanRoll = false;
                capsul.enabled = true;
                rig.gravityScale = 3;
                TimeRoll = TimeRollCollDown;
                TimeCanRoll = 0;
            }
        }
        else if (!CanRoll && TimeCanRoll < 1.53f)
        {
            TimeCanRoll += Time.deltaTime;
        }
    }
    public void Jump()
    {
        rig.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        isground = false;
    }
    public void Flip()
    {
        if (horizontal > 0 && !skill)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0 && !skill)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void AnimPlayer()
    {
        anim.SetFloat("run", Mathf.Abs(horizontal));
        anim.SetFloat("jump", vertical);
        anim.SetBool("isground", isground);
        anim.SetBool("roll", CanRoll);
        anim.SetBool("skill", skill);
        anim.SetBool("air attack", air_attack);
        anim.SetBool("defend", defend);
    }
    public void PhongThu()
    {
        if (Input.GetKey(KeyCode.K))
        {
            defend = true;
        }
        else
        {
            defend = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "san")
        {
            isground = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (!defend)
            {
                hitdame = true;
                hpPlayer.TakeDamage(20);
            }
        }
        if (collision.gameObject.tag == "BulletFire")
        {
            if (!defend)
            {
                hitdame = true;
                hpPlayer.TakeDamage(30);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy att")
        {
            if (!defend)
            {
                hpPlayer.TakeDamage(40);
                hitdame = true;
            }
        }
        if (collision.gameObject.tag == "hb boss")
        {
            if (!defend)
            {
                Debug.Log("aa");
                hpPlayer.TakeDamage(70);
                hitdame = true;
            }
        }
        if (collision.gameObject.tag == "Checkpl")
        {
            this.PostEvent(EventID.SpawnBoss);
            this.PostEvent(EventID.SwichCamera2);
            StopPlayer = true;
            horizontal = 0;
        }
        if (collision.gameObject.tag == "WinGame")
        {
            this.PostEvent(EventID.QuaMan);
        }
    }
    private void StopHit()
    {
        hitdame = false;
        stopcombo = false;
    }
    private void ResetCombo()
    {
        comboTempo = -1;
        stopcombo = false;
    }
    private void ResetSkill()
    {
        skill = false;
    }
    private void ResetAirAttack()
    {
        air_attack = false;
    }
    private void CoolDownAirAtt()
    {
        this.PostEvent(EventID.AirAttack);
    }
    private void CoolDownSkill()
    {
        this.PostEvent(EventID.Skill);
    }
    private void Death()
    {
        this.PostEvent(EventID.PlayerDie);
    }
}
