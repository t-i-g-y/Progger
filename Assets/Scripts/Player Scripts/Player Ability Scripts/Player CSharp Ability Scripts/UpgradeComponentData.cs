using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeComponentData
{
    public UpgradeType type;
    public float value;

    public UpgradeComponentData(UpgradeType type, float value)
    {
        this.type = type;
        this.value = value;
    }
}
