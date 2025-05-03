using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    [SerializeField] private PlayerInventoryController _inventoryController;
    private PlayerItem nearbyItem;
    private float holdTime = 0.75f;
    private float holdTimer;

    private void Update()
    {
        if (nearbyItem == null) 
            return;

        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.unscaledDeltaTime;

            if (holdTimer >= holdTime)
            {
                _inventoryController.CollectItem(nearbyItem);
                holdTimer = 0f;
                nearbyItem = null;
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            holdTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.ITEM_TAG))
        {
            nearbyItem = other.GetComponent<PlayerItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.ITEM_TAG))
        {
            if (nearbyItem == other.GetComponent<PlayerItem>())
            {
                nearbyItem = null;
            }
        }
    }
}


