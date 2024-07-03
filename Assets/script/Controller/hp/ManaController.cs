using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : IndexController
{
    public float MaxMana;
    public float CurrentMana;
    void Start()
    {
        SetMaxIndex(MaxMana);
        CurrentMana = MaxMana;
    }

    void Update()
    {
        SetIndex(CurrentMana);
        if (CurrentMana <= 0)
        {
            CurrentMana = 0;
        }
        if(CurrentMana >= MaxMana)
        {
            CurrentMana = MaxMana;
        }
    }
    public bool TruMana(float mana)
    {
        if (CurrentMana > mana)
        {
            CurrentMana -= mana;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CongMana(float amount)
    {
        CurrentMana += amount;
    }
}
