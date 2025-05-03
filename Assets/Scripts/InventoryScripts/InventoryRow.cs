using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRow : MonoBehaviour
{
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text effectText;
    [SerializeField] private Image icon;
    [SerializeField] private Image selectionHighlight;
    
    private PlayerItem linkedItem;
    private PlayerInventoryController inventory;

    public void Initialize(PlayerItem item, PlayerInventoryController inventory)
    {
        linkedItem = item;
        this.inventory = inventory;

        idText.text = item.ID.ToString();
        nameText.text = item.ItemName;
        effectText.text = $"{item.EffectType} +{item.EffectValue}";
        icon.sprite = item.ItemIcon.sprite;
        icon.color = item.ItemIcon.color;
    }
    
    public void SetSelected(bool selected)
    {
        if (selectionHighlight != null)
            selectionHighlight.enabled = selected;
    }
}
