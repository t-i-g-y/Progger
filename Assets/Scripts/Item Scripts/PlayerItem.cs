using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public static int GlobalID = 0;
    public int PrefabID;
    public int ID { get; private set; }
    public string ItemName;
    public ItemEffectType EffectType;
    public float EffectValue;
    [HideInInspector] public SpriteRenderer ItemIcon;

    private void Awake()
    {
        ItemIcon = GetComponent<SpriteRenderer>();
        ID = ++GlobalID;
    }

    public override string ToString() => $"{ID}: {ItemName}|{EffectType}|{EffectValue}";
}

