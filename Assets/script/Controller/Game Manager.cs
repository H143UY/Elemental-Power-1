using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Image hinhanhplayer;
    public static GameManager instance;
    public Transform spawnPoint;
    public GameObject[] players;
    public Sprite[] imagePlayer;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        hinhanhplayer = GameObject.Find("SelectedCharacterImage").GetComponent<Image>();
    }
    void Start()
    {
        int selectedCharacterIndex = CharacterManager.Instance.selectedCharacterIndex;
        SmartPool.Instance.Spawn(players[selectedCharacterIndex], spawnPoint.position, transform.rotation);
        hinhanhplayer.sprite = imagePlayer[selectedCharacterIndex];
    }
}
