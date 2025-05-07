using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeSlot : MonoBehaviour, IDropHandler
{
    public UpgradeComponent currentUpgrade;

    public void OnDrop(PointerEventData eventData)
    {
        UpgradeComponent incoming = eventData.pointerDrag.GetComponent<UpgradeComponent>();
        if (incoming == null)
            return;

        UpgradeComponent outgoing = currentUpgrade;
        incoming.transform.SetParent(transform, false);
        incoming.transform.localPosition = Vector3.zero;
        currentUpgrade = incoming;
        UpgradeComponentManager.Instance.AssignComponent(incoming.ComponentData);
        
        if (outgoing != null)
        {
            //ClearSlot();
            UpgradeComponentManager.Instance.UnassignComponent(outgoing.ComponentData);
            UpgradeComponentManager.Instance.ReturnToComponentList(outgoing);
        }
    }

    public void ClearSlot()
    {
        UpgradeComponentManager.Instance.UnassignComponent(currentUpgrade.ComponentData);
        UpgradeComponentManager.Instance.ReturnToComponentList(currentUpgrade);
        currentUpgrade = null;
    }

}


