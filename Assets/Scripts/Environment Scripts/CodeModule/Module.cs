using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Module : MonoBehaviour
{
    [SerializeField] private ModuleConsole _console;
    [SerializeField] private Vector3 _cameraFocus;
    [SerializeField] private float _cameraZoom;
    [SerializeField] private Transform _placedObjectsParent;
    [SerializeField] private List<ModuleObject> _availableObjects;
    [SerializeField] private List<ModuleCodeableArea> _availableAreas;
    [SerializeField] private float _panTime = 0.5f;
    private List<PointerBlock> pointerBlocks = new List<PointerBlock>();
    private PointerBlock firstPointerBlock;
    private int currentTargetID;
    
    private Camera mainCamera;
    private CameraFollow mainCameraFollow;
    private float originalCameraZoom;

    public Transform PlacedObjectsParent
    {
        get => _placedObjectsParent;
    }
    
    public List<ModuleObject> AvailableObjects
    {
        get => _availableObjects;
    }

    public List<ModuleCodeableArea> AvailableAreas
    {
        get => _availableAreas;
    }

    public List<PointerBlock> PointerBlocks
    {
        get => pointerBlocks;
        set => pointerBlocks = value;
    }
    
    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraFollow = mainCamera.GetComponent<CameraFollow>();
        currentTargetID = -1;
    }

    public void EnterModuleMode()
    {
        Time.timeScale = 0f;
        originalCameraZoom = mainCamera.orthographicSize;
        if (mainCameraFollow != null)
        {
            mainCameraFollow.IsEnabled = false;
        }

        foreach (var area in _availableAreas)
        {
            area.ShowCodeableArea();
        }
        
        StartCoroutine(CameraPanCoroutine());
        ModuleUIManager.Instance.OpenModuleEditor(this);
    }

    public void ExitModuleMode()
    {
        Time.timeScale = 1f;
        if (mainCameraFollow != null)
        {
            mainCameraFollow.IsEnabled = true;
            mainCamera.orthographicSize = originalCameraZoom;
        }
        
        foreach (var area in _availableAreas)
        {
            area.HideCodeableArea();
        }
        
        //ModuleUIManager.Instance.CloseModuleEditor();
    }

    private IEnumerator CameraPanCoroutine()
    {
        float elapsedTime = 0f;
        float startZoom = mainCamera.orthographicSize;
        Vector3 startPosition = mainCamera.transform.position;
        while (elapsedTime < _panTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainCamera.transform.position = Vector3.Lerp(startPosition, new Vector3(_cameraFocus.x, _cameraFocus.y, startPosition.z), elapsedTime / _panTime);
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, _cameraZoom, elapsedTime / _panTime);
            
            yield return null;
        }
    }
    
    public void AssignPointerBlockIDs()
    {
        List<ModuleObject> futurePointerBlocks = new();
        for (int i = 0; i < _availableObjects.Count; i++)
        {
            if (_availableObjects[i].IsPlaced) 
                return;

            PointerBlock pb = _availableObjects[i].Prefab.GetComponent<PointerBlock>();
            if (pb != null)
            {
                _availableObjects[i].IsPointer = true;
                futurePointerBlocks.Add(_availableObjects[i]);
                _availableObjects.Remove(_availableObjects[i]);
                i = -1;
            }
        }
        
        if (futurePointerBlocks.Count < 1) 
            return;
        
        HashSet<int> uniqueIDs = new();
        while (uniqueIDs.Count < futurePointerBlocks.Count + 1)
        {
            uniqueIDs.Add(UnityEngine.Random.Range(0, 100));
        }

        List<int> idList = new(uniqueIDs);
        
        for (int i = 0; i < futurePointerBlocks.Count; i++)
        {
            futurePointerBlocks[i].SourceID = idList[i];
            futurePointerBlocks[i].TargetID = idList[i + 1];
        }
        ShuffleList(futurePointerBlocks);
        _availableObjects.AddRange(futurePointerBlocks);
    }
    
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public bool CanChainBounce(PointerBlock pointer) 
    {
        if (currentTargetID == -1)
        {
            currentTargetID = pointer.TargetID;
            return true;
        }
        if (currentTargetID == pointer.SourceID)
        {
            currentTargetID = pointer.TargetID;
            return true;
        }
        return false;
    }
}

