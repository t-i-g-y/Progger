using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovingPlatform : MonoBehaviour, ICSharpModifiable
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _moveSpeed = 2f;
    private Vector3 nextPosition;
    private ModuleObjectComponentType activeComponentType = ModuleObjectComponentType.None;
    
    private void Start()
    {
        _pointA.SetParent(null);
        _pointB.SetParent(null);
        nextPosition = _pointB.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, _moveSpeed * Time.deltaTime);
        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == _pointB.position) ? _pointA.position : _pointB.position;
        }
    }

    private void OnDestroy()
    {
        Destroy(_pointA.gameObject);
        Destroy(_pointB.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = null;
        }
    }
    
    public void ApplyModuleComponent(ModuleObjectComponentType componentType)
    {
        
    }

    public void RemoveModuleComponent(ModuleObjectComponentType componentType)
    {
        
    }

    public bool HasModuleComponent()
    {
        return activeComponentType != ModuleObjectComponentType.None;
    }
    
    public ModuleObjectComponentType GetModuleComponentType()
    {
        return ModuleObjectComponentType.None;
    }
}
