using UnityEngine;

/// <summary>
/// Defines a weapon pickup.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class WeaponPickup : Pickup
{
    public WeaponAbstractionModule weaponPrefab;

    protected override bool TryPickup(Character character)
    {
        if (character.HasComponentInChildren(out WeaponMasterModule weaponMaster))
        {
            weaponMaster.AddWeapon(weaponPrefab.InstantiateComponent());
            return true;
        }

        return false;
    }
}