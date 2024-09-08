using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyXuongController : ObjectController
{
    private HpEnemyController hpEnemyController;
    private Vector3 dir;
    private Animator animator;
    private Rigidbody2D rig;
    [Header(" patrolling")]
    public bool run;
    [Header("Attack Player")]
    public bool attackPlayer = false;
    // phong thu
    public bool Shield = false;
    public float TimeShield;
    private bool DuocPhongThu = true;
    private bool hit = false;
    public float TimeCanShield;
    private GameObject player;
    private void Awake()
    {
        this.RegisterListener(EventID.FindPlayer, (sender, param) =>
        {
            player = GameObject.FindWithTag("Player");
        });
    }
    private void Start()
    {
        run = true;
        hpEnemyController = GetComponent<HpEnemyController>();
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        dir = new Vector3(1, 0, 0);
        TimeCanShield = 0;
    }
    private void Update()
    {
        if (run)
        {
            Move(dir);
        }
        AnimEnemy();
        Flip();
        PhongThu();
        if (attackPlayer)
        {
            run = false;
        }
        else if (!attackPlayer && !Shield && !hit)
        {
            run = true;
        }
        if (hpEnemyController.CurrentHp <= 0)
        {
            SmartPool.Instance.Despawn(this.gameObject);
        }
    }
    private void PhongThu()
    {
        if (Shield)
        {
            TimeShield += Time.deltaTime;
            if (TimeShield > 3.5f)
            {
                Shield = false;
                attackPlayer = true;
                TimeShield = 0;
            }
        }
        if (!DuocPhongThu)
        {
            TimeCanShield += Time.deltaTime;
            if (TimeCanShield > 7.5f)
            {
                DuocPhongThu = true;
                TimeCanShield = 0;
            }
        }
    }

    private void Flip()
    {
        if (Shield || attackPlayer)
        {
            if (player.transform.position.x > this.gameObject.transform.position.x)
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            if (dir.x > 0)
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);

            }
            else if (dir.x < 0)
            {
                this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

    }
    void AnimEnemy()
    {
        animator.SetFloat("run", Mathf.Abs(dir.x));
        animator.SetBool("shield", Shield);
        animator.SetBool("attack", attackPlayer);
        animator.SetBool("hit", hit);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player att")
        {
            if (DuocPhongThu)
            {
                Shield = true;
                run = false;
                DuocPhongThu = false;
                hit = false;
            }
            else
            {
                if (!hit && !Shield)
                {
                    hit = true;
                    run = false;
                }
            }
            if (Shield || DuocPhongThu)
            {
                hpEnemyController.TakeDamage(1);
            }
            else
            {
                hpEnemyController.TakeDamage(35);
            }
        }
        if (collision.gameObject.tag == "DiemA")
        {
            dir = new Vector3(1, 0, 0);
        }
        if (collision.gameObject.tag == "DiemB")
        {
            dir = new Vector3(-1, 0, 0);
        }
        if (collision.gameObject.tag == "HB skill")
        {
            Shield = false;
            hpEnemyController.TakeDamage(1000);
            rig.AddForce(new Vector2(0, 1) * 10f, ForceMode2D.Impulse);
        }
        if (collision.gameObject.tag == "HB air att")
        {
            if (DuocPhongThu)
            {
                Shield = true;
                run = false;
                DuocPhongThu = false;
                hit = false;
            }
            else
            {
                if (!hit && !Shield)
                {
                    hit = true;
                    run = false;
                }
            }
            if (Shield || DuocPhongThu)
            {
                hpEnemyController.TakeDamage(5);
            }
            else
            {
                hpEnemyController.TakeDamage(300);
                rig.velocity = new Vector2(player.transform.localScale.x * 3, 3);
            }
        }
    }
    private void StopAttack()
    {
        attackPlayer = false;
        run = true;
    }
    private void Hitfalse()
    {
        hit = false;
        run = true;
    }
    private void Die()
    {
        SmartPool.Instance.Despawn(this.gameObject);
    }
}
