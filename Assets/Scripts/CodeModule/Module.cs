using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField] private ModuleConsole console;
    [SerializeField] private Vector3 cameraFocus;
    [SerializeField] private float cameraZoom;
    [SerializeField] private List<ModuleObject> availableObjects;
    [SerializeField] private List<ModuleCodeableArea> availableAreas;
    [SerializeField] private float panTime = 0.5f;
    private Camera mainCamera;
    private CameraFollow mainCameraFollow;

    public List<ModuleObject> AvailableObjects
    {
        get => availableObjects;
    }

    public List<ModuleCodeableArea> AvailableAreas
    {
        get => availableAreas;
    }
    
    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraFollow = mainCamera.GetComponent<CameraFollow>();
    }

    public void EnterModuleMode()
    {
        Time.timeScale = 0f;
        if (mainCameraFollow != null)
        {
            mainCameraFollow.IsEnabled = false;
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
        }
        ModuleUIManager.Instance.CLoseModuleEditor();
    }

    private IEnumerator CameraPanCoroutine()
    {
        float elapsedTime = 0f;
        float startZoom = mainCamera.orthographicSize;
        Vector3 startPosition = mainCamera.transform.position;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainCamera.transform.position = Vector3.Lerp(startPosition, new Vector3(cameraFocus.x, cameraFocus.y, startPosition.z), elapsedTime / panTime);
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, cameraZoom, elapsedTime / panTime);
            
            yield return null;
        }
    }
}

