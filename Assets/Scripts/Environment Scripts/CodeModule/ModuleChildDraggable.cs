using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleChildDraggable : MonoBehaviour
{
    private Module currentModule;
    private Color validColor;
    private Color invalidColor;
    private Vector2 originalPosition;
    private SpriteRenderer spriteRenderer;
    private bool isDragging;

    public void Initialize(Module module, Color validColor, Color invalidColor)
    {
        currentModule = module;
        this.validColor = validColor;
        this.invalidColor = invalidColor;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!ModuleUIManager.Instance.EditorOpen)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                isDragging = true;
                originalPosition = transform.position;
                ModuleUIManager.Instance.GridHighlight.SetActive(true);
            }
        }
        
        if (isDragging)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseWorldPos;
            Vector3 snappedPosition = ModuleUIManager.Instance.SnapToGrid(transform.position);
            bool isValid = false;
            foreach (var area in currentModule.AvailableAreas)
            {
                if (area.IsInside(transform.position))
                {
                    isValid = true;
                    ModuleUIManager.Instance.GridHighlight.transform.position = snappedPosition;
                    ModuleUIManager.Instance.GridHighlight.SetActive(true);
                    break;
                }
                ModuleUIManager.Instance.GridHighlight.SetActive(false);
            }

            if (spriteRenderer != null)
                spriteRenderer.color = isValid ? validColor : invalidColor;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            bool isValid = false;
            foreach (var area in currentModule.AvailableAreas)
            {
                if (area.IsInside(transform.position))
                {
                    isValid = true;
                    break;
                }
            }
            ModuleUIManager.Instance.GridHighlight.SetActive(false);
            if (isValid)
            {
                transform.position = ModuleUIManager.Instance.SnapToGrid(transform.position);
            }
            else
            {
                transform.position = originalPosition;
            }

            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;

            isDragging = false;
        }
    }
    
}
