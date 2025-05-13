using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CentralConsole : MonoBehaviour
{
    [SerializeField] private ProgrammingLanguage _languageToReveal;
    [SerializeField] private MapManager _mapManager;

    private bool playerInRange;
    private bool alreadyRevealed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
            playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !alreadyRevealed)
        {
            if (AbilityManager.Instance.AbilityUnlocked(_languageToReveal))
            {
                _mapManager.RevealZone(_languageToReveal);
                alreadyRevealed = true;
                Debug.Log($"Zone revealed: {_languageToReveal}");
            }
        }
    }
}
