using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeartType
{
    NoHeart = 0,
    HalfHeart = 1,
    FullHeart = 2
}

public class PlayerHealth : MonoBehaviour
{
    private float health;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private Sprite[] heartSprites;
    [SerializeField] private SpriteRenderer[] heartRenderers;
    
    public float Health => health;
    private void Awake()
    {
        health = maxHealth;
        //SetHearts();
    }
    
    private void SetHearts()
    {
        int heartIndex = (int)(health * 2);
        for (int i = 0; i < (int)maxHealth; i++)
        {
            if (heartIndex > 1)
                heartRenderers[i].sprite = heartSprites[(int)HeartType.FullHeart];
            else if (heartIndex > 0)
                heartRenderers[i].sprite = heartSprites[(int)HeartType.HalfHeart];
            else
                heartRenderers[i].sprite = heartSprites[(int)HeartType.NoHeart];
            heartIndex -= 2;
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Took {damage} damage\nHealth: {health}");
        //SetHearts();
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
