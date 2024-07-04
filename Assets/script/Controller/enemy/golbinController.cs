using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golbinController : ObjectController
{
    private HpEnemyController hpEnemyController;
    private Vector3 dir;
    private Animator animator;
    private Rigidbody2D rig;
    [Header(" patrolling")]
    public bool run;
    public bool followPlayer;
    [Header(" bi tan cong")]
    public bool hit = false;
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
            if (!hit)
            {
                hit = true;
            }
        });
    }
    void Start()
    {
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
    }

    void Update()
    {
        if (run)
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
    }
    public void FolowPl()
    {
        if (followPlayer)
        {
            speed = 5.5f;
            float dirPlayer = PlayerController.Instance.transform.position.x - this.gameObject.transform.position.x;
            if(dirPlayer == distance)
            {
                Debug.Log("x");
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
        if (!hit && followPlayer)
        {
            CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distance, LayerEnem);
        }
        //Check đánh
        if (IsAttack == true)
        {
            animator.SetTrigger("is attack");
            run = false;
        }
        else // nếu chưa đánh thì chạy vào đây để check đánh
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player att")
        {
            hpEnemyController.TakeDamage(PlayerController.Instance.hand_damage);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DiemA")
        {
            if (!IsAttack && !followPlayer)
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
            hpEnemyController.TakeDamage(PlayerController.Instance.skill_damage);
            rig.AddForce(new Vector2(0, 1) * 10, ForceMode2D.Impulse);
        }
        if (collision.gameObject.tag == "HB air att")
        {
            hpEnemyController.TakeDamage(PlayerController.Instance.air_damage);
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
}

