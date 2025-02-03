using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 tmpPos;

    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;

    private void Awake()
    {
        player = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
    }

    private void LateUpdate()
    {
        if (!player)
            return;

        tmpPos = transform.position;
        tmpPos.x = player.position.x;
        tmpPos.y = player.position.y;

        if (tmpPos.x < minX)
            tmpPos.x = minX;
        if (tmpPos.x > maxX)
            tmpPos.x = maxX;
        if (tmpPos.y < minY)
            tmpPos.y = minY;
        if (tmpPos.y > maxY)
            tmpPos.y = maxY;

        transform.position = tmpPos;
    }
}
