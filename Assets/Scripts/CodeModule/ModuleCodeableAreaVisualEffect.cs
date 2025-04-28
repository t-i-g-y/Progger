using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCodeableAreaVisualEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private float pulseSpeed = 2f;
    private float minAlpha = 0.2f;
    private float maxAlpha = 0.5f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (sr != null)
        {
            float alpha = minAlpha + (maxAlpha - minAlpha) * (0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * pulseSpeed));
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
