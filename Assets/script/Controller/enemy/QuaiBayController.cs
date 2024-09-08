using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaiBayController : ObjectController
{
    private HpEnemyController hpEnemyController;
    private Vector3 dir;
    private Animator animator;
    [Header(" patrolling")]
    public bool fly;
    [Header(" bi tan cong")]
    private bool hit = false;
    [Header(" attack")]
    public bool CheckAttack;
    public bool IsAttack;
    private float TimeToAttack;
    public Transform PosAttack;
    public float distance;
    public LayerMask LayerEnem;
    private GameObject player;
    private void Awake()
    {
    }
    void Start()
    {
        hpEnemyController = GetComponent<HpEnemyController>();
        CheckAttack = false;
        IsAttack = false;
        hit = false;
        fly = true;
        animator = GetComponent<Animator>();
        dir = new Vector3(-1, 0, 0);
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if(hpEnemyController != null)
        {
            if(hpEnemyController.CurrentHp <= 0)
            {
                SmartPool.Instance.Despawn(this.gameObject);
            }
        }
        if (fly)
        {
            Move(dir);
        }
        Flip();
        Check();
        AnimEnemy();
        if(hit)
        {
            fly = false;
        }
    }
    private void Flip()
    {
        if (dir.x > 0)
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);

        }
        else if (dir.x < 0)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void Check()
    {
        if (!hit)
        {
            CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distance, LayerEnem);
        }
        if (CheckAttack && !hit)
        {
            fly = false;
        }
        else if (!CheckAttack && !IsAttack)
        {
            fly = true;
        }

        if (IsAttack == true)
        {
            animator.SetTrigger("is attack");
            fly = false;
            CheckAttack = false;
        }
        else
        {
            TimeToAttack += Time.deltaTime;
            if (TimeToAttack > 1f)
            {
                if (CheckAttack && !hit)
                {
                    IsAttack = true;
                    TimeToAttack = 0;
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
        animator.SetBool("fly", fly);
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
        if (collision.gameObject.tag == "DiemA")
        {
            if (!IsAttack)
            {
                dir = new Vector3(1, 0, 0);

            }
        }
        if (collision.gameObject.tag == "DiemB")
        {
            if (!IsAttack)
            {
                dir = new Vector3(-1, 0, 0);
            }
        }
        if (collision.gameObject.tag == "HB skill")
        {
            hpEnemyController.TakeDamage(1000);
            if (!hit)
            {
                hit = true;
            }
        }
        if (collision.gameObject.tag == "HB air att")
        {
            hpEnemyController.TakeDamage(300);
            if (!hit)
            {
                hit = true;
            }
        }
    }
    private void StopAttack()
    {
        IsAttack = false;
    }
    private void Hitfalse()
    {
        hit = false;
    }
}

