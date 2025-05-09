using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleObject
{
    [Header("General")]
    public GameObject Prefab;
    public bool IsPlaced;
    public bool IsMultipart;

    [Header("PointerBlock")] 
    public bool IsPointer;
    public int SourceID;
    public int TargetID;
}
