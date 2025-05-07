using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public enum UpgradeType
{
    MaxJump,
    Speed,
    JumpBoost,
    Health
}

public class UpgradeComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private UpgradeComponentData _data;
    private bool wasInSlot;
    public UpgradeComponentData ComponentData
    {
        get => _data; 
        set => _data = value;
    }
    
    public bool WasInSlot => wasInSlot;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(UpgradeComponentData data)
    {
        _data = data;
        GetComponentInChildren<TMP_Text>().text = GetLabelText();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        wasInSlot = false;
        
        if (originalParent.TryGetComponent(out UpgradeSlot slot))
        {
            UpgradeComponentManager.Instance.UnassignComponent(slot.currentUpgrade.ComponentData);
            slot.currentUpgrade = null;
        }

        transform.SetParent(transform.root, true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        
        if (transform.parent == transform.root)
        {
            if (originalParent.TryGetComponent(out UpgradeSlot slot))
            {
                //slot.ClearSlot();
                
            }

            UpgradeComponentManager.Instance.ReturnToComponentList(this);
        }
    }

    private string GetLabelText()
    {
        switch (_data.type)
        {
            case UpgradeType.MaxJump:
                return "+1 Max Jump";
            case UpgradeType.Speed:
                return "+50% Speed";
            case UpgradeType.JumpBoost:
                return "+50% Jump";
            case UpgradeType.Health:
                return "+3 Health";
        }
        return null;
    }
}
