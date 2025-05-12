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
    private UpgradeComponentData data;
    private bool wasInSlot;
    public UpgradeComponentData ComponentData
    {
        get => data; 
        set => data = value;
    }
    
    public bool WasInSlot => wasInSlot;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(UpgradeComponentData data)
    {
        this.data = data;
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
            UpgradeComponentManager.Instance.ReturnToComponentList(this);
        }
    }

    private string GetLabelText()
    {
        switch (data.type)
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
