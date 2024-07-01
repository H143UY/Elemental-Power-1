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
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy att")
        {
            TakeDamage(30);
            this.PostEvent(EventID.playerhit_dame);
        }
    }
    public void TakeDamage(float dame)
    {
        CurrentHp -= dame;
        SetIndex(CurrentHp);
    }
}
