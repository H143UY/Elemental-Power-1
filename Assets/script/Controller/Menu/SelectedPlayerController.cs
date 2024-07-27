using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedPlayerController : MonoBehaviour
{
    public PlayerModelController[] playermodel;
    public Transform transpot;
    public List<GameObject> players;
    private int current;
    [Header("thông tin")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI HomeTown;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI infor;
    public TextMeshProUGUI DiemManh;
    public TextMeshProUGUI DiemYeu;
    private void Awake()
    {
        Name = Name.GetComponent<TextMeshProUGUI>();
        HomeTown = HomeTown.GetComponent<TextMeshProUGUI>();
        Damage = Damage.GetComponent<TextMeshProUGUI>();
        infor = infor.GetComponent<TextMeshProUGUI>();
        DiemManh = DiemManh.GetComponent<TextMeshProUGUI>();
        DiemYeu = DiemYeu.GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        players = new List<GameObject>();   
        foreach (var player in playermodel)
        {
            GameObject go = Instantiate(player.Player, transpot.position, Quaternion.identity);
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
        Damage.text = "Damage :" + playermodel[current].Damage;
        infor.text = "Thông tin:" + playermodel[current].Infoamtion;
        DiemManh.text = "Điểm mạnh :" + playermodel[current].DiemManh;
        DiemYeu.text = "Điểm yếu :" + playermodel[current].DiemYeu;
    }
    public void OnclickNext()
    {
        players[current].SetActive(false);
        if(current <players.Count -1)
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
        if(current == 0)
        {
            current = players.Count - 1;
        }
        else
        {
            current--;
        }
        ShowPlayerFromList();
    }

}
