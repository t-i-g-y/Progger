using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private PlayerInventoryController _inventoryController;
    private bool inventoryUnlocked = false;
    private void Start()
    {
        //_inventoryController = FindObjectOfType<PlayerInventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.ITEM_TAG))
        {
            if (inventoryUnlocked)
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
            SpeedItem speedItem = other.GetComponent<SpeedItem>();
            if (speedItem != null)
            {
                speedItem.Collect();
            }
        }
    }
}
