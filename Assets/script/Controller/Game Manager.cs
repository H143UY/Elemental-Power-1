    using Cinemachine;
using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerModelController[] playermodel;
    public Image hinhanhplayer;
    private Image SkillPlayer;
    private Image AirAtt;
    public string NamePlayer;
    public static GameManager instance;
    public Transform spawnPoint;
    public GameObject[] players;
    private bool NextMan = false;
    private int selectedCharacterIndex;
    public CinemachineVirtualCamera VCam; 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        this.RegisterListener(EventID.QuaMan, (sender, param) =>
        {
            NextMan = true;
        });
        selectedCharacterIndex = CharacterManager.Instance.selectedCharacterIndex;
        GameObject cine = Instantiate(players[selectedCharacterIndex], spawnPoint.position, Quaternion.identity);
        VCam.Follow = cine.transform;
        NamePlayer = playermodel[selectedCharacterIndex].Name;
    }

    void Start()
    {
        this.PostEvent(EventID.FindPlayer);
        ChangeImgePlayer();
    }
    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            if (NextMan)
            {
                LoadNextScene();
                NextMan = false;
            }
        }
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes in build settings!");
        }
    }
    void ChangeImgePlayer()
    {
        hinhanhplayer = GameObject.Find("SelectedCharacterImage").GetComponent<Image>();
        AirAtt = GameObject.Find("AirAtt").GetComponent<Image>();
        SkillPlayer = GameObject.Find("skill").GetComponent<Image>();
        hinhanhplayer.sprite = playermodel[selectedCharacterIndex].AvatarPlayer;
        AirAtt.sprite = playermodel[selectedCharacterIndex].ImgAirAtt;
        SkillPlayer.sprite = playermodel[selectedCharacterIndex].ImgSkill;
    }
}
