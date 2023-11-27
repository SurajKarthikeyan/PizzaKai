using UnityEngine;

/// <summary>
/// An upgrade to the range of a projectile.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class RangeUpgrade : WeaponUpgrade
{
    #region Variables
    [Tooltip("The modification to range.")]
    public Modifier rangeModifier = new(0, 2);
    #endregion

    #region Main Logic
    public override bool ApplyUpgrade(WeaponModule weapon)
    {
        if (weapon.bullet is SimpleProjectile projectile)
        {
            // Since the bullet is a prefab, not an instance, we cannot modify
            // it here. Make a copy instead.
            var copy = projectile.InstantiateComponent();
            copy.transform.Localize(weapon.transform);
            copy.gameObject.SetActive(false);

            copy.range = rangeModifier.Modify(copy.range);

            // Now set the bullet of the weapon to the copy.
            weapon.bullet = copy;
        }

        return false;
    }
    #endregion
}