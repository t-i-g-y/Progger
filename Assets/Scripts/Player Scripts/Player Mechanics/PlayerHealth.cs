using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private Sprite[] _heartSprites;
    [SerializeField] private SpriteRenderer[] _heartRenderers;
    
    public float Health => health;
    private void Awake()
    {
        health = _maxHealth;
        //SetHearts();
    }
    
    private void SetHearts()
    {
        int heartIndex = (int)(health * 2);
        for (int i = 0; i < (int)_maxHealth; i++)
        {
            if (heartIndex > 1)
                _heartRenderers[i].sprite = _heartSprites[(int)HeartType.FullHeart];
            else if (heartIndex > 0)
                _heartRenderers[i].sprite = _heartSprites[(int)HeartType.HalfHeart];
            else
                _heartRenderers[i].sprite = _heartSprites[(int)HeartType.NoHeart];
            heartIndex -= 2;
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Took {damage} damage\nHealth: {health}");
        SoundEffectManager.Play("PlayerHit");
        //SetHearts();
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(float heal)
    {
        health += heal;
        if (health > _maxHealth)
            health = _maxHealth;
    }

    public void IncreaseMaxHealth(float increase)
    {
        _maxHealth += increase;
        health = _maxHealth;
    }

    public void DecreaseMaxHealth(float decrease)
    {
        _maxHealth -= decrease;
        if (health > _maxHealth)
            health = _maxHealth;
    }
}
