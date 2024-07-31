using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public int selectedCharacterIndex = -1; // Chỉ số mặc định khi chưa chọn nhân vật nào

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
    }
}