using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaiSauController : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    private bool attack;
    private float attackTime;
    private bool hit;
    public GameObject bullet;
    public Transform posShoot;
    void Start()
    {
        anim =  GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        attackTime = 0;
        attack = false;
    }

    void Update()
    {
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
        if (attackTime > 2)
        {
            attack = true;
        }
    }
    void Shoot()
    {
        Instantiate(bullet,posShoot.position, Quaternion.identity); 
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
}
