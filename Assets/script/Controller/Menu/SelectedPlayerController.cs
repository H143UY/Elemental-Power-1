using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectedPlayerController : MonoBehaviour
{
    public PlayerModelController[] playermodel;
    public Transform transpot;
    private List<GameObject> players;
    private int current;
    [Header("thông tin")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI HomeTown;
    public TextMeshProUGUI infor;
    public TextMeshProUGUI DiemManh;
    public TextMeshProUGUI DiemYeu;
    private void Awake()
    {
        Name = Name.GetComponent<TextMeshProUGUI>();
        HomeTown = HomeTown.GetComponent<TextMeshProUGUI>();
        infor = infor.GetComponent<TextMeshProUGUI>();
        DiemManh = DiemManh.GetComponent<TextMeshProUGUI>();
        DiemYeu = DiemYeu.GetComponent<TextMeshProUGUI>();
    }
    public void SpawnChosePlayer()
    {
        players = new List<GameObject>();
        foreach (var player in playermodel)
        {
            GameObject go = SmartPool.Instance.Spawn(player.Player, transpot.position, transpot.rotation);
            go.SetActive(false);
            go.transform.SetParent(transpot);
            players.Add(go);
        }
        ShowPlayerFromList();
    }
    void ShowPlayerFromList()
    {
        players[current].SetActive(true);
        Name.text = "Name :" + playermodel[current].Name;
        HomeTown.text = "HomeTown :" + playermodel[current].HomeTown;
        infor.text = "Infomation:" + playermodel[current].Infoamtion;
        DiemManh.text = "Strengths :" + playermodel[current].DiemManh;
        DiemYeu.text = "Weakness :" + playermodel[current].DiemYeu;
    }
    public void OnclickNext()
    {
        players[current].SetActive(false);
        if (current < players.Count - 1)
        {
            current++;
        }
        else
        {
            current = 0;
        }
        ShowPlayerFromList();
    }
    public void OnclickPrev()
    {
        players[current].SetActive(false);
        if (current == 0)
        {
            current = players.Count - 1;
        }
        else
        {
            current--;
        }
        ShowPlayerFromList();
    }
    public void OnclickChose()
    {
        players[current].SetActive(false);
        CharacterManager.Instance.SelectCharacter(current);
    }
    
}
