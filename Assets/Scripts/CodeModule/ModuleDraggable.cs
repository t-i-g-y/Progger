using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleDraggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Transform codeableArea;
    private bool isDragging = false;

    public void SetCodeableArea(Transform area)
    {
        codeableArea = area;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
            position.z = 0;
            transform.position = position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (!codeableArea.GetComponent<Collider2D>().bounds.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }
    
    
}
