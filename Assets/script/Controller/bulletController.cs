using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : BulletController
{
    private Animator anim;
    public float timeBoom;
    private bool Boom;
    private float direcFlip;
    public Vector3 dirtoPlayer;
    private void Awake()
    {
        this.RegisterListener(EventID.BoomBullet, (sender, param) =>
        {
            Boom = true;
        });
    }
    void Start()
    {
        Boom = false;
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindWithTag("Player");
        direcFlip = this.gameObject.transform.position.x - player.transform.position.x;
        dirtoPlayer = new Vector3(-Mathf.Sign(direcFlip), 0, 0);
        timeBoom = 0;
    }

    void Update()
    {
        BulletEx();
        Move(dirtoPlayer);
        Flip();
        if(Boom)
        {
            anim.Play("boom");
        }
    }
    void Flip()
    {
        if (direcFlip > 0)
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void Destroy()
    {
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != this.gameObject.tag)
        {
            Boom = true;
            //HpPlayerController hpPlayer = collision.gameObject.GetComponent<HpPlayerController>();
            //if (hpPlayer != null)
            //{
            //    hpPlayer.TakeDamage(40);
            //}
        }

    }
}
