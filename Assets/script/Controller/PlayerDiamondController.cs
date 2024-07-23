﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDiamondController : ObjectController
{
    private HpPlayerController hpPlayer;
    private CapsuleCollider2D capsul;
    public static PlayerDiamondController Instance;
    private ManaController mana;
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

    [Header("Skill")]
    public bool skill = false;
    public bool air_attack = false;
    [Header("defend")]
    public bool defend = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        capsul = GetComponent<CapsuleCollider2D>();
        hpPlayer = GetComponent<HpPlayerController>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        mana = GetComponent<ManaController>();
        comboTiming = 0.7f;
        comboTempo = comboTiming;
        TimeRollCollDown = 1f;
        TimeRoll = TimeRollCollDown;
        stopcombo = false;
    }

    void Update()
    {
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
        Move(Direction);
    }
    public void AirAttack()
    {
        if (Input.GetKeyDown(KeyCode.U) && !air_attack && !hitdame)
        {
            if (mana != null && mana.TruMana(30))
            {
                air_attack = true;
            }
        }
    }
    public void Skill()
    {
        if (Input.GetKeyDown(KeyCode.I) && !skill && isground && !hitdame)
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
        if (Input.GetKeyDown(KeyCode.J) && comboTempo < 0 && !stopcombo && !hitdame)
        {
            attacking = true;
            anim.SetTrigger("attack" + combo);
            comboTempo = comboTiming;
        }
        else if (Input.GetKeyDown(KeyCode.J) && comboTempo > 0 && comboTempo < 0.6f && !stopcombo && !hitdame)
        {
            attacking = true;
            if (!stopcombo)
            {
                combo++;
                stopcombo = true;
            }
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
        if (Input.GetKeyDown(KeyCode.Space) && !hitdame)
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
                hpPlayer.TakeDamage(70);
                hitdame = true;
            }
        }
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
    private void DoneDonDanh()
    {
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

