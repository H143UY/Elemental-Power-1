using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTimerController : MonoBehaviour
{
    [Header("air Attack")]
    [SerializeField]
    private Image imageAirCooldown;
    [SerializeField]
    private TMP_Text textAirCooldown;
    private bool isAirCoolDown;
    private float coolDownAirTime;
    private float coolDownAirTimer = 0f;
    public float TimeAirPlayer;
    private void Awake()
    {
        this.RegisterListener(EventID.AirAttack, (sender, param) =>
        {
            UseSkillAir();
        });
    }

    void Start()
    {
        coolDownAirTime = coolDownAirTimer;
        isAirCoolDown = false;
        textAirCooldown.gameObject.SetActive(false);
        imageAirCooldown.fillAmount = 0.0f;
    }

    void Update()
    {
        if (isAirCoolDown)
        {
            ApplyAirCoolDown();
        }
        GetSelectedPlayerTimeAirAttack();
    }
    private void ApplyAirCoolDown()
    {
        coolDownAirTime -= Time.deltaTime;
        if (coolDownAirTime < 0.0f)
        {
            this.PostEvent(EventID.CanAirAttack);
            isAirCoolDown = false;
            textAirCooldown.gameObject.SetActive(false);
            imageAirCooldown.fillAmount = 0.0f;
        }
        else
        {
            textAirCooldown.text = Mathf.RoundToInt(coolDownAirTime).ToString();
            imageAirCooldown.fillAmount = coolDownAirTime / coolDownAirTimer;
        }
    }
    public void UseSkillAir()
    {
        if (isAirCoolDown)
        {
            return;
        }
        else
        {
            isAirCoolDown = true;
            coolDownAirTime = TimeAirPlayer;
            textAirCooldown.gameObject.SetActive(true);
        }
    }
  
    public void GetSelectedPlayerTimeAirAttack()
    {
        if (CharacterManager.Instance.selectedCharacterIndex == 0)
        {
            if (PlayerEarthController.Instance != null)
            {
                TimeAirPlayer = PlayerEarthController.Instance.Time_air;
            }
            else
            {
                Debug.LogError("PlayerEarthController.Instance is null!"); //fix loi vi khi an start no chua chay vao day nen tao 1 script rieng o trong man choi de luu chi so
            }
        }
        else if (CharacterManager.Instance.selectedCharacterIndex == 1)
        {
            if (PlayerDiamondController.Instance != null)
            {
                TimeAirPlayer = PlayerDiamondController.Instance.Time_Air_Att;
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
                TimeAirPlayer = PlayerFireController.Instance.Time_Air_Att;
            }
            else
            {
                Debug.LogError("PlayerFireController.Instance is null!");
            }
        }
    }
}

