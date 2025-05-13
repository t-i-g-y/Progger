using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCDialogue _dialogueData;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public bool CanInteract() => !isDialogueActive;

    public void Interact()
    {
        if (_dialogueData == null || !isDialogueActive)
            return;

        if (isDialogueActive)
        {
            if (isTyping)
                SkipTyping();
            else
                NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        _dialoguePanel.SetActive(true);
        _nameText.text = _dialogueData.npcName;
        _portraitImage.sprite = _dialogueData.npcPortrait;

        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        _dialogueText.text = "";

        string line = _dialogueData.dialogueLines[dialogueIndex];
        foreach (char c in line)
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(_dialogueData.typingSpeed);
        }

        isTyping = false;

        if (_dialogueData.autoProgressLines.Length > dialogueIndex && _dialogueData.autoProgressLines[dialogueIndex])
        {
            float delay = _dialogueData.autoProgressDelay.Length > dialogueIndex ? _dialogueData.autoProgressDelay[dialogueIndex] : 2f;
            yield return new WaitForSeconds(delay);
            NextLine();
        }
    }

    private void SkipTyping()
    {
        StopAllCoroutines();
        _dialogueText.text = _dialogueData.dialogueLines[dialogueIndex];
        isTyping = false;
    }

    private void NextLine()
    {
        dialogueIndex++;
        if (dialogueIndex < _dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        _dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }
}

