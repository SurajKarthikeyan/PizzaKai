using Mono.Cecil;
using TMPro;
using UnityEngine;

/// <summary>
/// An upgrade which increases the pellet count for multi-shot projectiles.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class ShotgunUpgrade : WeaponUpgrade
{
    #region Variables
    public Modifier shotCountModifier = new(0, 1);
    #endregion

    public override bool ApplyUpgrade(WeaponModule weapon)
    {
        if (weapon.bullet is Multishot multishot)
        {
            // Since the bullet is a prefab, not an instance, we cannot modify
            // it here. Make a copy instead.
            Multishot copy = multishot.InstantiateComponent();
            copy.transform.Localize(weapon.transform);
            copy.gameObject.SetActive(false);

            foreach (var shot in copy.shots)
            {
                shot.amount = shotCountModifier.Modify(shot.amount);
            }

            weapon.bullet = copy;
            return true;
        }

        return false;
    }
}