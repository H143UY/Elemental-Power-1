using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpPlayerController : IndexController
{
    public float MaxHp;
    public float CurrentHp;
    private void Awake()
    {
        if (slider == null)
        {
            slider = GameObject.Find("hp").GetComponent<Slider>();
            if (slider == null)
            {
                Debug.LogWarning("Slider 'health bar' not found!");
            }
        }
    }
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
