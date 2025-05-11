using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffectType
{
    Heal,
    SpeedBoost,
    JumpBoost
}

public static class ItemEffectProcessor
{
    public static void ApplyEffect(PlayerItem item, GameObject player)
    {
        if (item == null || player == null) 
            return;

        switch (item.EffectType)
        {
            case ItemEffectType.Heal:
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                float previousHealth = health.TotalHealth;
                float hp = item.EffectValue;
                health.Heal(hp);
                Debug.Log($"Healed by {health.TotalHealth - previousHealth}");
                break;
            case ItemEffectType.SpeedBoost:
                var speedMovement = player.GetComponent<PlayerMovement>();
                float speedMultiplier = item.EffectValue;
                speedMovement.StartSpeedBoost(speedMultiplier);
                Debug.Log($"Activated SpeedBoost for {speedMultiplier}");
                break;
            case ItemEffectType.JumpBoost:
                var jumpMovement = player.GetComponent<PlayerMovement>();
                float jumpMultiplier = item.EffectValue;
                jumpMovement.StartJumpBoost(jumpMultiplier);
                Debug.Log($"Activated JumpBoost for {jumpMultiplier}");
                break;
        }
    }
}
