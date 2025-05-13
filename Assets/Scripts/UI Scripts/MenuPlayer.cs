using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private float _floatAmplitude = 0.2f;
    [SerializeField] private float _floatFrequency = 1f;

    private Vector3 offset;
    private Vector3 targetPos;

    void Start()
    {
        offset = new Vector3(0.5f, 0.5f, 0f);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        targetPos = Camera.main.ScreenToWorldPoint(mousePos) + offset;
        
        targetPos.y += Mathf.Sin(Time.time * _floatFrequency) * _floatAmplitude;
        
        transform.position = Vector3.Lerp(transform.position, targetPos, _followSpeed * Time.deltaTime);
    }
}

