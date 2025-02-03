using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    private bool canMove = false;
    [SerializeField] private float minX, maxX;

    private void Update()
    {
        RestrictMovement();
    }

    private void RestrictMovement()
    {
        Vector3 tmpPos = transform.position;
        tmpPos.x = transform.position.x;
        tmpPos.y = transform.position.y;
        if (tmpPos.x < minX)
            tmpPos.x = minX;
        if (tmpPos.x > maxX)
            tmpPos.x = maxX;

        transform.position = tmpPos;
    }
}
