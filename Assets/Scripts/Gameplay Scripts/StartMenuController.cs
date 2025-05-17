using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] SaveController _saveController;
    public void OnStartClick()
    {
        int recentSlot = -1;
        System.DateTime lastTime = System.DateTime.MinValue;

        for (int i = 1; i <= 3; i++)
        {
            string path = Path.Combine(Application.persistentDataPath, $"slot{i}.json");
            if (File.Exists(path))
            {
                var time = File.GetLastWriteTime(path);
                if (time > lastTime)
                {
                    lastTime = time;
                    recentSlot = i;
                }
            }
        }

        if (recentSlot != -1)
        {
            _saveController.SetSaveSlot(recentSlot);
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogWarning("No valid save slot found.");
        }
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
