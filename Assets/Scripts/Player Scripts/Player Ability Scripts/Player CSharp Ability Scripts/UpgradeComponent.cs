using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum UpgradeType
{
    MaxJump,
    Speed,
    JumpBoost,
    Health
}

public class UpgradeComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UpgradeType type;
    public float value;

    [HideInInspector] public Transform originalParent;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(UpgradeType type, float value, string labelText)
    {
        this.type = type;
        this.value = value;
        GetComponentInChildren<TMP_Text>().text = labelText;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
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
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
