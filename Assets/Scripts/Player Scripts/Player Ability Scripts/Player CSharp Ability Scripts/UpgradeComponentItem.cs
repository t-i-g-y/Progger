using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeComponentItem : MonoBehaviour
{
    public static int GlobalID = 0;
    public int PrefabID;
    public int ID;
    public string ItemName;
    [HideInInspector] public SpriteRenderer ItemIcon;
    public UpgradeComponentData componentData;
    
    private void Awake()
    {
        ItemIcon = GetComponent<SpriteRenderer>();
        ID = ++GlobalID;
    }
    
    public override string ToString() => $"UPGRADE COMPONENT{ID}: {ItemName}|{componentData.type}|{componentData.value}";
}
