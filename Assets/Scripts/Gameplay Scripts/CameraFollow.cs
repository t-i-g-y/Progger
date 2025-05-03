using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool isEnabled = true;
    private Transform player;
    private Vector3 tmpPos;

    [SerializeField] private Transform target;
    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;

    public bool IsEnabled
    {
        get => isEnabled;
        set => isEnabled = value;
    }
    private void Awake()
    {
        player = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void LateUpdate()
    {
        if (!target || !IsEnabled)
            return;

        Vector3 tmpPos = transform.position;
        tmpPos.x = target.position.x;
        tmpPos.y = target.position.y;

        tmpPos.x = Mathf.Clamp(tmpPos.x, minX, maxX);
        tmpPos.y = Mathf.Clamp(tmpPos.y, minY, maxY);

        transform.position = tmpPos;
    }
}
