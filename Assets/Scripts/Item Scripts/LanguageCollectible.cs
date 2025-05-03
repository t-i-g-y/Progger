using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProgrammingLanguage
{
    BASIC,
    C,
    SQL,
    CPlusPlus,
    Python,
    CSharp
}
public class LanguageCollectible : MonoBehaviour
{
    public ProgrammingLanguage language;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            AbilityManager.Instance.UnlockAbility(language);
            Destroy(gameObject);
        }
    }
}
