using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    private bool isEnabled = true;
    private Transform player;
    private Transform target;
    private Vector3 tmpPos;

    private bool isEditorMode;
    private Vector3 editorCamTarget;
    private Vector3 velocity = Vector3.zero;

    [Header("PlayerCamera")]
    [SerializeField] private float _minX, _maxX;
    [SerializeField] private float _minY, _maxY;
    private float lastMinX, lastMaxX, lastMinY, lastMaxY;

    [Header("EditorCamera")]
    [SerializeField] private bool _followMouseInEditor = true;
    [SerializeField] private float _mouseFollowSpeed = 5f;
    [SerializeField] private float _keyboardPanSpeed = 2f;
    [SerializeField] private float _smoothTime = 0.2f;
    [SerializeField] private float _mouseDeadZoneRadius = 0.5f;
    [SerializeField] private float _entryDelay = 0.5f;
    private float entryTimer;

    public bool IsEnabled
    {
        get => isEnabled;
        set => isEnabled = value;
    }

    private void Awake()
    {
        player = GameObject.FindWithTag(TagManager.PLAYER_TAG)?.transform;
        target = player;

        lastMinX = _minX;
        lastMaxX = _maxX;
        lastMinY = _minY;
        lastMaxY = _maxY;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void EnterEditorMode(Rect moduleBounds)
    {
        isEditorMode = true;
        target = null;

        lastMinX = _minX;
        lastMaxX = _maxX;
        lastMinY = _minY;
        lastMaxY = _maxY;

        _minX = moduleBounds.xMin;
        _maxX = moduleBounds.xMax;
        _minY = moduleBounds.yMin;
        _maxY = moduleBounds.yMax;

        entryTimer = _entryDelay;
        velocity = Vector3.zero;
    }

    public void ExitEditorMode()
    {
        isEditorMode = false;
        target = player;

        _minX = lastMinX;
        _maxX = lastMaxX;
        _minY = lastMinY;
        _maxY = lastMaxY;
    }

    private void LateUpdate()
    {
        if (!IsEnabled) return;

        if (isEditorMode && _followMouseInEditor)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 inputOffset = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"),
                0
            ) * _keyboardPanSpeed;

            editorCamTarget = Vector3.Lerp(transform.position, mouseWorld + inputOffset, Time.unscaledDeltaTime * _mouseFollowSpeed);

            editorCamTarget.x = Mathf.Clamp(editorCamTarget.x, _minX, _maxX);
            editorCamTarget.y = Mathf.Clamp(editorCamTarget.y, _minY, _maxY);
            editorCamTarget.z = transform.position.z;

            transform.position = editorCamTarget;
        }
        else if (target)
        {
            tmpPos = transform.position;
            tmpPos.x = Mathf.Clamp(target.position.x, _minX, _maxX);
            tmpPos.y = Mathf.Clamp(target.position.y, _minY, _maxY);
            transform.position = tmpPos;
        }
    }
}

