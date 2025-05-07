using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInventoryController : MonoBehaviour
{
    public static PlayerInventoryController Instance;
    
    [Header("Single Slot UI")]
    [SerializeField] private GameObject _singleSlotUI;
    [SerializeField] private Image _singleSlotIcon;
    [SerializeField] private Image _consumeProgressBar;
    [SerializeField] private float _consumeHoldTime = 1.25f;
    
    [Header("SQL Inventory")]
    [SerializeField] private GameObject _rowPrefab;
    [SerializeField] private Transform _SQLInventoryContent;
    [SerializeField] private GameObject _SQLInventoryUI;
    [SerializeField] private GameObject _menuCanvas;
    private PlayerItem currentHotbarItem;
    private List<PlayerItem> SQLItems = new();
    private List<InventoryRow> SQLRows = new();
    private int selectedIndex;
    private float consumeTimer;
    private bool isConsuming;
    private bool wasSQLUnlocked;
    private GameObject player;
    private bool IsSQLUnlocked => AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.SQL);
    
    [Header("C# Component Inventory")]
    [SerializeField] private Transform _cSharpInventoryContent;
    
    private List<UpgradeComponentItem> csharpComponents = new();

    public List<UpgradeComponentItem> CsharpComponents
    {
        get => csharpComponents;
        set => csharpComponents = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        HandleInventoryStateTransition();
        HandleHotbarInput();
        HandleSQLInventoryInput();
    }

    private void HandleInventoryStateTransition()
    {
        if (!wasSQLUnlocked && IsSQLUnlocked)
        {
            wasSQLUnlocked = true;

            if (currentHotbarItem)
            {
                AddToSQLInventory(currentHotbarItem);
            }
        }
    }

    private void HandleHotbarInput()
    {
        if (currentHotbarItem == null || _menuCanvas.activeSelf) 
            return;

        if (Input.GetKey(KeyCode.E))
        {
            isConsuming = true;
            consumeTimer += Time.deltaTime;
            _consumeProgressBar.fillAmount = consumeTimer / _consumeHoldTime;

            if (consumeTimer >= _consumeHoldTime)
            {
                ConsumeHotbarItem();
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isConsuming = false;
            consumeTimer = 0f;
            _consumeProgressBar.fillAmount = 0f;
        }
    }

    private void HandleSQLInventoryInput()
    {
        if (!IsSQLUnlocked || !_menuCanvas.activeSelf || !_SQLInventoryUI.activeSelf || SQLRows.Count == 0)
            return;
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = Mathf.Clamp(selectedIndex + 1, 0, SQLRows.Count - 1);
            UpdateRowSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = Mathf.Clamp(selectedIndex - 1, 0, SQLRows.Count - 1);
            UpdateRowSelection();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ConsumeSQLItem(SQLItems[selectedIndex]);
        }
    }

    public void CollectPlayerItem(PlayerItem item)
    {
        if (SQLItems.Contains(item)) 
            return;

        if (IsSQLUnlocked)
        {
            AddToSQLInventory(item);
            consumeTimer = 0f;
        }
        else
        {
            if (currentHotbarItem == null)
            {
                SetHotbarItem(item);
            }
            else
            {
                PlayerItem toDrop = currentHotbarItem;
                DropItem(toDrop);
                SetHotbarItem(item);
                consumeTimer = 0f;
            }
        }
    }

    public void CollectUpgradeComponentItem(UpgradeComponentItem item)
    {
        if (csharpComponents.Contains(item))
            return;
        
        csharpComponents.Add(item);
        UpgradeComponentManager.Instance.AddNewComponent(item.componentData);
        item.gameObject.SetActive(false);
        if (IsSQLUnlocked)
        {
            RefreshComponentInventory();
        }

        consumeTimer = 0f;
        Debug.Log($"{item}");
    }

    private void RefreshComponentInventory()
    {
        for (int i = 0; i < _cSharpInventoryContent.childCount; i++)
            Destroy(_cSharpInventoryContent.GetChild(i).gameObject);
        
        for (int i = 0; i < csharpComponents.Count; i++)
        {
            GameObject row = Instantiate(_rowPrefab, _cSharpInventoryContent);
            InventoryRow rowScript = row.GetComponent<InventoryRow>();
            rowScript.Initialize(csharpComponents[i]);
        }
    }
    
    private void SetHotbarItem(PlayerItem item)
    {
        currentHotbarItem = item;
        _singleSlotIcon.sprite = item.ItemIcon.sprite;
        _singleSlotIcon.color = item.ItemIcon.color;
        _singleSlotIcon.enabled = true;
        _singleSlotUI.SetActive(true);
        item.gameObject.SetActive(false);
    }

    private void ConsumeHotbarItem()
    {
        if (currentHotbarItem == null) 
            return;

        if (IsSQLUnlocked)
        {
            ConsumeSQLItem(currentHotbarItem);
        }
        else
        {
            ItemEffectProcessor.ApplyEffect(currentHotbarItem, player);
            Destroy(currentHotbarItem.gameObject);
        }
        currentHotbarItem = null;
        _singleSlotIcon.enabled = false;
        _consumeProgressBar.fillAmount = 0f;
        consumeTimer = 0f;
        UpdateHotbarSlot();
    }

    private void DropItem(PlayerItem item)
    {
        item.gameObject.transform.position = player.transform.position;
        item.gameObject.SetActive(true);
    }

    private void AddToSQLInventory(PlayerItem item)
    {
        Debug.Log($"Adding {item}; Null: {item == null}");
        SQLItems.Add(item);
        item.gameObject.SetActive(false);
        RefreshSQLInventory();
        Debug.Log($"{item} added; Null: {item == null}");
        UpdateRowSelection();
        UpdateHotbarSlot();
    }

    private void ConsumeSQLItem(PlayerItem item)
    {
        bool wasFirstItem = (SQLItems.Count > 0 && SQLItems[0] == item);
        ItemEffectProcessor.ApplyEffect(item, player);
        SQLItems.Remove(item);
        Destroy(item.gameObject);
        RefreshSQLInventory();
        if (wasFirstItem)
            UpdateHotbarSlot();
    }

    private void RefreshSQLInventory()
    {
        SQLRows.Clear();
        for (int i = 0; i < _SQLInventoryContent.childCount; i++)
            Destroy(_SQLInventoryContent.GetChild(i).gameObject);
        
        selectedIndex = 0;
        
        for (int i = 0; i < SQLItems.Count; i++)
        {
            GameObject row = Instantiate(_rowPrefab, _SQLInventoryContent);
            InventoryRow rowScript = row.GetComponent<InventoryRow>();
            Debug.Log($"Init {SQLItems[i]}; Null: {SQLItems[i] == null}");
            rowScript.Initialize(SQLItems[i]);
            SQLRows.Add(rowScript);
        }
    }
    
    private void UpdateRowSelection()
    {
        for (int i = 0; i < SQLRows.Count; i++)
        {
            SQLRows[i].SetSelected(i == selectedIndex);
        }
    }
    
    private void UpdateHotbarSlot()
    {
        if (SQLItems.Count > 0)
        {
            currentHotbarItem = SQLItems[0];
            _singleSlotIcon.sprite = currentHotbarItem.ItemIcon.sprite;
            _singleSlotIcon.color = currentHotbarItem.ItemIcon.color;
            _singleSlotIcon.enabled = true;
        }
        else
        {
            _singleSlotIcon.enabled = false;
        }
    }
    
    
}