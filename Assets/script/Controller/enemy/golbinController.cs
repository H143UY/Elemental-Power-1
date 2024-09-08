﻿using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinController : ObjectController
{
    private HpEnemyController hpEnemyController;
    private Vector3 dir;
    private CapsuleCollider2D capsul;
    private Animator animator;
    private Rigidbody2D rig;
    [Header(" patrolling")]
    public bool run;
    public bool followPlayer;
    public bool Stop;
    private float TimeToStop;
    [Header(" bi tan cong")]
    public bool hit = false;
    [Header(" attack")]
    public bool CheckAttack;
    public bool IsAttack;
    private float TimeToAttack;
    public Transform PosAttack;
    public float distance;
    public LayerMask LayerEnem;
    private bool die = false;
    private GameObject player;

    private void Awake()
    {
        this.RegisterListener(EventID.FindPlayer, (sender, param) =>
        {
            player = GameObject.FindWithTag("Player");
        });
    }

    void Start()
    {
        capsul = GetComponent<CapsuleCollider2D>();
        hpEnemyController = GetComponent<HpEnemyController>();
        rig = GetComponent<Rigidbody2D>();
        CheckAttack = false;
        IsAttack = false;
        hit = false;
        run = true;
        followPlayer = false;
        animator = GetComponent<Animator>();
        dir = new Vector3(-1, 0, 0);
        TimeToAttack = 10;
        Stop = false;
    }

    void Update()
    {
        // check die
        if (hpEnemyController.CurrentHp <= 0)
        {
            run = false;
            animator.SetTrigger("Die");
            capsul.enabled = false;
            rig.gravityScale = 0;
        }
        if (run && !Stop && !die)
        {
            Move(dir);
        }
        FolowPl();
        Flip();
        Check();
        AnimEnemy();
        if (hit)
        {
            run = false;
        }
        if (Stop)
        {
            TimeToStop += Time.deltaTime;
            if (TimeToStop > 1.5f)
            {
                dir = new Vector3(-dir.x, 0, 0);
                Stop = false;
                TimeToStop = 0;
                run = true;
            }
        }
    }

    public void FolowPl()
    {
        if (followPlayer)
        {
            speed = 5.5f;
            float dirPlayer = player.transform.position.x - this.gameObject.transform.position.x;
            if (dirPlayer == distance)
            {
                run = false;
            }
            else
            {
                run = true;
            }
            dir = new Vector3(Mathf.Sign(dirPlayer), 0, 0);
        }
    }

    private void Flip()
    {
        if (dir.x > 0 && !die)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dir.x < 0 && !die)
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Check()
    {
        if (!hit && followPlayer)
        {
            CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distance, LayerEnem);
        }
        // Kiểm tra tấn công
        if (IsAttack)
        {
            animator.SetTrigger("is attack");
            run = false;
        }
        else // Nếu chưa tấn công thì kiểm tra tấn công
        {
            TimeToAttack += Time.deltaTime;
            if (TimeToAttack > 1f)
            {
                TimeToAttack = 0;
                if (CheckAttack && !hit)
                {
                    IsAttack = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(PosAttack.position, distance);
    }

    void AnimEnemy()
    {
        animator.SetBool("run", run);
        animator.SetBool("can attack", IsAttack);
        animator.SetBool("hit", hit);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player att")
        {
            hpEnemyController.TakeDamage(35);
            if (!hit)
            {
                hit = true;
            }
        }
        if (collision.gameObject.tag == "DiemA" || collision.gameObject.tag == "DiemB")
        {
            if (!IsAttack && !followPlayer && !Stop)
            {
                run = false;
                Stop = true;
            }
        }
        if (collision.gameObject.tag == "HB skill")
        {
            if (!hit)
            {
                hit = true;
            }
            hpEnemyController.TakeDamage(1000);
            rig.AddForce(new Vector2(0, 1) * 10, ForceMode2D.Impulse);
        }
        if (collision.gameObject.tag == "HB air att")
        {
            if (!hit)
            {
                hit = true;
            }
            rig.velocity = new Vector2(player.transform.localScale.x * 3, 4);
            hpEnemyController.TakeDamage(300);
        }
    }

    private void StopAttack()
    {
        IsAttack = false;
        run = true;
    }

    private void Hitfalse()
    {
        hit = false;
        followPlayer = true; // bị đánh xong thì đuổi theo player
        run = true;
    }

    private void Die()
    {
        SmartPool.Instance.Despawn(this.gameObject);
    }
}