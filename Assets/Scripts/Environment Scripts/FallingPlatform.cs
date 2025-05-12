using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingPlatform : MonoBehaviour, ICSharpModifiable
{
    [SerializeField] private float _fallWait = 2f;
    [SerializeField] private float _destroyTime = 1f;
    private bool isFalling;
    private Rigidbody2D rb;
    private ModuleObjectComponentType activeComponent = ModuleObjectComponentType.None;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isFalling && other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            StartCoroutine(FallingPlatformCoroutine());
        }
    }

    private IEnumerator FallingPlatformCoroutine()
    {
        isFalling = true;
        yield return new WaitForSeconds(_fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Destroy(gameObject, _destroyTime);
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
}
