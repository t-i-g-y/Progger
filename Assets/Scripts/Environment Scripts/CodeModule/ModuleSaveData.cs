using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleSaveData
{
    public string moduleName;
    public List<PlacedObjectData> placedObjects;
}

[System.Serializable]
public class PlacedObjectData
{
    public Vector3 position;
    public int prefabID;
}
