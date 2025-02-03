using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private PushMoveableObject BASIC_ability;
    
    private LanguageCollectible language;
    
    public void ActivateBASIC()
    {
        BASIC_ability.enabled = true;
    }

    public void ActivateC()
    {
        Debug.Log("C Activated");
    }

    public void ActivateCPlusPlus()
    {
        Debug.Log("C++ Activated");
    }

    public void ActivateCSharp()
    {
        Debug.Log("C# Activated");
    }

    public void ActivateSQL()
    {
        Debug.Log("SQL Activated");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.LANGUAGE_TAG))
        {
            language = other.GetComponent<LanguageCollectible>();
            
            if (language.language == ProgrammingLanguage.BASIC)
                ActivateBASIC();
            else if (language.language == ProgrammingLanguage.C)
                ActivateC();
            else if (language.language == ProgrammingLanguage.CPlusPlus)
                ActivateCPlusPlus();
            else if (language.language == ProgrammingLanguage.CSharp)
                ActivateCSharp();
            else if (language.language == ProgrammingLanguage.SQL)
                ActivateSQL();
            
            Destroy(language.gameObject);
        }
    }
}
