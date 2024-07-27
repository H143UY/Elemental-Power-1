using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModelController",menuName ="Player")]
public class PlayerModelController : ScriptableObject
{
    public string Name;
    public string HomeTown;
    public string Damage;
    public string DiemManh;
    public string DiemYeu;
    public string Infoamtion;
    public enum Perfomance { Tier1, Tier2, Tier3}
    public Perfomance pertype;
    public GameObject Player;
}
