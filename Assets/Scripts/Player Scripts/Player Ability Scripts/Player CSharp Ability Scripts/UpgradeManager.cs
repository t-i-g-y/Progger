using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    public static PlayerUpgradeManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ApplyUpgrade(UpgradeType type, float value)
    {
        GameObject player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
        Debug.Log($"Upgrade: {type}");

        switch (type)
        {
            case UpgradeType.MaxJump:

                break;
            case UpgradeType.Speed:

                break;
            case UpgradeType.JumpBoost:

                break;
            case UpgradeType.Health:

                break;
        }
    }
}
