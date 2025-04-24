using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallWait = 2f;
    [SerializeField] private float destroyTime = 1f;
    private bool isFalling;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isFalling && other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            StartCoroutine(FallingPlatformCoroutine());
        }
    }

    private IEnumerator FallingPlatformCoroutine()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Destroy(gameObject, destroyTime);
    }
}
