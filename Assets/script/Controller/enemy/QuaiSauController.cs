using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaiSauController : MonoBehaviour
{
    private HpEnemyController hpEnemyController;
    private Animator anim;
    private GameObject player;
    private bool attack;
    public float attackTime;
    private bool hit;
    public GameObject bullet;
    public Transform posShoot;
    private bool die;
    private void Awake()
    {
        this.RegisterListener(EventID.FindPlayer, (sender, param) =>
        {
            player = GameObject.FindWithTag("Player");
        });
    }
    void Start()
    {
        die = false;
        hpEnemyController = GetComponent<HpEnemyController>();
        anim = GetComponent<Animator>();
        attackTime = 0;
        attack = false;
    }

    void Update()
    {
        if (hpEnemyController.CurrentHp <= 0)
        {
            die = true;
        }
        if (die)
        {
            anim.SetTrigger("die");
        }
        Flip();
        Anim();
        Att();
    }
    void Flip()
    {
        float direc = this.gameObject.transform.position.x - player.transform.position.x;
        if (direc > 0)
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void Anim()
    {
        anim.SetBool("att", attack);
        anim.SetBool("hit", hit);
    }
    void Att()
    {
        if (!attack)
        {
            attackTime += Time.deltaTime;
        }
        if (attackTime > 1.85f)
        {
            if (!hit && !die)
            {
                attack = true;
            }
        }
    }
    void Shoot()
    {
        Instantiate(bullet, posShoot.position, Quaternion.identity);
    }
    void StopAtt()
    {
        attack = false;
        attackTime = 0;
    }
    void stophit()
    {
        hit = false;
    }
    void Death()
    {
        SmartPool.Instance.Despawn(this.gameObject);
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
        if (collision.gameObject.tag == "HB skill")
        {
            if (!hit)
            {
                hit = true;
            }
            hpEnemyController.TakeDamage(1000);
        }
        if (collision.gameObject.tag == "HB air att")
        {
            if (!hit)
            {
                hit = true;
            }
            hpEnemyController.TakeDamage(300);
        }
    }
}
