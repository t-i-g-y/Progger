using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModuleDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ModuleObject currentObject;
    private Module currentModule;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;
    private Vector2 originalPosition;
    [SerializeField] private float gridSize = 0.16f;
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

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
        Sprite prefabSprite = currentObject.Prefab.GetComponent<SpriteRenderer>()?.sprite;
        if (prefabSprite != null)
        {
            var image = GetComponent<Image>();
            image.sprite = prefabSprite;
            image.preserveAspect = true;
            
            rectTransform.sizeDelta = new Vector2(prefabSprite.bounds.size.x, prefabSprite.bounds.size.y) * 64;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isValid = false;
        foreach (var area in currentModule.AvailableAreas)
        {
            if (area.IsInside(worldPosition))
            {
                isValid = true;
                break;
            }
        }

        if (image != null)
        {
            image.color = isValid ? validColor : invalidColor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //bool placed = false;
        
        foreach (var area in currentModule.AvailableAreas)
        {
            if (area.IsInside(worldPosition))
            {
                Vector2 snappedPosition = SnapToGrid(worldPosition, gridSize);
                GameObject placed = Instantiate(currentObject.Prefab, snappedPosition, Quaternion.identity, currentModule.PlacedObjectsParent);
                currentObject.IsPlaced = true;
                Destroy(gameObject);
                //placed = true;
                break;
            }
        }
        
        if (!currentObject.IsPlaced)
        {
            rectTransform.anchoredPosition = originalPosition;
            GetComponent<Image>().color = Color.white;
        }
    }

    private Vector2 SnapToGrid(Vector2 position, float gridSize)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }
}
