using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSnakeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject snakeSegmentPrefab;
    [SerializeField] private int maxSegments = 12;
    [SerializeField] private float segmentHeight = 0.5f;
    [SerializeField] private float growDelay = 0.1f;
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private LayerMask ceilingMask;

    private bool isOnCooldown;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isOnCooldown)
        {
            StartCoroutine(GrowSnake());
        }
    }

    private IEnumerator GrowSnake()
    {
        isOnCooldown = true;

        Vector2 spawnPosition = transform.position;

        for (int i = 0; i < maxSegments; i++)
        {
            Vector2 nextPos = spawnPosition + Vector2.up * (i * segmentHeight);
            if (Physics2D.OverlapCircle(nextPos, 0.2f, ceilingMask))
                break;

            Instantiate(snakeSegmentPrefab, nextPos, Quaternion.identity);
            yield return new WaitForSeconds(growDelay);
        }

        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}

