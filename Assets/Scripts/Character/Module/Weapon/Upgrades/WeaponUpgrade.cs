using UnityEngine;

/// <summary>
/// Represents an upgrade for a weapon.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public abstract class WeaponUpgrade : Module
{
    #region Methods
    /// <summary>
    /// Applies the upgrade to the weapon.
    /// </summary>
    public abstract bool ApplyUpgrade(WeaponModule weapon);
    #endregion
}