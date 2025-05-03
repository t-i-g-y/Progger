using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleChildDraggable : MonoBehaviour
{
    private Module currentModule;
    private float gridSize;
    private Color validColor;
    private Color invalidColor;
    private Vector2 originalPosition;
    private SpriteRenderer spriteRenderer;
    private bool isDragging;

    public void Initialize(Module module, float gridSize, Color validColor, Color invalidColor)
    {
        currentModule = module;
        this.gridSize = gridSize;
        this.validColor = validColor;
        this.invalidColor = invalidColor;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                isDragging = true;
                originalPosition = transform.position;
            }
        }

        if (isDragging)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mouseWorldPos;

            bool isValid = false;
            foreach (var area in currentModule.AvailableAreas)
            {
                if (area.IsInside(transform.position))
                {
                    isValid = true;
                    break;
                }
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

            if (isValid)
            {
                transform.position = SnapToGrid(transform.position, gridSize);
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
    
    private Vector2 SnapToGrid(Vector2 position, float gridSize)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }
    
}
