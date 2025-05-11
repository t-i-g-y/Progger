using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ModuleUIManager : MonoBehaviour
{
    private bool editorOpen;
    public static ModuleUIManager Instance;
    private Module currentModule;
    private List<GameObject> instantiatedPalette = new List<GameObject>();
    private GameObject gridHighlight;
    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _moduleCanvas;
    [SerializeField] private Transform _paletteParent;
    [SerializeField] private GameObject _draggableObject;
    [SerializeField] private GameObject _gridHighlightPrefab;
    
    public bool EditorOpen => editorOpen;
    public Module CurrentModule => currentModule;
    public Grid Grid => _grid;
    public Transform PaletteParent => _paletteParent;
    
    public GameObject GridHighlight
    {
        get
        {
            if (gridHighlight == null)
            {
                gridHighlight = Instantiate(_gridHighlightPrefab, currentModule.AvailableAreas[0].transform);
                gridHighlight.SetActive(false);
            }
            return gridHighlight;
        }
        set => gridHighlight = value;
    }
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
        _moduleCanvas.SetActive(true);
        currentModule = module;
        editorOpen = true;
        ClearPalette();
        module.AssignPointerBlockIDs();
        PopulatePalette(module.AvailableObjects.FindAll(obj => !obj.IsPlaced));
    }

    public void CloseModuleEditor()
    {
        currentModule.ExitModuleMode();
        ClearPalette();
        _moduleCanvas.SetActive(false);
        editorOpen = false;
        //Destroy(gridHighlight);
    }

    private void PopulatePalette(List<ModuleObject> prefabs)
    {
        foreach (var prefab in prefabs)
        {
            GameObject uiElement = Instantiate(_draggableObject, _paletteParent);
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
        currentModule.ResetPointerBlocks();
        currentModule.OccupiedTiles.Clear();
        ClearPalette();
        currentModule.PointerBlocks.Clear();
        PopulatePalette(currentModule.AvailableObjects);
    }
    
    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3Int cellPos = _grid.WorldToCell(position);
        Vector3 snappedPos = _grid.GetCellCenterWorld(cellPos);
        return snappedPos;
    }
}
