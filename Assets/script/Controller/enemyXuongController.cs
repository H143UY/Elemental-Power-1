using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyXuongController : ObjectController
{
    private Vector3 dir;
    private Animator animator;
    [Header(" patrolling")]
    public bool run;
    [Header("Attack Player")]
    public bool attackPlayer = false;
    public Transform check;
    public LayerMask layermask;
    public float distance ;
    public bool CanCheck = true;
    public float AttackCooldown;
    // phong thu
    public bool Shield = false;
    public float TimeShield;
    private bool DuocPhongThu = true;
    private void Awake()
    {
        this.RegisterListener(EventID.hit_dame, (sender, param) =>
        {
            if (DuocPhongThu)
            {
                Shield = true;
                run = false;
                DuocPhongThu = false;
            }
        });
    }
    private void Start()
    {
        run = true;
        animator = GetComponent<Animator>();
        dir = new Vector3(1, 0, 0);
    }
    private void Update()
    {
        if(run)
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
    }

    private void PhongThu()
    {
        if(Shield)
        {
            TimeShield += Time.deltaTime;
            if(TimeShield > 3.5f)
            {
                Shield = false;
                attackPlayer = true;
                run = false;
                TimeShield = 0;
            }
        }
    }
   
    private void Flip()
    {
        if(Shield || attackPlayer)
        {
            if(PlayerController.Instance.transform.position.x > this.gameObject.transform.position.x)
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
        animator.SetFloat("run",Mathf.Abs(dir.x));
        animator.SetBool("shield", Shield);
        animator.SetBool("attack", attackPlayer);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "")
        {
            run = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DiemA")
        {
            dir = new Vector3(1, 0, 0);
        }
        if (collision.gameObject.tag == "DiemB")
        {
            dir = new Vector3(-1, 0, 0);
        }
    }
    public void StopAttack()
    {
        attackPlayer = false;
        run = true;
        CanCheck = false;
    }
}
