using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool jumped;
    private float jumpForce = 5f;
    private float horizontalMovement;
    
    private BoxCollider2D boxCol2D;
    private Rigidbody2D myBody;
    private Vector3 movePos;
    private RaycastHit2D groundCast;
    private PlayerAnimation playerAnimation;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float normalJumpForce = 5f;
    [SerializeField] private LayerMask groundMask;

    private void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
        myBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw(TagManager.HORIZONTAL_MOVEMENT);
        HandleJumping();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
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
        IsGrounded();
        if (Input.GetButtonDown(TagManager.JUMP_BUTTON))
        {
            Debug.Log("Jump pressed");
            if (IsGrounded())
            {
                jumpForce = normalJumpForce;
                Jump();
            }
        }
    }

    private void Jump()
    {
        myBody.velocity = Vector2.up * jumpForce;
        jumped = true;
    }

    private bool IsGrounded()
    {
        groundCast = Physics2D.BoxCast(boxCol2D.bounds.center, boxCol2D.bounds.size, 0f, Vector2.down, 0.1f,
            groundMask);
        Debug.DrawRay(boxCol2D.bounds.center + new Vector3(boxCol2D.bounds.extents.x, 0f), Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);
        Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x, 0f), Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);
        Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x, boxCol2D.bounds.extents.y + 0.01f), Vector2.right * boxCol2D.bounds.size.x, Color.red);
        return groundCast.collider != null;
    }
}
