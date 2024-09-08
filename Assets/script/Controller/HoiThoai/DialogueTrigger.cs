using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public List<PlayerModelController> playerModels; // Danh sách các nhân vật
    private bool IsDialogue = true;

    private void Awake()
    {
        // Đăng ký sự kiện tìm kiếm người chơi
        this.RegisterListener(EventID.FindPlayer, (sender, param) =>
        {
            // Lấy nhân vật đã chọn từ CharacterManager
            int selectedCharacterIndex = CharacterManager.Instance.selectedCharacterIndex;
            PlayerModelController selectedPlayer = playerModels[selectedCharacterIndex];

            foreach (DialogueLine line in dialogue.dialogueLines)
            {
                // Gán tên và hình ảnh từ nhân vật đã chọn
                if (string.IsNullOrEmpty(line.character.name))
                {
                    line.character.name = selectedPlayer.Name;
                }

                if (line.character.icon == null)
                {
                    line.character.icon = selectedPlayer.AvatarPlayer;
                }
            }
        });

        this.RegisterListener(EventID.StartDialogue, (sender, param) =>
        {
            if (IsDialogue)
            {
                TriggerDialogue();
                IsDialogue = false;
            }
        });
    }

    public void TriggerDialogue()
    {
        if (DialogueManager.instance != null) // Kiểm tra null trước khi sử dụng
        {
            DialogueManager.instance.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogError("DialogueManager instance is null in TriggerDialogue!");
        }
    }
}