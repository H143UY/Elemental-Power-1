using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : ObjectController
{
    private HpPlayerController hpPlayer;
    private CapsuleCollider2D capsul;
    public static PlayerController Instance;
    public ManaController mana;
    [Header("Chỉ số người chơi")]
    public float hand_damage;
    public float skill_damage;
    public float air_damage;
    public float JumpForce;

    [Header("di chuyển")]
    private float horizontal;
    private float vertical;
    private Vector2 Direction;
    private Animator anim;
    private Rigidbody2D rig;
    public bool isground;
    [Header("thien")]
    public bool mediate = false;
    public bool loopMediate = false;
    private float plustimemana;
    [Header("Roll")]
    public bool CanRoll = false;
    private float TimeRollCollDown;
    public float TimeRoll;
    public float TimeCanRoll;
    public float powerRoll = 3;
    [Header("tan cong")]
    public int combo = 1;
    public bool attacking;
    public float comboTiming;
    private float comboTempo;
    private int comboNumber = 3;
    private bool stopcombo = false;
    public bool hitdame = false;

    [Header("Skill")]
    public bool skill = false;
    public bool air_attack = false;
    [Header("defend")]
    public bool defend = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        this.RegisterListener(EventID.playerhit_dame, (sender, param) =>
        {
            hitdame = true;
        });
    }
    void Start()
    {
        capsul = GetComponent<CapsuleCollider2D>();
        hpPlayer = GetComponent<HpPlayerController>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        mana = GetComponent<ManaController>();
        comboTiming = 0.6f;
        comboTempo = comboTiming;
        TimeRollCollDown = 1f;
        TimeRoll = TimeRollCollDown;
        plustimemana = 0;
    }

    void Update()
    {
        DiChuyen();
        vertical = rig.velocity.y;
        Roll();
        Thien();
        Flip();
        AnimPlayer();
        ComboAttack();
        PhongThu();
        Skill();
        AirAttack();
    }
    private void DiChuyen()
    {
        if (!hitdame && !skill)
        {
            horizontal = 0;
            if (Input.GetKeyDown(KeyCode.W) && isground)
            {
                Jump();
            }
            horizontal = Input.GetAxis("Horizontal");
            Direction = new Vector3(horizontal, 0);
        }
        else
        {
            Direction = new Vector3(0, 0);
        }
        Move(Direction);
    }
    private void Thien()
    {
        if (Input.GetKey(KeyCode.L))
        {
            mediate = true;
            plustimemana += Time.deltaTime;
            if(plustimemana > 1f)
            {
                mana.CongMana(10);
                plustimemana = 0;
            }
        }
        else
        {
            mediate = false;
            loopMediate = false;
            plustimemana = 0;
        }

    }
    public void AirAttack()
    {
        if (Input.GetKeyDown(KeyCode.U) && !air_attack)
        {
            if (mana != null && mana.TruMana(30))
            {
                air_attack = true;
            }
        }
    }
    public void Skill()
    {
        if (Input.GetKeyDown(KeyCode.I) && !skill && isground)
        {
            if (mana != null && mana.TruMana(70))
            {
                skill = true;
            }
        }
    }
    public void ComboAttack()
    {
        comboTempo -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.J) && comboTempo < 0 && !stopcombo)
        {
            attacking = true;
            anim.SetTrigger("attack" + combo);
            comboTempo = comboTiming;
        }
        else if (Input.GetKeyDown(KeyCode.J) && comboTempo > 0 && comboTempo < 0.5f && !stopcombo)
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
        }
        if (comboTempo < 0)
        {
            combo = 1;
        }
    }
    public void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        anim.SetBool("mediate", mediate);
        anim.SetBool("loop mediate", loopMediate);
        anim.SetBool("defend", defend);
        if (hitdame == true)
        {
            anim.SetTrigger("hit");
        }
        anim.SetBool("roll", CanRoll);
        anim.SetBool("skill", skill);
        anim.SetBool("air attack", air_attack);
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
                hpPlayer.TakeDamage(20);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy att")
        {
            if(!defend || !CanRoll)
            {
                hpPlayer.TakeDamage(40);
            }
        }
    }
    private void MediateLoop()
    {
        loopMediate = true;
    }
    private void StopHit()
    {
        hitdame = false;
    }
    private void ResetCombo()
    {
        combo = 1;
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
}
