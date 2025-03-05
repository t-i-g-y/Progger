using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleConsole : MonoBehaviour
{
    [SerializeField] private GameObject[] codeBlocks;
    [SerializeField] private ProgrammingLanguage[] languages;
    [SerializeField] private GameObject codeableArea;
    [SerializeField] private GameObject moduleUI;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float cameraZoom = 6f;
    [SerializeField] private Transform codeBlockContainer;

    private bool moduleMode;
    private bool isUsable;
    private Vector3 startCameraPosition;
    private float startCameraSize;
    private List<GameObject> placedBlocks = new List<GameObject>();

    

    private void Update()
    {
        if (isUsable)
        {
            if (!moduleMode && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Console opened");
                EnterModuleMode();
            }
            else if (moduleMode && Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Console exited");
                ExitModuleMode();
            }
        }
    }

    private void EnterModuleMode()
    {
        moduleMode = true;
        startCameraPosition = mainCamera.transform.position;
        startCameraSize = mainCamera.orthographicSize;
        mainCamera.transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y,
            mainCamera.transform.position.z);
        mainCamera.orthographicSize = cameraZoom;
    }

    private void ExitModuleMode()
    {
        moduleMode = false;
        mainCamera.transform.position = startCameraPosition;
        mainCamera.orthographicSize = startCameraSize;
    }

    private void SetupCodeBlocks()
    {
        foreach (GameObject codeBlock in codeBlocks)
        {
            GameObject buttonUI = new GameObject($"{codeBlock.name} Button");
            buttonUI.transform.SetParent(codeBlockContainer);
            Button button = buttonUI.AddComponent<Button>();
            button.onClick.AddListener(() => PlaceCodeBlocks(codeBlock));
        }
    }

    private void PlaceCodeBlocks(GameObject codeBlock)
    {
        GameObject newCodeBlock = Instantiate(codeBlock);
        newCodeBlock.AddComponent<ModuleDraggable>().SetCodeableArea(codeableArea.transform);
        placedBlocks.Add(newCodeBlock);
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
