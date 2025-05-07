using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static int GlobalID = 0;
    public int PrefabID;
    public int ID;
    public string ItemName;
    [HideInInspector] public SpriteRenderer ItemIcon;

    private void Awake()
    {
        ItemIcon = GetComponent<SpriteRenderer>();
        ID = ++GlobalID;
    }
}
