using UnityEngine;

/// <summary>
/// An upgrade for ammo capacity.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AmmoCapacityUpgrade : WeaponUpgrade
{
    #region Variables
    public Modifier ammoModifier = new(20, 1);
    #endregion

    #region Main Logic
    public override bool ApplyUpgrade(WeaponModule weapon)
    {
        weapon.ammoCount = Mathf.FloorToInt(ammoModifier.Modify(weapon.ammoCount));
        return true;
    }
    #endregion
}