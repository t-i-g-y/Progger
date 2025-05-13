using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;
    private HashSet<ProgrammingLanguage> unlockedAbilities = new HashSet<ProgrammingLanguage>();
    private GameObject player;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
            playerMovement = player.GetComponent<PlayerMovement>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ActivateBASIC()
    {
        Debug.Log("BASIC Activated");
    }

    private void ActivateC()
    {
        Debug.Log("C Activated");
        playerMovement.CanDash = true;
    }

    private void ActivateCPlusPlus()
    {
        Debug.Log("C++ Activated");
        playerMovement.MaxJumps++;
    }

    private void ActivateSQL()
    {
        Debug.Log("SQL Activated");
    }

    private void ActivatePython()
    {
        Debug.Log("Python Activated");
    }
    
    private void ActivateCSharp()
    {
        Debug.Log("C# Activated");
    }
    public bool AbilityUnlocked(ProgrammingLanguage language) => unlockedAbilities.Contains(language);
    public List<ProgrammingLanguage> GetUnlockedAbilities()
    {
        List<ProgrammingLanguage> result = new();
        foreach (ProgrammingLanguage lang in Enum.GetValues(typeof(ProgrammingLanguage)))
        {
            if (AbilityUnlocked(lang))
                result.Add(lang);
        }
        return result;
    }

    public void LoadUnlockedAbilities(List<ProgrammingLanguage> unlocked)
    {
        foreach (var lang in unlocked)
        {
            UnlockAbility(lang);
        }
    }

    public void UnlockAbility(ProgrammingLanguage language)
    {
        if (!unlockedAbilities.Contains(language))
        {
            unlockedAbilities.Add(language);

            switch (language)
            {
                case ProgrammingLanguage.BASIC:
                    ActivateBASIC();
                    break;
                case ProgrammingLanguage.C:
                    ActivateC();
                    break;
                case ProgrammingLanguage.SQL:
                    ActivateSQL();
                    break;
                case ProgrammingLanguage.CPlusPlus:
                    ActivateCPlusPlus();
                    break;
                case ProgrammingLanguage.Python:
                    ActivatePython();
                    break;
                case ProgrammingLanguage.CSharp:
                    ActivateCSharp();
                    break;
            }
        }
    }
    
    
    
}
