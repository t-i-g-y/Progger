using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool canOpen;
    private BoxCollider2D doorCollider;
    private Vector3 doorScale;
    
    [SerializeField] private float scaleTime = 1f;

    private void Awake()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
        if (canOpen)
        {
            Debug.Log("Door opened");
            doorScale = transform.localScale;
            Debug.Log(doorScale);
            doorScale.y -= scaleTime * Time.deltaTime;
            Debug.Log(doorScale);
            if (doorScale.y <= 0f)
            {
                doorScale.y = 0f;
                doorCollider.enabled = false;
            }

            transform.localScale = doorScale;
        }
        
    }

    public void ActivateDoor()
    {
        canOpen = true;
    }
}
