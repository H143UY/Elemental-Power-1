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
        if (CurrentMana <= 0)
        {
            CurrentMana = 0;
        }
    }
    public void TruMana(float dame)
    {
        CurrentMana -= dame;
        SetIndex(CurrentMana);
    }
}
