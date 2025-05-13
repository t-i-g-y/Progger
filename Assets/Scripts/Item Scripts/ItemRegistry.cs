using System.Collections.Generic;
using UnityEngine;

public class ItemRegistry : MonoBehaviour
{
    public static ItemRegistry Instance;

    [SerializeField] private List<ItemEntry> registeredItems = new();

    private Dictionary<int, GameObject> itemLookup = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (var item in registeredItems)
                itemLookup[item.itemID] = item.prefab;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject GetPlayerItemByID(int id)
    {
        return Instance.itemLookup.TryGetValue(id, out var prefab) ? prefab : null;
    }
}

[System.Serializable]
public class ItemEntry
{
    public int itemID;
    public GameObject prefab;
}