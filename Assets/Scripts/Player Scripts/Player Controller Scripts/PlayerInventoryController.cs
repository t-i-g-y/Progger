using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("Single Slot UI")]
    [SerializeField] private GameObject singleSlotUI;
    [SerializeField] private Image singleSlotIcon;
    [SerializeField] private Image consumeProgressBar;
    [SerializeField] private float consumeHoldTime = 1.25f;
    
    [Header("SQL Inventory")]
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform SQLInventoryContent;
    [SerializeField] private GameObject SQLInventoryUI;
    [SerializeField] private GameObject menuCanvas;
    private PlayerItem currentHotbarItem;
    private List<PlayerItem> SQLItems = new();
    private List<InventoryRow> SQLRows = new();
    private int selectedIndex;
    private float consumeTimer;
    private bool isConsuming;
    private bool wasSQLUnlocked;
    private GameObject player;
    private bool IsSQLUnlocked => AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.SQL);

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
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
        if (currentHotbarItem == null || menuCanvas.activeSelf) 
            return;

        if (Input.GetKey(KeyCode.E))
        {
            isConsuming = true;
            consumeTimer += Time.deltaTime;
            consumeProgressBar.fillAmount = consumeTimer / consumeHoldTime;

            if (consumeTimer >= consumeHoldTime)
            {
                ConsumeHotbarItem();
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isConsuming = false;
            consumeTimer = 0f;
            consumeProgressBar.fillAmount = 0f;
        }
    }

    private void HandleSQLInventoryInput()
    {
        if (!IsSQLUnlocked || !menuCanvas.activeSelf || !SQLInventoryUI.activeSelf || SQLRows.Count == 0)
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

    public void CollectItem(PlayerItem item)
    {
        if (SQLItems.Contains(item)) 
            return;

        if (IsSQLUnlocked)
        {
            AddToSQLInventory(item);
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

    private void SetHotbarItem(PlayerItem item)
    {
        currentHotbarItem = item;
        singleSlotIcon.sprite = item.ItemIcon.sprite;
        singleSlotIcon.color = item.ItemIcon.color;
        singleSlotIcon.enabled = true;
        singleSlotUI.SetActive(true);
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
        singleSlotIcon.enabled = false;
        consumeProgressBar.fillAmount = 0f;
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
        SQLItems.Add(item);
        item.gameObject.SetActive(false);
        RefreshSQLInventory();
        Debug.Log($"{item}");
        UpdateRowSelection();
        UpdateHotbarSlot();
    }

    public void ConsumeSQLItem(PlayerItem item)
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
        for (int i = 0; i < SQLInventoryContent.childCount; i++)
            Destroy(SQLInventoryContent.GetChild(i).gameObject);
        
        selectedIndex = 0;
        
        for (int i = 0; i < SQLItems.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, SQLInventoryContent);
            InventoryRow rowScript = row.GetComponent<InventoryRow>();
            rowScript.Initialize(SQLItems[i], this);
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
            singleSlotIcon.sprite = currentHotbarItem.ItemIcon.sprite;
            singleSlotIcon.color = currentHotbarItem.ItemIcon.color;
            singleSlotIcon.enabled = true;
        }
        else
        {
            singleSlotIcon.enabled = false;
        }
    }
}
