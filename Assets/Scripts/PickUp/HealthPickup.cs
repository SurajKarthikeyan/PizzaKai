using UnityEngine;

/// <summary>
/// Defines a health pickup that can either increase or decrease health.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class HealthPickup : Pickup
{
    [Tooltip("How much health is gained? Make negative for health loss.")]
    public int healthGain;

    protected override bool TryPickup(Character character)
    {
        if (healthGain > 0)
        {
            character.RestoreHealth(healthGain);
            return true;
        }
        else if (healthGain < 0)
        {
            return character.TakeDamage(
                healthGain,
                RNGExt.OnCircle()
            );
        }

        return false;
    }
}