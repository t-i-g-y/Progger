using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleUIManager : MonoBehaviour
{
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

    public void OpenModuleEditor(Module module)
    {
        moduleCanvas.SetActive(true);
        currentModule = module;
        PopulatePalette(module.AvailableObjects);
    }

    public void CLoseModuleEditor()
    {
        ClearPalette();
        moduleCanvas.SetActive(false);
    }

    private void PopulatePalette(List<ModuleObject> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            GameObject uiElement = Instantiate(draggableObject, paletteParent);
            ModuleDraggable draggable = uiElement.GetComponent<ModuleDraggable>();
            draggable.Intialize(prefab, currentModule);
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
}
