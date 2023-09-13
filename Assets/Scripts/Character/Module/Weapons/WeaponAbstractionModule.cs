using System;
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
    [Tooltip("The name of the weapon.")]
    [SerializeField]
    [OnValueChanged(nameof(WeaponName_Changed))]
    private string weaponName;

    public WeaponModule[] subweapons;

    /// <inheritdoc cref="WeaponMaster"/>
    private WeaponMasterModule weaponMaster;
    #endregion

    #region Properties
    public WeaponModule CurrentWeapon => subweapons[SubweaponIndex];

    public WeaponModule Primary => subweapons[0];

    public WeaponModule Alt => subweapons[1];

    /// <summary>
    /// The reference to the weapon master.
    /// </summary>
    public WeaponMasterModule WeaponMaster => weaponMaster;

    public int SubweaponIndex => weaponMaster.LastMouseBtnClick;

    public string WeaponName => weaponName;
    #endregion

    #region Validation

    private void WeaponName_Changed()
    {
        foreach (var weapon in subweapons)
        {
            weapon.WeaponName = $"{WeaponName} {weapon.gameObject.name}";
        }
    }
    #endregion

    #region Methods
    protected override void OnLinked()
    {
        Master.RequireComponentInChildren(out weaponMaster);
    }

    public bool TryFireWeapon()
    {
        return CurrentWeapon.TryFireWeapon();
    }

    public bool TryReloadWeapon()
    {
        // Only primary can reload.
        return Primary.TryReloadWeapon();
    }

    public void ResetBurst()
    {
        CurrentWeapon.ResetBurst();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    #endregion
}