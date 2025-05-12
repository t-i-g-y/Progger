using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GoToSystem : MonoBehaviour, ICSharpModifiable
{
    [SerializeField] private Transform _goToMarker;
    private GameObject player;
    private bool canGoTo;
    private ModuleObjectComponentType activeComponent = ModuleObjectComponentType.None;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
    }

    private void Update()
    {
        if (canGoTo && Input.GetKeyDown(KeyCode.E))
        {
            PlayerGoTo();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GoTo callable");
            canGoTo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canGoTo = false;
        }
    }

    private void PlayerGoTo()
    {
        player.transform.position = _goToMarker.position;
    }
    
    public void ApplyModuleComponent(ModuleObjectComponentType componentType)
    {
        activeComponent = componentType;
    }

    public void RemoveModuleComponent(ModuleObjectComponentType componentType)
    {
        activeComponent = ModuleObjectComponentType.None;
    }

    public bool HasModuleComponent()
    {
        return activeComponent != ModuleObjectComponentType.None;
    }
    
    public ModuleObjectComponentType GetModuleComponentType()
    {
        return activeComponent;
    }
}
