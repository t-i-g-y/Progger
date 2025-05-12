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
    private Dictionary<Vector3, GameObject> _placedObjects = new Dictionary<Vector3, GameObject>();
    private HashSet<Vector3> occupiedTiles = new HashSet<Vector3>();
    private List<PointerBlock> pointerBlocks = new List<PointerBlock>();
    [SerializeField] private int currentTargetID;
    private int firstPointerBlockSourceID;
    private int lastPointerBlockSourceID;
    private float lastBounceTimer;
    [SerializeField] private float pointerChainResetTimer = 10f;
    
    private Camera mainCamera;
    private CameraFollow mainCameraFollow;
    private float originalCameraZoom;

    public Dictionary<Vector3, GameObject> PlacedObjects
    {
        get => _placedObjects;
        set => _placedObjects = value;
    }
    
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
        set => _availableAreas = value;
    }

    public HashSet<Vector3> OccupiedTiles
    {
        get => occupiedTiles;
        set => occupiedTiles = value;
    }
    
    public List<PointerBlock> PointerBlocks
    {
        get => pointerBlocks;
        set => pointerBlocks = value;
    }
    
    public int FirstPointerBlockSourceID => firstPointerBlockSourceID;
    public int LastPointerBlockSourceID => lastPointerBlockSourceID;
    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraFollow = mainCamera.GetComponent<CameraFollow>();
        currentTargetID = -1;
        
        foreach (var area in _availableAreas)
        {
            area.HideCodeableArea();
        }
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

    public bool TryGetPlacedObject(Vector3 position, out GameObject placedObject)
    {
        return _placedObjects.TryGetValue(position, out placedObject);
    }
    
    public void RegisterPlacedObject(Vector3 position, GameObject placedObject)
    {
        _placedObjects[position] = placedObject;
    }

    public void RemovePlacedObject(Vector3 position)
    {
        _placedObjects.Remove(position);
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

        firstPointerBlockSourceID = futurePointerBlocks[0].SourceID;
        lastPointerBlockSourceID = futurePointerBlocks[^1].SourceID;
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
        Debug.Log($"{currentTargetID} -> {pointer.TargetID}");

        if (pointer.SourceID == firstPointerBlockSourceID)
        {
            ResetPointerBlocks();
            return true;
        }
        
        if (currentTargetID == -1 || currentTargetID == pointer.SourceID)
        {
            if (pointer.SourceID == lastPointerBlockSourceID)
            {
                StartCoroutine(PointerBlockResetCoroutine(3f));
            }
            currentTargetID = pointer.TargetID;
            
            return true;
        }
        //FailSoundEffect
        return false;
    }

    private IEnumerator PointerBlockResetCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        ResetPointerBlocks();
    }
    
    public void ResetPointerBlocks()
    {
        foreach (var pointerBlock in pointerBlocks)
        {
            pointerBlock.DeactivatePointerBlock();
        }
        currentTargetID = -1;
    }
}

