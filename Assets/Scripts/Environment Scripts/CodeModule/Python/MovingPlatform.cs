using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, ICSharpModifiable
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 2f;
    private Vector3 nextPosition;
    private void Start()
    {
        pointA.SetParent(null);
        pointB.SetParent(null);
        nextPosition = pointB.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointB.position) ? pointA.position : pointB.position;
        }
    }

    private void OnDestroy()
    {
        Destroy(pointA.gameObject);
        Destroy(pointB.gameObject);
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
}
