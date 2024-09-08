using Core.Pool;
using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeController : ObjectController
{
    private Animator anim;
    private HpEnemyController hp;
    private Rigidbody2D rig;
    public ShakeData explosionShakeData;
    public bool StartBoss;
    private float TimeStartEnd;
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
    public float TimeAttackCoolDown;

    [Header("Chuyển trạng thái")]
    public bool isPhase2 = false;
    public bool chuyenhoa = false;
    public float TimeChange;
    private Vector3 initScale;
    public Vector3 targetScale = new Vector3(1.4f, 1.4f, 1f);
    public string NameChange = "Chuyen hoa";

    public bool hit;
    private float giap;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hp = GetComponent<HpEnemyController>();
        run = true;
        SaveSpeed = speed;
        TimeToAttack = 5;
        hit = false;
        TimeAttackCoolDown = 1.7f;
        initScale = transform.localScale;
        StartBoss = false;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == NameChange)
            {
                TimeChange = clip.length;
                break;
            }
        }
    }

    void Update()
    {
        if (StartBoss)
        {
            AnimeBoss();
            TimeStartEnd += Time.deltaTime;
            if (TimeStartEnd >= 2f)
            {
                this.PostEvent(EventID.SwichCamera1);
                StartBoss = false;
                this.PostEvent(EventID.SinhQuaiXong);
            }
            return;
        }
        if (hp.CurrentHp <= 0)
        {
            anim.SetTrigger("death");
        }
        distance = player.transform.position.x - this.gameObject.transform.position.x;
        dir = new Vector3(Mathf.Sign(distance), 0, 0);
        if (run && !CheckAttack)
        {
            Move(dir);
        }
        Flip();
        TanCong();
        AnimeBoss();
        change();
        CheckPhaseTransition();
    }

    private void AnimeBoss()
    {
        anim.SetBool("run", run);
        anim.SetBool("can attack", attacking);
        anim.SetBool("hit", hit);
    }

    private void Flip()
    {
        if (!isPhase2)
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
        else
        {
            if (dir.x < 0)
            {
                this.gameObject.transform.localScale = new Vector3(1.4f, 1.4f, 1);
            }
            else
            {
                this.gameObject.transform.localScale = new Vector3(-1.4f, 1.4f, 1);
            }
        }
    }

    private void TanCong()
    {
        CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distanceAttack, LayerEnem);
        if (CheckAttack)
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
            if (TimeToAttack > TimeAttackCoolDown)
            {
                if (CheckAttack && !chuyenhoa)
                {
                    attacking = true;
                    TimeToAttack = 0;
                }

            }
        }
    }
    private void change()
    {
        if (hp.CurrentHp <= hp.MaxHp * 0.4f && !isPhase2)
        {
            chuyenhoa = true;
            anim.SetTrigger("chuyen hoa");
            StartCoroutine(ScaleOverTime(TimeChange));
        }
        if (!isPhase2)
        {
            giap = 0;
        }
        else
        {
            giap = 20;
        }
    }

    private void CheckPhaseTransition()
    {
        if (isPhase2)
        {
            int phase2LayerIndex = anim.GetLayerIndex("Pharse 2");
            anim.SetLayerWeight(phase2LayerIndex, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(PosAttack.position, distanceAttack);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StopBoss")
        {
            if (!StartBoss)
            {
                run = false;
                StartBoss = true;
            }
        }
        if (collision.gameObject.tag == "player att" && !chuyenhoa)
        {
            hp.TakeDamage((35 - giap));
            if (!hit)
            {
                hit = true;
            }
        }
        if (collision.gameObject.tag == "HB skill" && !chuyenhoa)
        {
            if (!hit)
            {
                hit = true;
            }
            hp.TakeDamage((300 - giap));
        }
        if (collision.gameObject.tag == "HB air att" && !chuyenhoa)
        {
            if (!hit)
            {
                hit = true;
            }
            rig.velocity = new Vector2(Mathf.Sign(player.transform.position.x - gameObject.transform.position.x) * -8, 4);
            hp.TakeDamage((150 - giap));
        }
    }
    private IEnumerator ScaleOverTime(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(initScale, targetScale, t);
            yield return null;
        }
        transform.localScale = targetScale;
    }
    private void StopAttack()
    {
        TimeToAttack = 0;
        attacking = false;
    }

    private void EndHit()
    {
        hit = false;
    }
    private void Die()
    {
        SmartPool.Instance.Despawn(this.gameObject);
    }
    private void ChuyenTrangThai()
    {
        isPhase2 = true;
        TimeToAttack = 0.8f;
        speed = 5.5f;
        chuyenhoa = false;
    }
    private void Shakecamera()
    {
        CameraShakerHandler.Shake(explosionShakeData);
    }
}
