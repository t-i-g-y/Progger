using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject singleSlotUI;
    [SerializeField] private TabController _tabController;
    private void Start()
    {
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            _tabController.RefreshTab();
            bool newState = !menuCanvas.activeSelf;
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            Time.timeScale = menuCanvas.activeSelf ? 0f : 1f;
            singleSlotUI.SetActive(!newState);
        }
    }
}
