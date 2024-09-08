using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownSkillController : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField]
    private Image imageSkillCooldown;
    [SerializeField]
    private TMP_Text textSkillCooldown;
    private bool isSkillCoolDown;
    private float coolDownSkillTime;
    private float coolDownSkillTimer = 0f;
    public float TimeSkill;
    private void Awake()
    {
        this.RegisterListener(EventID.Skill, (sender, param) =>
        {
            UseSkillSpecial();
        });
    }
    void Start()
    {
        coolDownSkillTime = coolDownSkillTimer;
        isSkillCoolDown = false;
        textSkillCooldown.gameObject.SetActive(false);
        imageSkillCooldown.fillAmount = 0.0f;
    }
    void Update()
    {
        if (isSkillCoolDown)
        {
            ApplySkillCoolDown();
        }
        GetSelectedPlayerTimeAirAttack();
    }
    private void ApplySkillCoolDown()
    {
        coolDownSkillTime -= Time.deltaTime;
        if (coolDownSkillTime < 0.0f)
        {
            this.PostEvent(EventID.CanSkill);
            isSkillCoolDown = false;
            textSkillCooldown.gameObject.SetActive(false);
            imageSkillCooldown.fillAmount = 0.0f;
        }
        else
        {
            textSkillCooldown.text = Mathf.RoundToInt(coolDownSkillTime).ToString();
            imageSkillCooldown.fillAmount = coolDownSkillTime / coolDownSkillTimer;
        }
    }
    public void UseSkillSpecial()
    {
        if (isSkillCoolDown)
        {
            return;
        }
        else
        {
            isSkillCoolDown = true;
            coolDownSkillTime = TimeSkill;
            textSkillCooldown.gameObject.SetActive(true);
        }
    }
    public void GetSelectedPlayerTimeAirAttack()
    {
        if (CharacterManager.Instance.selectedCharacterIndex == 0)
        {
            if (PlayerEarthController.Instance != null)
            {
                TimeSkill = PlayerEarthController.Instance.TimeToSkill;
            }
            else
            {
                Debug.LogError("PlayerEarthController.Instance is null!"); 
            }
        }
        else if (CharacterManager.Instance.selectedCharacterIndex == 1)
        {
            if (PlayerDiamondController.Instance != null)
            {
                TimeSkill = PlayerDiamondController.Instance.TimeToSkill;
            }
            else
            {
                Debug.LogError("PlayerDiamondController.Instance is null!");
            }
        }
        else if (CharacterManager.Instance.selectedCharacterIndex == 2)
        {
            if (PlayerFireController.Instance != null)
            {
                TimeSkill = PlayerFireController.Instance.TimeToSkill;
            }
            else
            {
                Debug.LogError("PlayerFireController.Instance is null!");
            }
        }
    }
}
