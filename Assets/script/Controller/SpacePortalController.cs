using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePortalController : MonoBehaviour
{
    public bool Idle;
    private Animator animator;
    [Header("sinh quai")]
    public float TimeStop;
    void Start()
    {
        Idle = false;
        animator = GetComponent<Animator>();
        TimeStop = 0f;
    }

    void Update()
    {
        Anim();
        SpawnEnmey();
    }

    void SpawnEnmey()
    {
        if (Idle)
        {
            TimeStop += Time.deltaTime;
            if (TimeStop >= 25f)
            {
                Idle = false;
                animator.SetTrigger("end");
            }
        }
    }

    void Anim()
    {
        animator.SetBool("idle", Idle);
    }

    private void ChangeStart()
    {
        Idle = true;
    }

    private void ChangeEnd()
    {
        SmartPool.Instance.Despawn(this.gameObject);
        this.PostEvent(EventID.EndSpace);
    }
}
