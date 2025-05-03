using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleConsole : MonoBehaviour
{
    private bool isUsable;
    [SerializeField] private Module linkedModule;
    
    private void Update()
    {
        if (isUsable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Console opened");
                linkedModule.EnterModuleMode();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Console exited");
                linkedModule.ExitModuleMode();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            isUsable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            isUsable = false;
        }
    }
}
