using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeaderBlock : MonoBehaviour
{
    [SerializeField] private GameObject _codeableAreaPrefab;
    private GameObject instantiatedArea;
    private ModuleCodeableArea codeableArea;
    
    private void Start()
    {
        InstantiateArea();
    }

    private void InstantiateArea()
    {
        instantiatedArea = Instantiate(_codeableAreaPrefab, transform.position, Quaternion.identity, transform);
        codeableArea = instantiatedArea.GetComponent<ModuleCodeableArea>();
        ModuleUIManager.Instance.CurrentModule.AvailableAreas.Add(codeableArea);
        //codeableArea.ShowCodeableArea();
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
}
