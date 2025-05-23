using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public static int GlobalID = 0;
    public int PrefabID;
    public int ID;
    public string ItemName;
    [HideInInspector] public SpriteRenderer ItemIcon;
    public ItemEffectType EffectType;
    public float EffectValue;
    private void Awake()
    {
        ItemIcon = GetComponent<SpriteRenderer>();
        ID = ++GlobalID;
    }
    public override string ToString() => $"{ID}: {ItemName}|{EffectType}|{EffectValue}";
}

