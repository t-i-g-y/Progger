using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;



public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float extraHealth;
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private HealthUI _healthUI;
    
    public float TotalHealth => health + extraHealth;
    private void Awake()
    {
        health = _maxHealth;
        extraHealth = 0;
        _healthUI.SetMaxHearts(_maxHealth);
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Took {damage} damage\nHealth: {health} + {extraHealth}: {TotalHealth}");
        SoundEffectManager.Play("PlayerHit");
        _healthUI.UpdateHearts(health);
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
        _healthUI.UpdateHearts(health);
    }

    public void IncreaseMaxHealth(float increase)
    {
        _maxHealth += increase;
        health = _maxHealth;
        _healthUI.SetMaxHearts(_maxHealth);
    }

    public void DecreaseMaxHealth(float decrease)
    {
        _maxHealth -= decrease;
        if (health > _maxHealth)
            health = _maxHealth;
        _healthUI.SetMaxHearts(_maxHealth);
    }
}
