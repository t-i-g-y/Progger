using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeSlot : MonoBehaviour, IDropHandler
{
    public UpgradeComponent currentUpgrade;

    public void OnDrop(PointerEventData eventData)
    {
        UpgradeComponent upgrade = eventData.pointerDrag.GetComponent<UpgradeComponent>();

        if (upgrade != null)
        {
            if (currentUpgrade != null && currentUpgrade != upgrade)
            {
                Destroy(currentUpgrade.gameObject);
            }

            upgrade.transform.SetParent(transform);
            upgrade.transform.localPosition = Vector3.zero;
            upgrade.originalParent = transform;
            currentUpgrade = upgrade;

            PlayerUpgradeManager.Instance.ApplyUpgrade(upgrade.type, upgrade.value);
        }
    }
}


