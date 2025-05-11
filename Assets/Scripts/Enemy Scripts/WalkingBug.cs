using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WalkingBug : MonoBehaviour
{
    private bool moveLeft;
    private Vector3 tempPos;
    private Vector3 tempScale;
    
    private Transform groundCheckPos;
    private RaycastHit2D groundHit;
    [SerializeField] private float _damage = .5f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _damageCoolDown = 0.5f;
    private float damageTimer;
    private bool canDamage;
    private PlayerHealth playerHealth;

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
        if (damageTimer > 0)
          damageTimer -= Time.deltaTime;
    }

    private void HandleMovement()
    {
        tempPos = transform.position;
        tempScale = transform.localScale;

        if (moveLeft)
        {
          tempPos.x -= _moveSpeed * Time.deltaTime;
          tempScale.x = -1f;
        }
        else
        {
          tempPos.x += _moveSpeed * Time.deltaTime;
          tempScale.x = 1f;
        }
            
        transform.position = tempPos;
        transform.localScale = tempScale;
    }

    private void CheckForGround()
    {
        groundHit = Physics2D.Raycast(groundCheckPos.position, Vector2.down, 0.5f, _groundLayer);
        if (!groundHit)
          moveLeft = !moveLeft;
        Debug.DrawRay(groundCheckPos.position, Vector2.down * 0.5f, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            if (damageTimer <= 0)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(_damage);
                damageTimer = _damageCoolDown;
            }
        }
    }
}
