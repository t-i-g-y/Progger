using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleUIManager : MonoBehaviour
{
    private bool editorOpen;
    public static ModuleUIManager Instance;
    private Module currentModule;
    private List<GameObject> instantiatedPalette = new List<GameObject>();
    
    [SerializeField] private GameObject moduleCanvas;
    [SerializeField] private Transform paletteParent;
    [SerializeField] private GameObject draggableObject;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (editorOpen && Input.GetKeyDown(KeyCode.R))
        {
            ResetModuleObjects();
        }
    }

    public void OpenModuleEditor(Module module)
    {
        moduleCanvas.SetActive(true);
        currentModule = module;
        editorOpen = true;
        ClearPalette();
        PopulatePalette(module.AvailableObjects.FindAll(obj => !obj.IsPlaced));
    }

    public void CloseModuleEditor()
    {
        ClearPalette();
        moduleCanvas.SetActive(false);
        editorOpen = false;
    }

    private void PopulatePalette(List<ModuleObject> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            GameObject uiElement = Instantiate(draggableObject, paletteParent);
            ModuleDraggable draggable = uiElement.GetComponent<ModuleDraggable>();
            draggable.Initialize(prefab, currentModule);
            instantiatedPalette.Add(uiElement);
        }
    }

    private void ClearPalette()
    {
        foreach (var element in instantiatedPalette)
        {
            Destroy(element);
        }
        instantiatedPalette.Clear();
    }

    public void ResetModuleObjects()
    {
        foreach (var obj in currentModule.AvailableObjects)
        {
            obj.IsPlaced = false;
        }

        foreach (Transform child in currentModule.PlacedObjectsParent)
        {
            Destroy(child.gameObject);
        }
        ClearPalette();
        PopulatePalette(currentModule.AvailableObjects);
    }
}
