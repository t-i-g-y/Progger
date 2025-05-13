using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexItem : MonoBehaviour
{
    public ProgrammingLanguage language;
    [TextArea] public string codexText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            PlayerInventoryController.Instance.CollectCodexItem(this);
            gameObject.SetActive(false);
        }
    }
}
