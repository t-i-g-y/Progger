using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ModuleObject currentObject;
    private Module currentModule;
    private RectTransform rectTransform;
    private Canvas canvas;

    public void Intialize(ModuleObject moduleObject, Module module)
    {
        currentObject = moduleObject;
        currentModule = module;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        Sprite prefabSprite = currentObject.prefab.GetComponent<SpriteRenderer>()?.sprite;
        GetComponent<UnityEngine.UI.Image>().sprite = prefabSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (var area in currentModule.AvailableAreas)
        {
            if (area.IsInside(worldPosition))
            {
                Vector2 snappedPosition = SnapToGrid(worldPosition, 0.16f);
                GameObject placed = Instantiate(currentObject.prefab, snappedPosition, Quaternion.identity);
                currentObject.isPlaced = true;
                Destroy(gameObject);
                break;
            }
        }

        if (!currentObject.isPlaced)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    private Vector2 SnapToGrid(Vector2 position, float gridSize)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }
}

[System.Serializable]
public struct ModuleObject
{
    public GameObject prefab;
    public bool isPlaced;
    public bool isMultipart;
}