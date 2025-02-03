using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public float timeThreshold = 0.1f;
    private float timer;
    private int state = 0;
    
    private SpriteRenderer sr;
    [SerializeField] private Sprite[] animationSprites;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.time > timer)
        {
            sr.sprite = animationSprites[state++ % animationSprites.Length];
            timer = Time.time + timeThreshold;
        }
    }

    public void ChangeDirection(int direction)
    {
        if (direction > 0)
            sr.flipX = false;
        else if (direction < 0)
            sr.flipX = true;
    }
}
