using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UpgradeComponentManager : MonoBehaviour
{
    public static UpgradeComponentManager Instance;

    [SerializeField] private Transform _componentContainer;
    [SerializeField] private GameObject _upgradeComponentPrefab;

    private List<UpgradeComponentData> availableComponents = new();
    private List<UpgradeComponentData> assignedComponents = new();
    private GameObject player;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    public List<UpgradeComponentData> AvailableComponents
    {
        get => availableComponents;
        set => availableComponents = value;
    }
    
    public Transform ComponentContainer => _componentContainer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
            playerMovement = player.GetComponent<PlayerMovement>();
            playerHealth = player.GetComponent<PlayerHealth>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddNewComponent(UpgradeComponentData data)
    {
        if (!availableComponents.Contains(data) && !assignedComponents.Contains(data))
        {
            availableComponents.Add(data);
        }
    }

    public void BuildComponentList()
    {
        foreach (Transform child in _componentContainer)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var data in availableComponents)
        {
            if (assignedComponents.Contains(data))
                continue;

            GameObject go = Instantiate(_upgradeComponentPrefab, _componentContainer);
            var comp = go.GetComponent<UpgradeComponent>();
            comp.Initialize(data);
        }

        Debug.Log($"List built: {availableComponents.Count} available, {assignedComponents.Count} assigned");
    }

    public void AssignComponent(UpgradeComponentData data)
    {
        if (!assignedComponents.Contains(data))
        {
            assignedComponents.Add(data);
            availableComponents.Remove(data);
            ApplyUpgrade(data);
            Debug.Log($"Assigning component: {availableComponents.Count} available, {assignedComponents.Count} assigned");
        }
    }

    public void UnassignComponent(UpgradeComponentData data)
    {
        if (assignedComponents.Contains(data))
        {
            assignedComponents.Remove(data);
            availableComponents.Add(data);
            RemoveUpgrade(data);
            Debug.Log($"Unassigning component: {availableComponents.Count} available, {assignedComponents.Count} assigned");
        }
    }
    
    public void ReturnToComponentList(UpgradeComponent component)
    {
        component.transform.SetParent(_componentContainer, false);
        component.transform.localPosition = Vector3.zero;
        BuildComponentList();
    }
    
    public void ApplyUpgrade(UpgradeComponentData data)
    {
        Debug.Log($"Upgrade: {data.type}");

        switch (data.type)
        {
            case UpgradeType.MaxJump:
                playerMovement.MaxJumps += (int)data.value;
                break;
            case UpgradeType.Speed:
                playerMovement.SpeedMultiplier += data.value;
                break;
            case UpgradeType.JumpBoost:
                playerMovement.JumpMultiplier += data.value;
                break;
            case UpgradeType.Health:
                playerHealth.IncreaseMaxHealth(data.value);
                break;
        }
    }
    
    public void RemoveUpgrade(UpgradeComponentData data)
    {
        Debug.Log($"Remove Upgrade: {data.type}");

        switch (data.type)
        {
            case UpgradeType.MaxJump:
                playerMovement.MaxJumps -= (int)data.value;
                break;
            case UpgradeType.Speed:
                playerMovement.SpeedMultiplier -= data.value;
                break;
            case UpgradeType.JumpBoost:
                playerMovement.JumpMultiplier -= data.value;
                break;
            case UpgradeType.Health:
                playerHealth.DecreaseMaxHealth(data.value);
                break;
        }
    }
}
