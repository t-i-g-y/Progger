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
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int slotIndex = i + 1;
            string path = GetSlotPath(slotIndex);

            if (File.Exists(path))
            {
                string timestamp = File.GetLastWriteTime(path).ToString("g");
                slots[i].statusText.text = $"Saved: {timestamp}";
            }
            else
            {
                slots[i].statusText.text = "Empty";
            }
        }
    }

    private string GetSlotPath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"slot{slotIndex}.json");
    }
}
