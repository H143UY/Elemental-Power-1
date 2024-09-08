using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerModelController",menuName ="Player")]
public class PlayerModelController : ScriptableObject
{
    public string Name;
    public string HomeTown;
    public string DiemManh;
    public string DiemYeu;
    public string Infoamtion;
    public GameObject Player;

    //giao dien play chon
    public Sprite AvatarPlayer;
    public Sprite ImgAirAtt;
    public Sprite ImgSkill;
}
