using UnityEngine;

public class WeaponPickup : Pickup
{
    #region Variables
    public WeaponModule weapon;
    #endregion

    #region Pickup Implementation
    protected override bool ReceiveCharacter(Character character)
    {
        if (character.HasComponentInChildren(out WeaponMasterModule master))
        {
            master.AddWeapon(Instantiate(weapon));
            return true;
        }

        return false;
    }
    #endregion
}