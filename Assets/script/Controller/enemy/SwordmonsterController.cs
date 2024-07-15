using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordmonsterController : ObjectController
{
    private HpEnemyController hpEnemyController;
    private BoxCollider2D boxcolider;
    private Vector3 dir;
    private Animator animator;
    private Rigidbody2D rig;
    [Header(" patrolling")]
    public bool run;
    [Header(" bi tan cong")]
    private bool hit = false;
    [Header(" attack")]
    public bool CheckAttack;
    public bool IsAttack;
    private float TimeToAttack;
    public Transform PosAttack;
    public float distance;
    public LayerMask LayerEnem;
    private void Awake()
    {
    }
    void Start()
    {
        boxcolider = GetComponent<BoxCollider2D>();
        hpEnemyController = GetComponent<HpEnemyController>();
        rig = GetComponent<Rigidbody2D>();
        CheckAttack = false;
        IsAttack = false;
        hit = false;
        run = true;
        animator = GetComponent<Animator>();
        dir = new Vector3(-1, 0, 0);
    }

    void Update()
    {
        if (run)
        {
            Move(dir);
        }
        Flip();
        Check();
        AnimEnemy();
        if (hit)
        {
            run = false;
        }
        if(hpEnemyController.CurrentHp <= 0)
        {
            animator.SetTrigger("die");
            CheckAttack = false;
            IsAttack = false;
            boxcolider.enabled = false;
            rig.gravityScale = 0;
        }
    }
    private void Flip()
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
    private void Check()
    {
        if (!hit)
        {
            CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distance, LayerEnem);
        }
        if (CheckAttack && !hit)
        {
            run = false;
        }
        else if (!CheckAttack && !IsAttack)
        {
            run = true;
        }

        if (IsAttack == true)
        {
            animator.SetTrigger("is attack");
            run = false;
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
        animator.SetBool("walk", run);
        animator.SetBool("can attack", IsAttack);
        animator.SetBool("hit", hit);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player att")
        {
            hpEnemyController.TakeDamage(PlayerController.Instance.hand_damage);
            if (!hit)
            {
                hit = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            if (!hit)
            {
                hit = true;
            }
            hpEnemyController.TakeDamage(PlayerController.Instance.skill_damage);
            rig.AddForce(new Vector2(0, 1) * 10, ForceMode2D.Impulse);
        }
        if (collision.gameObject.tag == "HB air att")
        {
            if (!hit)
            {
                hit = true;
            }
            rig.velocity = new Vector2(Mathf.Sign(PlayerController.Instance.transform.position.x - gameObject.transform.position.x) * -8, 4);
            hpEnemyController.TakeDamage(PlayerController.Instance.air_damage);
        }
    }
    private void StopAttack()
    {
        IsAttack = false;
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
