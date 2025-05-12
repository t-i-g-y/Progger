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
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private HealthUI _healthUI;
    
    public float TotalHealth => health + extraHealth;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        StartCoroutine(DamageFlashRedCoroutine());
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageFlashRedCoroutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
    
    public void Heal(float heal)
    {
        health += heal;
        if (health > _maxHealth)
            health = _maxHealth;
        StartCoroutine(HealFlashGreenCoroutine());
        _healthUI.UpdateHearts(health);
    }

    private IEnumerator HealFlashGreenCoroutine()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            spriteRenderer.color = Color.Lerp(Color.white, Color.green, t);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.05f);
        
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            spriteRenderer.color = Color.Lerp(Color.green, Color.white, t);
            yield return null;
        }

        spriteRenderer.color = Color.white;
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
