using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpEnemyController : MonoBehaviour
{
    public float MaxHp;
    public float CurrentHp;
    void Start()
    {
        //SetMaxIndex(MaxHp);
        CurrentHp = MaxHp;
    }

    void Update()
    {
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            SmartPool.Instance.Despawn(this.gameObject);
        }
    }
    public void TakeDamage(float dame)
    {
        this.PostEvent(EventID.hit_dame);
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
        //SetIndex(CurrentHp);
    }
}
