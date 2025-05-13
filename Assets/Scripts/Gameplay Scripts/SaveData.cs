using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public float playerHealth;
    public List<ProgrammingLanguage> unlockedAbilities;

    public List<InventorySaveData> sqlInventory;
    public List<int> upgradeComponentIDs;
    public List<CodexEntryData> collectedCodex;

    public List<ModuleSaveData> savedModules;
}
