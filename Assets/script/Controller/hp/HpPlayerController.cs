using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPlayerController : IndexController
{
    public float MaxHp;
    public float CurrentHp;
    void Start()
    {
        SetMaxIndex(MaxHp);
        CurrentHp = MaxHp;
    }

    void Update()
    {
        SetIndex(CurrentHp);
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            TakeDamage(20);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy att")
        {
            TakeDamage(30);
        }
    }
    public void TakeDamage(float dame)
    {
        this.PostEvent(EventID.playerhit_dame);
        CurrentHp -= dame;
    }
}
