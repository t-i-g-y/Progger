using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    private bool terminalIsActivated;
    private bool terminalIsUsable;
    private SpriteRenderer terminalSprite;

    [SerializeField] private Door door;
    [SerializeField] private PressureTrigger pressurePlate;
    
    private void Awake()
    {
        terminalSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (pressurePlate)
            pressurePlate.OnTriggerPlate += ActivateTerminal;
    }

    private void Update()
    {
        if (terminalIsUsable && terminalIsActivated && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed");
            door.ActivateDoor();
        }
    }
    
    private void ActivateTerminal(bool trigger)
    {
        Debug.Log("Trigger terminal");
        terminalIsActivated = trigger;
        if (trigger)
        {
            ColorManager.ChangeSpriteColor(terminalSprite, ColorManager.LOCAL_GREEN);
        }
        else
        {
            ColorManager.ChangeSpriteColor(terminalSprite, ColorManager.LOCAL_RED);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
            terminalIsUsable = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
            terminalIsUsable = false;
    }
}
