﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Image CharacteIcon;
    public Text NameText;
    public Text DialogueText;
    public Queue<DialogueLine> sentences = new Queue<DialogueLine>(); // Khởi tạo hàng đợi
    public float Speed_Word;
    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("instance null");
        }
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisPlayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        this.gameObject.SetActive(true);
        sentences.Clear();
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            sentences.Enqueue(dialogueLine);
        }
        DisPlayNextSentence();
    }

    public void DisPlayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLine currentline = sentences.Dequeue();
        CharacteIcon.sprite = currentline.character.icon;
        NameText.text = currentline.character.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentline));
    }

    IEnumerator TypeSentence(DialogueLine dialogueline)
    {
        DialogueText.text = "";
        foreach (char letter in dialogueline.line.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(Speed_Word);
        }
    }

    void EndDialogue()
    {
        this.PostEvent(EventID.SinhQuaiXong);
        anim.SetBool("close", true);
    }
    private void close()
    {
        this.gameObject.SetActive(false);
        this.PostEvent(EventID.SinhQuaiXong);
        this.PostEvent(EventID.SwichCamera3);
    }

}
