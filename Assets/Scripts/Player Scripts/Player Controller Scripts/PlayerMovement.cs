using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private BoxCollider2D playerCollider;
    private CircleCollider2D collectorCollider;
    private Rigidbody2D playerBody;
    private Transform playerTransform;
    private PlayerAnimation playerAnimation;
    [HideInInspector] public static bool InputBlocked;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    private float speedMultiplier = 1f;
    private float horizontalMovement;
    private Vector3 movePos;
    
    [Header("Jumping")]
    [SerializeField] private float _normalJumpForce = 8f;
    [SerializeField] private float _doubleJumpForce = 6f;
    private bool jumped;
    private float jumpForce = 5f;
    private int maxJumps = 1;
    private int jumpsRemaining;
    private float jumpMultiplier = 1f;
    
    [Header("Long Jumping")]
    [SerializeField] private float _longJumpChargeTime = 1f;
    [SerializeField] private float _longJumpForce = 10f;
    private float jumpPressDuration = 0f;
    private bool isChargingJump;
    
    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundMask;
    private RaycastHit2D groundCast;
    
    [Header("Dropping")] 
    [SerializeField] private float _dropTime = 0.25f;
    private bool isOnPlatform;
    
    [Header("Gravity")] 
    [SerializeField] private float _baseGravity = 2;
    [SerializeField] private float _maxFallSpeed = 15f;
    [SerializeField] private float _fallMultiplier = 2f;
    
    [Header("Dashing")] 
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashDuration = 0.1f;
    [SerializeField] private float _dashCooldown = 1f;
    private bool isDashing;
    private bool canDash;
    private TrailRenderer trailRenderer;
    
    [Header("Wall Movement")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 0.05f);
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private float _wallSlideSpeed = 2f;
    [SerializeField] private float _wallJumpTime = 0.5f;
    [SerializeField] private Vector2 _wallJumpPower = new Vector2(5f, 10f);
    private bool isFacingRight = true;
    private bool isWallSliding;
    private RaycastHit2D wallCast;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTimer;
    
    [Header("Climbing")] 
    [SerializeField] private float _climbSpeed = 5f;
    [SerializeField] private Transform _topCheck;
    [SerializeField] private float _topCheckRadius = 0.1f;
    private bool canClimb;
    private bool isClimbing;
    
    //[Header("Debugging")]
    private float jumpPressTimestamp;
    private float jumpExecutionTimestamp;

    public float JumpMultiplier
    {
        get => jumpMultiplier;
        set
        {
            jumpMultiplier = value;
            if (jumpMultiplier < 1f)
            {
                jumpMultiplier = 1f;
            }
        }
    }
    public float SpeedMultiplier
    {
        get => speedMultiplier;
        set
        {
            speedMultiplier = value;
            if (speedMultiplier < 1f)
            {
                speedMultiplier = 1f;
            }
        }
    }
    
    public int MaxJumps
    {
        get => maxJumps;
        set
        {
            maxJumps = value;
            if (maxJumps < 1)
            {
                maxJumps = 1;
            }
        }
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
        //SpeedItem.OnSpeedCollected += StartSpeedBoost;
    }

    private void Update()
    {
        if (InputBlocked || Time.timeScale == 0f)
            return;
        if (isDashing)
            return;
        HandleClimbing();
        HandleJumping();
        HandleGravity();
        if (AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.Python))
        {
            HandleWallSlide();
            HandleWallJump(); 
        }
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
    
    private void HandleMovement()
    {
        if (horizontalMovement > 0)
            playerBody.velocity = new Vector2(_moveSpeed * speedMultiplier, playerBody.velocity.y);
        else if (horizontalMovement < 0)
            playerBody.velocity = new Vector2(-_moveSpeed * speedMultiplier, playerBody.velocity.y);
        else
            playerBody.velocity = new Vector2(0f, playerBody.velocity.y);
    }
    
    public void StartSpeedBoost(float multiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier));
    }
    
    private IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        speedMultiplier += multiplier;
        yield return new WaitForSeconds(2f);
        speedMultiplier = 1f;
    }
    
    private void HandleJumping()
    {
        if (isClimbing)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpPressTimestamp = Time.time;
            isChargingJump = true;
            jumpPressDuration = 0f;
            jumpsRemaining = maxJumps;
            jumped = true;
        }

        if (isChargingJump && Input.GetKey(KeyCode.Space))
        {
            jumpPressDuration += Time.deltaTime;
        }

        if (isChargingJump && Input.GetKeyUp(KeyCode.Space))
        {
            isChargingJump = false;

            float force;

            if (AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.BASIC))
            {
                float chargeRatio = Mathf.Clamp01(jumpPressDuration / _longJumpChargeTime);
                force = Mathf.Lerp(_normalJumpForce, _longJumpForce, chargeRatio);
            }
            else
            {
                force = _normalJumpForce;
            }

            jumpForce = force;
            Jump();
        }

        if (!IsGrounded() && Input.GetKeyDown(KeyCode.Space) && jumped && jumpsRemaining > 0 
            && AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.CPlusPlus))
        {
            jumpForce = _doubleJumpForce;
            Jump();
        }

        if (wallJumpTimer > 0f)
        {
            isWallJumping = true;
            playerBody.velocity = new Vector2(wallJumpDirection * _wallJumpPower.x, _wallJumpPower.y);
            wallJumpTimer = 0;

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(CancelWallJump), _wallJumpTime + 0.1f);
        }
    }
    
    private void Jump()
    {
        float startY = transform.position.y;
        
        if (wallJumpTimer > 0f)
        {
            isWallJumping = true;
            playerBody.velocity = new Vector2(wallJumpDirection * _wallJumpPower.x, _wallJumpPower.y);
        }
        playerBody.velocity = Vector2.up * (jumpMultiplier * jumpForce);
        StartCoroutine(LogJumpHeight(startY));
        jumpExecutionTimestamp = Time.time;
        Debug.Log($"Jump response time: {(jumpExecutionTimestamp - jumpPressTimestamp)} seconds");
        jumpsRemaining--;
        jumped = true;
    }
    
    private IEnumerator LogJumpHeight(float startY)
    {
        float maxY = startY;
        yield return new WaitUntil(() => !IsGrounded());
        while (!IsGrounded())
        {
            if (transform.position.y > maxY)
                maxY = transform.position.y;
            yield return null;
        }
        
        float height = maxY - startY;
        Debug.Log($"Jump height: {height:F2}");
    }
    
    public void StartJumpBoost(float multiplier)
    {
        StartCoroutine(JumpBoostCoroutine(multiplier));
    }
    
    private IEnumerator JumpBoostCoroutine(float multiplier)
    {
        jumpMultiplier += multiplier;
        yield return new WaitForSeconds(2f);
        jumpMultiplier = 1f;
    }
    
    private bool IsGrounded()
    {
        groundCast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.1f,
            _groundMask);
        //Debug.DrawRay(boxCol2D.bounds.center + new Vector3(boxCol2D.bounds.extents.x, 0f), Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);
        //Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x, 0f), Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);
        //Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x, boxCol2D.bounds.extents.y + 0.01f), Vector2.right * boxCol2D.bounds.size.x, Color.red);
        return groundCast.collider != null;
    }
    
    private void HandleDrop()
    {
        if (Input.GetKeyDown(KeyCode.S) && IsGrounded() && isOnPlatform)
        {
            StartCoroutine(DropCoroutine());
        }
    }
    private IEnumerator DropCoroutine()
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(_dropTime);
        playerCollider.enabled = true;
    }
    
    private void HandleGravity()
    {
        if (isClimbing)
        {
            playerBody.gravityScale = 0f;
        }
        else if (playerBody.velocity.y < 0)
        {
            playerBody.gravityScale = _baseGravity * _fallMultiplier;
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(playerBody.velocity.y, -_maxFallSpeed));
        }
        else
        {
            playerBody.gravityScale = _baseGravity;
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
    
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        
        float dashDirection = isFacingRight ? 1f : -1f;
        playerBody.velocity = new Vector2(transform.localScale.x * _dashSpeed, 0);
        Debug.Log(playerBody.velocity);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(_dashDuration);
        
        playerBody.velocity = new Vector2(0f, playerBody.velocity.y);
        isDashing = false;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(_dashCooldown);
        canDash = true;
    }
    
    private void HandleWallSlide()
    {
        if (!IsGrounded() && IsOnWall() && horizontalMovement != 0)
        {
            Debug.Log("Is on wall");
            isWallSliding = true;
            playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Max(playerBody.velocity.y, -_wallSlideSpeed));
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
            wallJumpTimer = _wallJumpTime;
            
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }
    
    private void CancelWallJump()
    {
        isWallJumping = false;
    }
    
    private bool IsOnWall()
    {
        return Physics2D.OverlapBox(_wallCheck.position, _wallCheckSize, 0f, _wallMask);
    }
    
    private void HandleAnimation()
    {
        playerAnimation.ChangeDirection((int)playerBody.velocity.x);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_wallCheck.position, _wallCheckSize);
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

    private void HandleClimbing()
    {
        
        if (canClimb && (Input.GetButton(TagManager.JUMP_BUTTON) || Input.GetKey(KeyCode.W)))
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, _climbSpeed);
            isClimbing = true;
        }
        else if (canClimb && (Input.GetKey(KeyCode.S)))
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, -_climbSpeed);
            isClimbing = true;
        }
        else if (canClimb)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.PLATFORM_TAG))
        {
            isOnPlatform = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TagManager.CLIMBABLE_TAG))
        {
            canClimb = true;
            Debug.Log("Can climb");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TagManager.CLIMBABLE_TAG))
        {
            canClimb = false;
            Debug.Log("Cannot climb");
        }
    }
}
