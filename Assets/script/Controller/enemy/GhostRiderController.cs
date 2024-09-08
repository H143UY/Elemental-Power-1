using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRiderController : ObjectController
{
    private Vector3 Dir;
    private bool Boom;
    private Animator Animator;
    private float TimeToBoom;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    void Start()
    {
        Dir = new Vector3(Mathf.Sign(player.transform.position.x - this.gameObject.transform.position.x), 0, 0);
        speed = 11;
        Animator = GetComponent<Animator>();
        Boom = false;
        TimeToBoom = 0f;
        FLip();
    }
    void Update()
    {
        Move(Dir);
        if (Boom)
        {
            Dir = new Vector3(0, 0, 0);
        }
        No();
        Anime();
    }
    void Anime()
    {
        Animator.SetBool("Boom", Boom);
    }
    void FLip()
    {
        if (player.transform.position.x - this.gameObject.transform.position.x > 0)
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void No()
    {
        TimeToBoom += Time.deltaTime;
        if (TimeToBoom > 6f)
        {
            Boom = true;
            TimeToBoom = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Boom = true;
        }
    }
    void EndBoom()
    {
        Destroy(this.gameObject);
    }
}
