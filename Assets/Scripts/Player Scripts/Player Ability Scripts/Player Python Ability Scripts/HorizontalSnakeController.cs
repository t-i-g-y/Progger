using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSnakeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float selfDestructTime = 1.5f;

    private float selfDestructTimer = 0f;
    private HorizontalSnakeManager manager;

    private void Start()
    {
        manager = FindObjectOfType<HorizontalSnakeManager>();
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
        HandleSelfDestruct();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // You can put your trigger interaction logic here later
            Debug.Log("Snake Interacted!");
        }
    }

    private void HandleSelfDestruct()
    {
        if (Input.GetKey(KeyCode.R))
        {
            selfDestructTimer += Time.deltaTime;

            if (selfDestructTimer >= selfDestructTime)
            {
                manager.ReturnToPlayer();
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            selfDestructTimer = 0f;
        }
    }
}
