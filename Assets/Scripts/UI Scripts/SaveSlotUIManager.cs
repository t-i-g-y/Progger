using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveSlotUIManager : MonoBehaviour
{
    [SerializeField] private SlotUI[] slots = new SlotUI[3];
    [SerializeField] private SaveController saveController;
    
    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int slotIndex = i + 1;
            string path = GetSlotPath(slotIndex);

            slots[i].saveButton.onClick.AddListener(() =>
            {
                saveController.SetSaveSlot(slotIndex);
                saveController.SaveGame();
                RefreshUI();
            });

            slots[i].loadButton.onClick.AddListener(() =>
            {
                if (File.Exists(path))
                {
                    saveController.SetSaveSlot(slotIndex);
                    saveController.LoadGame();
                }
            });
            
            slots[i].deleteButton.onClick.AddListener(() =>
            {
                saveController.SetSaveSlot(slotIndex);

                SaveData emptyData = new SaveData
                {
                    playerPosition = Vector3.zero,
                    playerHealth = 3,
                    unlockedAbilities = new List<ProgrammingLanguage>(),
                    sqlInventory = new List<InventorySaveData>(),
                    upgradeComponentIDs = new List<int>(),
                    collectedCodex = new List<CodexEntryData>(),
                    savedModules = new List<ModuleSaveData>()
                };

                File.WriteAllText(GetSlotPath(slotIndex), JsonUtility.ToJson(emptyData));
                Debug.Log($"Slot {slotIndex} reset to default.");
                RefreshUI();
            });
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int slotIndex = i + 1;
            string path = GetSlotPath(slotIndex);

            if (File.Exists(path))
            {
                string timestamp = File.GetLastWriteTime(path).ToString("g");
                SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

                slots[i].statusText.text = IsEmpty(data) ? "ПУСТО" : $"СОХРАНЕНО: {timestamp}";
            }
            else
            {
                slots[i].statusText.text = "ПУСТО";
            }
        }
    }
    
    private string GetSlotPath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"slot{slotIndex}.json");
    }

    private bool IsEmpty(SaveData data)
    {
        bool playerStatCheck = data.playerPosition == Vector3.zero && data.playerHealth == 3;
        bool playerAbilityCheck = data.unlockedAbilities.Count == 0;
        bool inventoryCheck = data.sqlInventory.Count == 0 && data.upgradeComponentIDs.Count == 0 && data.collectedCodex.Count == 0;
        return playerStatCheck && playerAbilityCheck && inventoryCheck;
    }
}
