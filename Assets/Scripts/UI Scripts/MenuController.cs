using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _singleSlotUI;
    [SerializeField] private TabController _tabController;
    private void Start()
    {
        _menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            _tabController.RefreshTab();
            bool newState = !_menuCanvas.activeSelf;
            _menuCanvas.SetActive(!_menuCanvas.activeSelf);
            Time.timeScale = _menuCanvas.activeSelf ? 0f : 1f;
            _singleSlotUI.SetActive(!newState);
        }
    }
}
