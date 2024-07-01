using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : ObjectController
{
    public static PlayerController Instance;
    [Header("di chuyển")]
    private float horizontal;
    private float vertical;
    public float JumpForce;
    private Vector2 Direction;
    private Animator anim;
    private Rigidbody2D rig;
    public bool isground;
    [Header("thien")]
    public bool mediate = false;
    public bool loopMediate = false;

    [Header("tan cong")]
    public int combo = 1;
    public bool attacking;
    public float comboTiming;
    private float comboTempo;
    private int comboNumber = 3;
    private bool stopcombo = false;

    public bool hitdame = false;

    [Header("defend")]
    public bool defend = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        this.RegisterListener(EventID.playerhit_dame, (sender, param) =>
        {
            hitdame = true;
        });
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        comboTiming = 0.6f;
        comboTempo = comboTiming;
    }

    void Update()
    {
        DiChuyen();
        vertical = rig.velocity.y;

        Thien();
        Flip();
        AnimPlayer();
        ComboAttack();
        PhongThu();
    }
    private void DiChuyen()
    {
        if (!hitdame)
        {
            horizontal = 0;
            if (Input.GetKeyDown(KeyCode.W) && isground)
            {
                Jump();
            }
            horizontal = Input.GetAxis("Horizontal");
            Direction = new Vector3(horizontal, 0);
        }
        else
        {
            Direction = new Vector3(0, 0);
        }
        Move(Direction);
    }
    private void Thien()
    {
        if (Input.GetKey(KeyCode.L))
        {
            mediate = true;
        }
        else
        {
            mediate = false;
            loopMediate = false;
        }
    }
    public void ComboAttack()
    {
        comboTempo -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.J) && comboTempo < 0 && !stopcombo)
        {
            attacking = true;
            anim.SetTrigger("attack" + combo);
            comboTempo = comboTiming;
        }
        else if (Input.GetKeyDown(KeyCode.J) && comboTempo > 0 && comboTempo < 0.5f && !stopcombo)
        {
            attacking = true;
            combo++;
            if (combo >= comboNumber)
            {
                stopcombo = true;
                combo = 3;
            }
            anim.SetTrigger("attack" + combo);
            comboTempo = comboTiming;
        }
        else if (comboTempo < 0 && !Input.GetKeyDown(KeyCode.J))
        {
            attacking = false;
        }
        if (comboTempo < 0)
        {
            combo = 1;
        }
    }
    public void Jump()
    {
        rig.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        isground = false;
    }
    public void Flip()
    {
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void AnimPlayer()
    {
        anim.SetFloat("run", Mathf.Abs(horizontal));
        anim.SetFloat("jump", vertical);
        anim.SetBool("isground", isground);
        anim.SetBool("mediate", mediate);
        anim.SetBool("loop mediate", loopMediate);
        anim.SetBool("defend", defend);
        if (hitdame == true)
        {
            anim.SetTrigger("hit");
        }
    }
    public void PhongThu()
    {
        if (Input.GetKey(KeyCode.K))
        {
            defend = true;
        }
        else
        {
            defend = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "san")
        {
            isground = true;
        }
    }
    private void MediateLoop()
    {
        loopMediate = true;
    }
    private void StopHit()
    {
        hitdame = false;
    }
    private void ResetCombo()
    {
        combo = 1;
        stopcombo = false;
    }

}
