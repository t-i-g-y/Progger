using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private BoxCollider2D boxCol2D;
    private Rigidbody2D myBody;
    private Transform myTransform;
    private PlayerAnimation playerAnimation;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
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

    [Header("Gravity")] 
    [SerializeField] private float baseGravity = 2;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float fallMultiplier = 2.5f;

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
    
    public int MaxJumps
    {
        get => maxJumps;
        set => maxJumps = value;
    }
    
    private void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
        myBody = GetComponent<Rigidbody2D>();
        //playerAnimation = GetComponent<PlayerAnimation>();
        myTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        
        HandleJumping();
        HandleGravity();
        
        HandleWallSlide();
        HandleWallJump();
        if (!isWallJumping)
        {
            horizontalMovement = Input.GetAxisRaw(TagManager.HORIZONTAL_MOVEMENT);
            Flip();
        }
        
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
            HandleMovement();
    }

    private void HandleMovement()
    {
        if (horizontalMovement > 0)
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        else if (horizontalMovement < 0)
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        else
            myBody.velocity = new Vector2(0f, myBody.velocity.y);
    }

    private void HandleAnimation()
    {
        playerAnimation.ChangeDirection((int)myBody.velocity.x);
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
                myBody.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
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
            myBody.velocity = new Vector2(myBody.velocity.x, Mathf.Max(myBody.velocity.y, -wallSlideSpeed));
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

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Jump()
    {
        if (wallJumpTimer > 0f)
        {
            isWallJumping = true;
            myBody.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
        }
        myBody.velocity = Vector2.up * jumpForce;
        jumpsRemaining--;
        jumped = true;
        
        
    }

    private bool IsGrounded()
    {
        groundCast = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size, 0f, Vector2.down, 0.1f,
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
        if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = baseGravity * fallMultiplier;
            myBody.velocity = new Vector2(myBody.velocity.x, Mathf.Max(myBody.velocity.y, -maxFallSpeed));
        }
        else
        {
            myBody.gravityScale = baseGravity;
        }
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
}
