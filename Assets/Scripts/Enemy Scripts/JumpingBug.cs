using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JumpingBug : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _chaseSpeed = 2f;
    [SerializeField] private float _jumpForce = 2f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, _groundLayer);
        float direction = Mathf.Sign(_player.position.x - transform.position.x);
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << _player.gameObject.layer);

        if (isGrounded)
        {
            rb.velocity = new Vector2(direction * _chaseSpeed, rb.velocity.y);

            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, _groundLayer);
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, _groundLayer);
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, _groundLayer);

            if (!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if (isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            Vector2 direction = (_player.position - transform.position).normalized;
            Vector2 jumpDirection = direction * _jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, _jumpForce));
        }
    }
}
