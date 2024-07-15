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
    public void TakeDamage(float dame)
    {
        this.PostEvent(EventID.playerhit_dame);
        CurrentHp -= dame;
    }
}
