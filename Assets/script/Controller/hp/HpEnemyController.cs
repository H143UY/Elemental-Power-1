using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpEnemyController : IndexController
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player att")
        {
            TakeDamage(30);
        }
    }
    public void TakeDamage(float dame)
    {
        CurrentHp -= dame;
        SetIndex(CurrentHp);
    }
}
