using Core.Pool;
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
        SetIndex(CurrentHp);
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            this.PostEvent(EventID.die);
        }
    }
    public void TakeDamage(float dame)
    {
        if(dame >= MaxHp)
        {
            if(CurrentHp == MaxHp)
            {
                CurrentHp = 1;
            }
            else
            {
                CurrentHp -= dame;
            }
        }
        else
        {
            CurrentHp -= dame;
        }
    }
}
