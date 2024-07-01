using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordmonsterController : ObjectController
{
    private Vector3 dir;
    private Animator animator;
    [Header(" patrolling")]
    public bool run;
    [Header(" bi tan cong")]
    public bool hurt;
    [Header(" attack")]
    public bool CheckAttack;
    public bool IsAttack;
    private float TimeToAttack;
    public Transform PosAttack;
    public float distance;
    public LayerMask LayerEnem;
    private void Awake()
    {
        this.RegisterListener(EventID.hit_dame, (sender, param) =>
        {
            hurt = true;
        });
    }
    void Start()
    {
        CheckAttack = false;
        IsAttack = false;
        hurt = false;
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
        CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distance, LayerEnem);
        if (CheckAttack)
        {
            IsAttack = true;
        }
        else if (!CheckAttack && !IsAttack)
        {
            run = true;
        }

        if (IsAttack == true)
        {
            animator.SetTrigger("is attack");
            run = false;
        }
        else
        {
            TimeToAttack += Time.deltaTime;
            if (TimeToAttack > 1f)
            {
                if (CheckAttack)
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
    }
    private void StopAttack()
    {
        IsAttack = false;
    }
}
