using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryRow : MonoBehaviour
{
    [SerializeField] private TMP_Text _IDText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _effectText;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _selectionHighlight;
    
    private PlayerItem linkedItem;

    public void Initialize(PlayerItem item)
    {
        linkedItem = item;
        
        _IDText.text = item.ID.ToString();
        _nameText.text = item.ItemName;
        _effectText.text = $"{item.EffectType} +{item.EffectValue}";
        _icon.sprite = item.ItemIcon.sprite;
        _icon.color = item.ItemIcon.color;
    }

    public void Initialize(UpgradeComponentItem item)
    {
        _IDText.text = item.ID.ToString();
        _nameText.text = item.ItemName;
        _effectText.text = $"{item.componentData.type} +{item.componentData.value}";
        _icon.sprite = item.ItemIcon.sprite;
        _icon.color = item.ItemIcon.color;
    }
    
    public void SetSelected(bool selected)
    {
        if (_selectionHighlight != null)
            _selectionHighlight.enabled = selected;
    }
}
