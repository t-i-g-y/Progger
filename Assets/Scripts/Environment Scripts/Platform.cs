using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Platform : MonoBehaviour, ICSharpModifiable
{
    private ModuleObjectComponentType activeComponent = ModuleObjectComponentType.None;
    private bool playerOnPlatform;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    
    [Header("Healer Component")]
    [SerializeField] private float _healingDuration = 1f;
    [SerializeField] private float _healingAmount = 0.5f;
    Coroutine healingCoroutine;
    
    [Header("Amplifier Component")]
    [SerializeField] private float _amplifierModifier = 0.5f;

    [Header("Catcher Component")] 
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Vector2 _boxSize = new Vector2(0.1f, 1f);
    
    private IEnumerator HealingRoutine()
    {
        if (playerHealth == null) 
            yield break;
        
        while (playerOnPlatform)
        {
            yield return new WaitForSeconds(_healingDuration);
            playerHealth.Heal(_healingAmount);
        }
    }

    private void ApplyAmplifierToPlayer()
    {
        if (playerMovement == null)
            return;
        playerMovement.SpeedMultiplier += _amplifierModifier;
        playerMovement.JumpMultiplier += _amplifierModifier;
    }

    private void RemoveAmplifierFromPlayer()
    {
        if (playerMovement == null)
            return;
        playerMovement.SpeedMultiplier -= _amplifierModifier;
        playerMovement.JumpMultiplier -= _amplifierModifier;
    }

    private void TryCatchEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _boxSize, 0f, _enemyLayer);
        Debug.Log("Trying to catch enemies");
        foreach (var hit in hits)
        {
            ITryCatcheable catchable = hit.transform.GetComponent<ITryCatcheable>();
            if (catchable != null)
            {
                catchable.GetCaught();
            }
            else
            {
                Rigidbody2D rb = hit.attachedRigidbody;
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Static;
                }

                MonoBehaviour script = hit.GetComponent<MonoBehaviour>();
                if (script != null)
                {
                    script.enabled = false;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            if (playerHealth == null)
                playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerMovement == null)
                playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            
            playerOnPlatform = true;
            if (activeComponent == ModuleObjectComponentType.Healer)
                healingCoroutine = StartCoroutine(HealingRoutine());
            if (activeComponent == ModuleObjectComponentType.Amplifier)
                ApplyAmplifierToPlayer();
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"Still colliding with {collision.gameObject.name}");
        if (activeComponent == ModuleObjectComponentType.TryCatcher)
        {
            TryCatchEnemies();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            playerOnPlatform = false;
            if (activeComponent == ModuleObjectComponentType.Healer)
            {
                if (healingCoroutine != null)
                {
                    StopCoroutine(healingCoroutine);
                    healingCoroutine = null;
                }
            }
            if (activeComponent == ModuleObjectComponentType.Amplifier)
                RemoveAmplifierFromPlayer();
        }
    }

    public void ApplyModuleComponent(ModuleObjectComponentType componentType)
    {
        activeComponent = componentType;
    }

    public void RemoveModuleComponent(ModuleObjectComponentType componentType)
    {
        activeComponent = ModuleObjectComponentType.None;
    }

    public bool HasModuleComponent()
    {
        return activeComponent != ModuleObjectComponentType.None;
    }

    public ModuleObjectComponentType GetModuleComponentType()
    {
        return activeComponent;
    }

    private void OnDrawGizmosSelected()
    {
        if (activeComponent == ModuleObjectComponentType.TryCatcher)
        {
            Gizmos.color = Color.red;
            Vector2 center = (Vector2)transform.position + Vector2.up;
            Gizmos.DrawWireCube(center, _boxSize);
        }
    }

}
