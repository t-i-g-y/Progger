using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class TabController : MonoBehaviour
{
    [SerializeField] private Image[] _tabImages;
    [SerializeField] private GameObject[] _pages;
    private bool CSharpAlreadyUnlocked;
    private bool SQLAlreadyUnlocked;
    private int lastTab;
    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tab)
    {
        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(false);
            _tabImages[i].color = Color.grey;
        }

        if (!CSharpAlreadyUnlocked && tab == (int)MenuPages.Player && AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.CSharp))
        {
            _pages[tab].transform.GetChild(0).gameObject.SetActive(true);
            UpgradeComponentManager.Instance.BuildComponentList();
            CSharpAlreadyUnlocked = true;
        }
        else if (!SQLAlreadyUnlocked && tab == (int)MenuPages.Inventory && AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.SQL))
        {
            _pages[tab].transform.GetChild(0).gameObject.SetActive(true);
            SQLAlreadyUnlocked = true;
        }
        _pages[tab].SetActive(true);
        _tabImages[tab].color = Color.white;
        lastTab = tab;
    }

    public void RefreshTab()
    {
        ActivateTab(lastTab);
    }
    private enum MenuPages
    {
        Player,
        Inventory,
        Map,
        Settings
    }
}
