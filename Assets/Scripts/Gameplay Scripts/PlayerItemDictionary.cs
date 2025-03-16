using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDictionary : MonoBehaviour
{
    public List<PlayerItem> itemPrefabs;
    private Dictionary<int, GameObject> playerItemDictionary;

    private void Awake()
    {
        playerItemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }
        }

        foreach (PlayerItem item in itemPrefabs)
        {
            playerItemDictionary[item.ID] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int ID)
    {
        playerItemDictionary.TryGetValue(ID, out GameObject prefab);

        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID:{ID} not found in dictionary");
        }

        return prefab;
    }
}
