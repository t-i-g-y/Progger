using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WalkingBug : MonoBehaviour
{
    private bool moveLeft;
    private Vector3 tempPos;
    private Vector3 tempScale;
    
    private Transform groundCheckPos;
    private RaycastHit2D groundHit;

    [SerializeField] private float damage = .5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        groundCheckPos = transform.GetChild(0).transform;

        if (Random.Range(0, 2) > 0)
          moveLeft = true;
        else
          moveLeft = false;

        moveLeft = Random.Range(0, 2) > 0 ? true : false;
          
    }

    private void Update()
    {
        HandleMovement();
        CheckForGround();
    }

    private void HandleMovement()
    {
        tempPos = transform.position;
        tempScale = transform.localScale;

        if (moveLeft)
        {
          tempPos.x -= moveSpeed * Time.deltaTime;
          tempScale.x = -1f;
        }
        else
        {
          tempPos.x += moveSpeed * Time.deltaTime;
          tempScale.x = 1f;
        }
            
        transform.position = tempPos;
        transform.localScale = tempScale;
    }

    private void CheckForGround()
    {
        groundHit = Physics2D.Raycast(groundCheckPos.position, Vector2.down, 0.5f, groundLayer);
        if (!groundHit)
          moveLeft = !moveLeft;
        Debug.DrawRay(groundCheckPos.position, Vector2.down * 0.5f, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
