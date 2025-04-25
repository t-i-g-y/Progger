using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private BoxCollider2D playerCollider;
    private CircleCollider2D collectorCollider;
    private Rigidbody2D playerBody;
    private Transform playerTransform;
    private PlayerAnimation playerAnimation;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private float speedMultiplier = 1f;
    private float horizontalMovement;
    private Vector3 movePos;
    
    [Header("Jumping")]
    [SerializeField] private float normalJumpForce = 5f;
    [SerializeField] private float doubleJumpForce = 4f;
    private bool jumped;
    private float jumpForce = 5f;
    private int maxJumps = 2;
    private int jumpsRemaining;
    
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    private RaycastHit2D groundCast;


    [Header("Dropping")] 
    [SerializeField] private float dropTime = 0.25f;
    private bool isOnPlatform;
    
    [Header("Gravity")] 
    [SerializeField] private float baseGravity = 2;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float fallMultiplier = 2.5f;

    [Header("Dashing")] 
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 0.2f;
    private bool isDashing;
    private bool canDash = true;
    private TrailRenderer trailRenderer;
    
    [Header("Wall Movement")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float wallSlideSpeed = 2f;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private RaycastHit2D wallCast;
    private bool isWallJumping;
    private float wallJumpDirection;
    [SerializeField] private float wallJumpTime = 0.5f;
    private float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    public float SpeedMultiplier
    {
        get => speedMultiplier;
        set => speedMultiplier = value;
    }
    
    public int MaxJumps
    {
        get => maxJumps;
        set => maxJumps = value;
    }

    public bool CanDash
    {
        get => canDash;
        set => canDash = value;
    }
    
    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();
        //playerAnimation = GetComponent<PlayerAnimation>();
        playerTransform = GetComponent<Transform>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        SpeedItem.OnSpeedCollected += StartSpeedBoost;
    }

    private void Update()
    {
        if (isDashing)
            return;
        HandleJumping();
        HandleGravity();
        HandleWallSlide();
        HandleWallJump();
        HandleDash();
        HandleDrop();
        if (!isWallJumping)
        {
            horizontalMovement = Input.GetAxisRaw(TagManager.HORIZONTAL_MOVEMENT);
            Flip();
        }
        
    }
    
    private void FixedUpdate()
    {
        if (isDashing)
            return;
        
        if (!isWallJumping)
            HandleMovement();
    }

    private void StartSpeedBoost(float multiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(2f);
        speedMultiplier = 1f;
    }
    
    private void HandleMovement()
    {
        if (horizontalMovement > 0)
            playerBody.velocity = new Vector2(moveSpeed * speedMultiplier, playerBody.velocity.y);
        else if (horizontalMovement < 0)
            playerBody.velocity = new Vector2(-moveSpeed * speedMultiplier, playerBody.velocity.y);
        else
            playerBody.velocity = new Vector2(0f, playerBody.velocity.y);
    }

    private void HandleAnimation()
    {
        playerAnimation.ChangeDirection((int)playerBody.velocity.x);
    }

    private void HandleJumping()
    {
        //IsGrounded();
        if (Input.GetButtonDown(TagManager.JUMP_BUTTON))
        {
            Debug.Log("Jump pressed");
            if (IsGrounded())
            {
                jumpsRemaining = maxJumps;
                jumpForce = normalJumpForce;
                Jump();
            }
            else if (jumped && jumpsRemaining > 0)
            {
                jumpForce = doubleJumpForce;
                Jump();
            }
            if (wallJumpTimer > 0f)
            {
                isWallJumping = true;
                playerBody.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
                wallJumpTimer = 0;

                if (transform.localScale.x != wallJumpDirection)
                {
                    isFacingRight = !isFacingRight;
                    Vector3 localScale = transform.localScale;
                    localScale.x *= -1f;
                    transform.localScale = localScale;
                }
                
                Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
            }
            
        }
    }

    private void HandleWallSlide()
    {
        if (!IsGrounded() && IsOnWall() && horizontalMovement != 0)
        {
            Debug.Log("Is on wall");
            isWallSliding = true;
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(playerBody.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
        
    }

    private void HandleWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            Debug.Log("Dash pressed");
            StartCoroutine(DashCoroutine());
        }
    }
    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Jump()
    {
        if (wallJumpTimer > 0f)
        {
            isWallJumping = true;
            playerBody.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
        }
        playerBody.velocity = Vector2.up * jumpForce;
        jumpsRemaining--;
        jumped = true;
        
        
    }

    private bool IsGrounded()
    {
        groundCast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.1f,
            groundMask);
        //Debug.DrawRay(boxCol2D.bounds.center + new Vector3(boxCol2D.bounds.extents.x, 0f), Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);
        //Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x, 0f), Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);
        //Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x, boxCol2D.bounds.extents.y + 0.01f), Vector2.right * boxCol2D.bounds.size.x, Color.red);
        return groundCast.collider != null;
    }
    
    private bool IsOnWall()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, wallMask);
    }

    private void HandleGravity()
    {
        if (playerBody.velocity.y < 0)
        {
            playerBody.gravityScale = baseGravity * fallMultiplier;
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(playerBody.velocity.y, -maxFallSpeed));
        }
        else
        {
            playerBody.gravityScale = baseGravity;
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        
        float dashDirection = isFacingRight ? 1f : -1f;
        playerBody.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        Debug.Log(playerBody.velocity);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        
        playerBody.velocity = new Vector2(0f, playerBody.velocity.y);
        isDashing = false;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void HandleDrop()
    {
        if (Input.GetKeyDown(KeyCode.S) && IsGrounded() && isOnPlatform && playerCollider.enabled)
        {
            StartCoroutine(DropCoroutine());
        }
    }
    private IEnumerator DropCoroutine()
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(dropTime);
        playerCollider.enabled = true;
    }
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.PLATFORM_TAG))
        {
            isOnPlatform = true;
            Debug.Log("is on platform");
        }
    }
}
