using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    [SerializeField] private float _holdTime = 0.75f;
    private float holdTimer;
    private PlayerItem nearbyPlayerItem;
    private UpgradeComponentItem nearbyUpgradeComponentItem;
    
    private void Update()
    {
        if (nearbyPlayerItem == null && nearbyUpgradeComponentItem == null) 
            return;

        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.unscaledDeltaTime;

            if (holdTimer >= _holdTime)
            {
                if (nearbyUpgradeComponentItem != null)
                {
                    PlayerInventoryController.Instance.CollectUpgradeComponentItem(nearbyUpgradeComponentItem);
                    nearbyUpgradeComponentItem = null;
                }
                else if (nearbyPlayerItem != null)
                {
                    PlayerInventoryController.Instance.CollectPlayerItem(nearbyPlayerItem);
                    nearbyPlayerItem = null;
                }
                holdTimer = 0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            holdTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.COMPONENT_ITEM_TAG))
        {
            nearbyUpgradeComponentItem = other.GetComponent<UpgradeComponentItem>();
        }
        else if (other.CompareTag(TagManager.ITEM_TAG))
        {
            nearbyPlayerItem = other.GetComponent<PlayerItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.COMPONENT_ITEM_TAG))
        {
            if (nearbyUpgradeComponentItem == other.GetComponent<UpgradeComponentItem>())
            {
                nearbyUpgradeComponentItem = null;
            }
        }
        else if (other.CompareTag(TagManager.ITEM_TAG))
        {
            if (nearbyPlayerItem == other.GetComponent<PlayerItem>())
            {
                nearbyPlayerItem = null;
            }
        }
    }
}


