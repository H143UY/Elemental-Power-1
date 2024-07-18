using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeController : ObjectController
{
    private Animator anim;
    //run
    public bool run;
    private Vector3 dir;
    private float distance;
    private float SaveSpeed;
    [Header("tan cong")]
    public float distanceAttack;
    public Transform PosAttack;
    public LayerMask LayerEnem;
    public bool attacking = false;
    public bool CheckAttack = false;
    public float TimeToAttack;

    private bool hit;
    void Start()
    {
        anim = GetComponent<Animator>();
        run = true;
        SaveSpeed = speed;
        TimeToAttack = 5;
        hit = false;
    }

    void Update()
    {
        distance = PlayerController.Instance.transform.position.x - this.gameObject.transform.position.x;
        dir = new Vector3(Mathf.Sign(distance), 0, 0);
        if (run && !CheckAttack)
        {
            Move(dir);
        }
        Flip();
        TanCong();
        AnimeBoss();
    }

    private void AnimeBoss()
    {
        anim.SetBool("run", run);
        anim.SetBool("can attack", attacking);
    }

    private void Flip()
    {
        if (dir.x < 0)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void TanCong()
    {
        if (!hit)
        {
            CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distanceAttack, LayerEnem);
        }
        if (CheckAttack && !hit)
        {
            run = false;
        }
        else if (!CheckAttack && !attacking)
        {
            run = true;
        }

        if (attacking == true)
        {
            anim.SetTrigger("attack");
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
                    attacking = true;
                    TimeToAttack = 0;
                }

            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(PosAttack.position, distanceAttack);
    }
    private void StopAttack()
    {
        TimeToAttack = 0;
        attacking = false;
    }
}
