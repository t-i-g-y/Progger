using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using TMPro;

public class ModuleDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ModuleObject currentObject;
    private Module currentModule;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;
    private Vector2 originalPosition;
    [SerializeField] private Color _validColor = Color.green;
    [SerializeField] private Color _invalidColor = Color.red;
    [SerializeField] private TMP_Text _upperText;
    [SerializeField] private TMP_Text _lowerText;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(ModuleObject moduleObject, Module module)
    {
        currentObject = moduleObject;
        currentModule = module;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        Sprite prefabSprite;

        if (currentObject.IsPointer)
        {
            _upperText.text = currentObject.TargetID.ToString();
            _lowerText.text = currentObject.SourceID.ToString();
        }
        else
        {
            _upperText.text = String.Empty;
            _lowerText.text = String.Empty;
        }
        
        if (currentObject.IsMultipart)
        {
            prefabSprite = currentObject.Prefab.transform.GetChild(0).GetComponent<SpriteRenderer>()?.sprite;
        }
        else
        {
            prefabSprite = currentObject.Prefab.GetComponent<SpriteRenderer>()?.sprite;
        }
        if (prefabSprite != null)
        {
            image.sprite = prefabSprite;
            image.preserveAspect = true;
            
            rectTransform.sizeDelta = new Vector2(prefabSprite.bounds.size.x, prefabSprite.bounds.size.y) * 64;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform, false);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 snappedPosition = ModuleUIManager.Instance.SnapToGrid(worldPosition);
        bool isValid = false;
        foreach (var area in currentModule.AvailableAreas)
        {
            ModuleUIManager.Instance.GridHighlight.SetActive(false);
            if (area.IsInside(worldPosition) && !currentModule.OccupiedTiles.Contains(snappedPosition))
            {
                isValid = true;
                ModuleUIManager.Instance.GridHighlight.transform.position = snappedPosition;
                ModuleUIManager.Instance.GridHighlight.SetActive(true);
                break;
            }
        }

        if (image != null)
        {
            image.color = isValid ? _validColor : _invalidColor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        foreach (var area in currentModule.AvailableAreas)
        {
            Vector3 snappedPosition = ModuleUIManager.Instance.SnapToGrid(worldPosition);
            if (area.IsInside(worldPosition) && !currentModule.OccupiedTiles.Contains(snappedPosition))
            {
                currentModule.OccupiedTiles.Add(snappedPosition);
                GameObject placed = Instantiate(currentObject.Prefab, snappedPosition, Quaternion.identity, currentModule.PlacedObjectsParent);
                currentObject.IsPlaced = true;
                if (currentObject.IsPointer)
                {
                    PointerBlock pointerBlock = placed.GetComponent<PointerBlock>();
                    if (pointerBlock != null)
                    {
                        pointerBlock.Initialize(currentObject.SourceID, currentObject.TargetID, currentModule);
                        currentModule.PointerBlocks.Add(pointerBlock);
                    }
                }
                if (currentObject.IsMultipart)
                {
                    SetupMultipartChildren(placed);
                }
                ModuleUIManager.Instance.GridHighlight.SetActive(false);
                Destroy(gameObject);
                return;
            }
        }
        transform.SetParent(ModuleUIManager.Instance.PaletteParent, false);
        GetComponent<Image>().color = Color.white;
    }

    private void SetupMultipartChildren(GameObject parent)
    {
        Transform root = parent.transform;
        for (int i = 1; i < root.childCount; i++)
        {
            GameObject child = root.GetChild(i).gameObject;
            child.AddComponent<ModuleChildDraggable>().Initialize(currentModule, _validColor, _invalidColor);
        }
    }
}
