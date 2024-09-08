using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OctobusBossController : ObjectController
{

    private bool StopOcTobus;
    private HpEnemyController hpOcToBus;
    private Rigidbody2D rig;
    private CapsuleCollider2D capsul;
    private Animator animator;
    [Header("StartGame")]
    public GameObject StopPos;
    private bool StartGame;
    [Header("fly")]
    public bool Fly;
    [Header("Move")]
    private Vector3 Dir;
    private GameObject player;
    public bool Walk;
    private float SpeedToStopPos;
    [Header("tan cong")]
    private int Combo;
    public float distanceAttack;
    public Transform PosAttack;
    public LayerMask LayerEnem;
    public bool attacking = false;
    public bool CheckAttack = false;
    public float TimeToAttack;
    public float TimeAttackCoolDown;
    [Header("hit dame")]
    public bool hit;
    [Header("Giai doan 2")]
    private bool ChuyenCap2;
    private bool Cap2;
    public Transform PosFly2;
    public bool GoiQuai;
    public bool EndChange2;
    private void Awake()
    {
        this.RegisterListener(EventID.SwichCamera2, (sender, param) =>
        {
            StopOcTobus = false;
        });
        this.RegisterListener(EventID.SwichCamera3, (sender, param) =>
        {
            StartGame = true;
            rig.gravityScale = 3;
            Fly = false;
            Walk = true;
        });
        this.RegisterListener(EventID.EndSpace, (sender, param) =>
        {
            GoiQuai = false;
            Fly = true;
            capsul.enabled = true;
            ChuyenCap2 = false;
            EndChange2 = true;
            SpeedToStopPos = 7f;
        });
    }
    void Start()
    {
        capsul = GetComponent<CapsuleCollider2D>();
        hpOcToBus = GetComponent<HpEnemyController>();
        //tan cong
        Combo = 1;
        attacking = false;
        TimeToAttack = 5;
        TimeAttackCoolDown = 2.5f;
        hit = false;
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        StartGame = false;
        Fly = true;
        player = GameObject.FindWithTag("Player");
        Walk = false;
        StopOcTobus = true;
        //giai doan 2
        SpeedToStopPos = 13f;
        ChuyenCap2 = false;
        Cap2 = false;
        GoiQuai = false;
        EndChange2 = false;
    }
    void Update()
    {
        if (StopOcTobus)
        {
            return;
        }
        if (ChuyenCap2)
        {
            AnimeOctobus();
            Number2();
            return;
        }
        if (hpOcToBus.CurrentHp <= 0)
        {
            animator.SetTrigger("Die");
            return;
        }
        if (!StartGame || EndChange2)
        {
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, StopPos.transform.position, SpeedToStopPos * Time.deltaTime);
        }
        else
        {
            Move(Dir);
        }
        DiChuyen();
        AnimeOctobus();
        Attack();
        FLip();
        Number2();
    }
    private void DiChuyen()
    {
        if (Walk)
        {
            Dir = new Vector3(Mathf.Sign(player.transform.position.x - this.gameObject.transform.position.x), 0, 0);
        }
        else
        {
            Dir = Vector3.zero;
        }
    }
    void FLip()
    {
        if (player.transform.position.x - this.gameObject.transform.position.x > 0)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private void Attack()
    {
        CheckAttack = Physics2D.OverlapCircle(PosAttack.position, distanceAttack, LayerEnem);
        if (CheckAttack || attacking)
        {
            Walk = false;
        }
        else if (!CheckAttack && !attacking)
        {
            Walk = true;
        }
        if (attacking == true)
        {
            hit = false;
            animator.SetTrigger("atk" + Combo);
            //CheckAttack = false;
        }
        else
        {
            TimeToAttack += Time.deltaTime;
            if (TimeToAttack > TimeAttackCoolDown)
            {
                if (CheckAttack)
                {
                    attacking = true;
                }

            }
        }
    }
    void Number2()
    {
        if (!Cap2)
        {
            if (hpOcToBus.CurrentHp <= hpOcToBus.MaxHp / 3)
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);
                rig.gravityScale = 0;
                capsul.enabled = false;
                Fly = true;
                Walk = false;
                ChuyenCap2 = true;
                this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, PosFly2.transform.position, 13f * Time.deltaTime);
                if (this.gameObject.transform.position == PosFly2.transform.position)
                {
                    this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
                    GoiQuai = true;
                    Fly = false;
                }
            }
            if (GoiQuai)
            {
                hpOcToBus.CurrentHp += 90 * Time.deltaTime;
            }
            if (ChuyenCap2)
            {
                Walk = false;
                attacking = false;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(PosAttack.position, distanceAttack);
    }
    private void AnimeOctobus()
    {
        animator.SetBool("fly", Fly);
        animator.SetBool("walk", Walk);
        animator.SetBool("attaking", attacking);
        animator.SetBool("hit", hit);
        animator.SetBool("GoiQuai", GoiQuai);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "StopBoss")
        {
            if (!StartGame)
            {
                this.PostEvent(EventID.StartDialogue);
            }
            if (EndChange2)
            {
                EndChange2 = false;
                Fly = false;
                Walk = true;
                rig.gravityScale = 3;
                Cap2 = true;
            }
        }

        if (collision.gameObject.tag == "player att")
        {
            if (!attacking)
            {
                hpOcToBus.TakeDamage(50);
            }
            if (!hit)
            {
                hit = true;
            }
        }
        if (collision.gameObject.tag == "HB skill")
        {
            if (!hit)
            {
                hit = true;
            }
            if (!attacking)
            {
                hpOcToBus.TakeDamage(1000);
            }
        }
        if (collision.gameObject.tag == "HB air att")
        {
            if (!hit)
            {
                hit = true;
            }
            if (!attacking)
            {
                hpOcToBus.TakeDamage(500);
            }
        }
    }
    private void UpCombo()
    {
        if (Cap2)
        {
            Combo = 2;
        }
        else
        {
            TimeToAttack = 0;
            attacking = false;
        }
    }
    private void ResetCombo()
    {
        Combo = 1;
        TimeToAttack = 0;
        attacking = false;
    }
    private void EndHit()
    {
        hit = false;
    }
    private void SpawnSpace()
    {
        this.PostEvent(EventID.SpawnSpace);
    }
    private void EndGame()
    {
        SceneManager.LoadScene(3);
    }
}
