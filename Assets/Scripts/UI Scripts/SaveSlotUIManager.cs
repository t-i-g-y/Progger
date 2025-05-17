using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class SaveSlotUIManager : MonoBehaviour
{
    [SerializeField] private SlotUI[] _slots = new SlotUI[3];
    [SerializeField] private SaveController _saveController;
    [SerializeField] private bool _isStartMenu;
    private void Start()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            int slotIndex = i + 1;
            string path = GetSlotPath(slotIndex);

            if (!_isStartMenu)
            {
                _slots[i].saveButton.onClick.AddListener(() =>
                {
                    _saveController.SetSaveSlot(slotIndex);
                    _saveController.SaveGame();
                    RefreshUI();
                });

                _slots[i].loadButton.onClick.AddListener(() =>
                {
                    if (File.Exists(path))
                    {
                        _saveController.SetSaveSlot(slotIndex);
                        _saveController.LoadGame();
                    }
                });
            
                _slots[i].deleteButton.onClick.AddListener(() =>
                {
                    _saveController.SetSaveSlot(slotIndex);

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
            else
            {
                _slots[i].saveButton.onClick.AddListener(() =>
                {
                    _saveController.SetSaveSlot(slotIndex);

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
                    SceneManager.LoadScene("GameScene");
                });

                _slots[i].loadButton.onClick.AddListener(() =>
                {
                    if (File.Exists(path))
                    {
                        _saveController.SetSaveSlot(slotIndex);
                        SceneManager.LoadScene("GameScene");
                    }
                });
            }
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            int slotIndex = i + 1;
            string path = GetSlotPath(slotIndex);

            if (File.Exists(path))
            {
                string timestamp = File.GetLastWriteTime(path).ToString("g");
                SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

                _slots[i].statusText.text = IsEmpty(data) ? "ПУСТО" : $"СОХРАНЕНО: {timestamp}";
            }
            else
            {
                _slots[i].statusText.text = "ПУСТО";
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
