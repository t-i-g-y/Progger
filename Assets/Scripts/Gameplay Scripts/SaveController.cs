using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string _saveLocation;
    private PlayerInventoryController _inventoryController;
    private string _saveSlot = "slot1";
    private string SavePath => Path.Combine(Application.persistentDataPath, $"{_saveSlot}.json");
    
    void Start()
    {
        _saveLocation = SavePath;
        _inventoryController = FindObjectOfType<PlayerInventoryController>();
        Debug.Log(_saveLocation);
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform.position,
            playerHealth = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).GetComponent<PlayerHealth>().TotalHealth,
            unlockedAbilities = AbilityManager.Instance.GetUnlockedAbilities(),
            sqlInventory = _inventoryController.GetSQLInventoryData(),
            upgradeComponentIDs = _inventoryController.GetUpgradeComponentIDs(),
            collectedCodex = _inventoryController.GetCodexData(),
            savedModules = ModuleSaveHelper.SaveAllModules()
        };

        File.WriteAllText(SavePath, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (!File.Exists(_saveLocation))
        {
            SaveGame();
            return;
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));

        GameObject player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
        player.transform.position = saveData.playerPosition;
        player.GetComponent<PlayerHealth>().SetHealth(saveData.playerHealth);

        AbilityManager.Instance.LoadUnlockedAbilities(saveData.unlockedAbilities);

        _inventoryController.LoadInventoryFromSave(saveData);

        ModuleSaveHelper.LoadAllModules(saveData.savedModules);
    }
    
    public void SetSaveSlot(int slotIndex)
    {
        _saveSlot = $"slot{Mathf.Clamp(slotIndex, 1, 3)}";
        Debug.Log(_saveSlot);
    }
}
