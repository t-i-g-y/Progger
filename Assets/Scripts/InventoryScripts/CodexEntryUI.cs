using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodexEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _languageText;
    [SerializeField] private TMP_Text _idText;
    [SerializeField] private TMP_Text _codexText;

    public void Initialize(CodexEntryData data)
    {
        _languageText.text = data.language.ToString();
        _idText.text = data.ID.ToString();
        _codexText.text = data.codexText;
    }
}
