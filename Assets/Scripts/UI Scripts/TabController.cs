using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabController : MonoBehaviour
{
    [SerializeField] private Image[] tabImages;
    public GameObject[] pages;
    private bool CSharpAlreadyUnlocked;
    private bool SQLAlreadyUnlocked;
    private int lastTab;
    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tab)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }

        if (!CSharpAlreadyUnlocked && tab == (int)MenuPages.Player && AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.CSharp))
        {
            pages[tab].transform.GetChild(0).gameObject.SetActive(true);
            UpgradeComponentManager.Instance.BuildComponentList();
            CSharpAlreadyUnlocked = true;
        }
        else if (!SQLAlreadyUnlocked && tab == (int)MenuPages.Inventory && AbilityManager.Instance.AbilityUnlocked(ProgrammingLanguage.SQL))
        {
            pages[tab].transform.GetChild(0).gameObject.SetActive(true);
            SQLAlreadyUnlocked = true;
        }
        pages[tab].SetActive(true);
        tabImages[tab].color = Color.white;
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
