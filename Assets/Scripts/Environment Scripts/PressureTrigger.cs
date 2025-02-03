using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureTrigger : MonoBehaviour
{
    
    private SpriteRenderer sr;
    private int triggers = 0;

    public event Action<bool> OnTriggerPlate;

    public bool IsTriggered()
    {
        OnTriggerPlate?.Invoke(triggers > 0);
        return triggers > 0;
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggers++;
        if (IsTriggered())
        {
            ColorManager.ChangeSpriteColor(sr, ColorManager.LOCAL_GREEN);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        triggers--;
        if (!IsTriggered())
        {
            ColorManager.ChangeSpriteColor(sr, ColorManager.LOCAL_RED);
        }
    }
}
