using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController _inventoryController;

    private void Start()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.ITEM_TAG))
        {
            PlayerItem item = other.GetComponent<PlayerItem>();
            if (item != null)
            {
                bool itemAdded = _inventoryController.AddItem(other.gameObject);
                if (itemAdded)
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
