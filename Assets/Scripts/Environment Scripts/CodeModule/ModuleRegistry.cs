using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleRegistry : MonoBehaviour
{
    public static ModuleRegistry Instance;

    public List<ModulePrefabEntry> entries = new();

    private Dictionary<int, GameObject> idToPrefab = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var entry in entries)
        {
            idToPrefab[entry.prefabID] = entry.prefab;
        }
    }

    public static GameObject GetPrefabByID(int id)
    {
        return Instance.idToPrefab.TryGetValue(id, out var prefab) ? prefab : null;
    }
}

[System.Serializable]
public class ModulePrefabEntry
{
    public int prefabID;
    public GameObject prefab;
}