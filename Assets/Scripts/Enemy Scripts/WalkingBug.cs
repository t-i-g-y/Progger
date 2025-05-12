using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WalkingBug : MonoBehaviour, ITryCatcheable
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private LayerMask _groundLayer;
    private bool moveLeft;
    private Vector3 tempPos;
    private Vector3 tempScale;
    private Transform groundCheckPos;
    private RaycastHit2D groundHit;
    private Rigidbody2D rb;
    
    [Header("Damage")]
    [SerializeField] private float _damage = .5f;
    [SerializeField] private float _damageCoolDown = 0.5f;
    private float damageTimer;
    private bool canDamage;
    private PlayerHealth playerHealth;

    [Header("Module Behaviour")] 
    [SerializeField] private bool _isInModule;
    private bool hasReset;
    private Vector3 startPosition;
    
    [Header("TryCatch")] 
    [SerializeField] private Sprite _caughtSprite;
    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    bool isCaught;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalSprite = spriteRenderer.sprite;
        originalColor = spriteRenderer.color;
        groundCheckPos = transform.GetChild(0).transform;
        if (_isInModule)
            startPosition = transform.position;
        
        moveLeft = Random.Range(0, 2) > 0 ? true : false;
    }

    private void Update()
    {
        if (!isCaught)
            HandleMovement();
        if (_isInModule)
            HandleReset();
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

    private void HandleReset()
    {
        if (!hasReset && rb.velocity.y < -0.5f)
        {
            hasReset = true;
            Debug.Log($"Resetting at {rb.velocity.y}");
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;

            transform.position = startPosition;

            isCaught = false;
            spriteRenderer.sprite = originalSprite;
            spriteRenderer.color = originalColor;
            StartCoroutine(RestorePhysics());
        }
    }
    
    private IEnumerator RestorePhysics()
    {
        yield return new WaitForFixedUpdate(); 

        rb.bodyType = RigidbodyType2D.Dynamic;
        hasReset = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCaught)
            return;
        
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            if (damageTimer <= 0)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(_damage);
                damageTimer = _damageCoolDown;
            }
        }
    }

    public void GetCaught()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.grey;
            spriteRenderer.sprite = _caughtSprite;
        }
        isCaught = true;
    }
}
