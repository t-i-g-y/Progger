using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeaderBlock : MonoBehaviour, ICSharpModifiable
{
    [SerializeField] private GameObject _codeableAreaPrefab;
    private GameObject instantiatedArea;
    private ModuleCodeableArea codeableArea;
    private ModuleObjectComponentType activeComponent = ModuleObjectComponentType.None;
    
    private void Start()
    {
        InstantiateArea();
    }

    private void InstantiateArea()
    {
        instantiatedArea = Instantiate(_codeableAreaPrefab, transform.position, Quaternion.identity, transform);
        codeableArea = instantiatedArea.GetComponent<ModuleCodeableArea>();
        ModuleUIManager.Instance.CurrentModule.AvailableAreas.Add(codeableArea);
    }

    private void OnDestroy()
    {
        if (instantiatedArea != null)
        {
            ModuleUIManager.Instance.CurrentModule.AvailableAreas.Remove(codeableArea);
            codeableArea.HideCodeableArea();
            codeableArea = null;
            Destroy(instantiatedArea);
        }
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
