using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCodeableArea : MonoBehaviour
{
    [SerializeField] private Rect areaBounds;
    [SerializeField] private GameObject codeableAreaPrefab;
    private GameObject spawnedPrefab;
    
    private void Start()
    {
        if (codeableAreaPrefab != null)
        {
            spawnedPrefab = Instantiate(codeableAreaPrefab, transform);
            spawnedPrefab.transform.localPosition = Vector3.zero;
            spawnedPrefab.transform.localScale = new Vector3(areaBounds.width, areaBounds.height, 1f);
            //spawnedPrefab.SetActive(false);
        }
    }

    public bool IsInside(Vector3 position)
    {
        Vector2 center = transform.position;
        Rect relativeRect = new Rect(center - areaBounds.size / 2f, areaBounds.size);
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
        Gizmos.DrawWireCube(transform.position, new Vector3(areaBounds.width, areaBounds.height, 0));
    }
}
