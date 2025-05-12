using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ModuleCodeableArea : MonoBehaviour
{
    [SerializeField] private Rect _areaBounds;
    [SerializeField] private GameObject _codeableAreaPrefab;
    private GameObject spawnedPrefab;
    
    private void Start()
    {
        if (_codeableAreaPrefab != null)
        {
            spawnedPrefab = Instantiate(_codeableAreaPrefab, transform);
            spawnedPrefab.transform.localPosition = Vector3.zero;
            spawnedPrefab.transform.localScale = new Vector3(_areaBounds.width, _areaBounds.height, 1f);
            //spawnedPrefab.SetActive(false);
        }
    }

    public bool IsInside(Vector3 position)
    {
        Vector2 center = transform.position;
        Rect relativeRect = new Rect(center - _areaBounds.size / 2f, _areaBounds.size);
        return relativeRect.Contains(position);
    }

    public void ShowCodeableArea()
    {
        if (spawnedPrefab != null)
        {
            spawnedPrefab.SetActive(true);
        }
    }

    public void HideCodeableArea()
    {
        if (spawnedPrefab != null)
        {
            spawnedPrefab.SetActive(false);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_areaBounds.width, _areaBounds.height, 0));
    }
}
