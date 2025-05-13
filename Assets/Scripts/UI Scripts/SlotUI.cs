using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SlotUI
{
    public Button saveButton;
    public Button loadButton;
    public Button deleteButton; // <== NEW
    public TMP_Text statusText;
}
