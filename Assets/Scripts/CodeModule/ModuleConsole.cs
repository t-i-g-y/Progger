using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleConsole : MonoBehaviour
{
    [SerializeField] private GameObject[] codeBlocks;
    [SerializeField] private ProgrammingLanguage[] languages;
    private bool isUsable;

    private void Update()
    {
        if (isUsable && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Console opened");
            // Codeblock controls
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isUsable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isUsable = false;
        }
    }
}
