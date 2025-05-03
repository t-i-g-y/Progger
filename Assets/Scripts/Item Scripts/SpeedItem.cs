using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : PlayerItem
{
    public static event Action<float> OnSpeedCollected;
    public float speedMultiplier = 2f;

    public void Collect()
    {
        OnSpeedCollected?.Invoke(speedMultiplier);
        SoundEffectManager.Play("SpeedItem");
        Destroy(gameObject);
    }
}
