using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// The alt fire is pretty much another weapon. If that's the case, why not make
/// it another gun?
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class WeaponAbstractionModule : Module, IWeapon
{
    #region Variables
    public WeaponModule[] subweapons;

    [ReadOnly]
    public int subweaponIndex;
    #endregion

    #region Properties
    public WeaponModule CurrentWeapon => subweapons[subweaponIndex];

    public WeaponModule Primary => subweapons[0];

    public WeaponModule Alt => subweapons[1];
    #endregion

    #region Methods
    public bool TryFireWeapon()
    {
        return CurrentWeapon.TryFireWeapon();
    }

    public void ReloadWeapon()
    {
        // Only primary can reload.
        Primary.ReloadWeapon();
    }

    public void ResetBurst()
    {
        CurrentWeapon.ResetBurst();
    }
    #endregion
}