using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCodeableArea : MonoBehaviour
{
    [SerializeField] private Rect areaBounds;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaBounds.width, areaBounds.height, 0));
    }

    public bool IsInside(Vector2 position)
    {
        Vector2 center = transform.position;
        Rect relativeRect = new Rect(center - areaBounds.size / 2f, areaBounds.size);
        return relativeRect.Contains(position);
    }
}
