using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string _saveLocation;

    private PlayerInventoryController _inventoryController;
    // Start is called before the first frame update
    void Start()
    {
        _saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        _inventoryController = FindObjectOfType<PlayerInventoryController>();
        Debug.Log(_saveLocation);
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform.position,
            //inventorySaveData = _inventoryController.GetInventoryItems()
        };
        
        File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform.position = saveData.playerPosition;
            //_inventoryController.SetInventoryItems(saveData.inventorySaveData);
        }
        else
        {
            SaveGame();
        }
    }
}
