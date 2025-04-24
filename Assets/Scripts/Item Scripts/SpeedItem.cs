using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public static event Action<float> OnSpeedCollected;
    public float speedMultiplier = 2f;

    private void Collect()
    {
        OnSpeedCollected?.Invoke(speedMultiplier);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with " + other.gameObject.name);
            Collect();
        }
    }
}
