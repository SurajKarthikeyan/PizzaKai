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
            WeaponModule newweapon = master.AddWeapon(Instantiate(weapon));
            newweapon.LinkToMaster(character);
            return true;
        }

        return false;
    }
    #endregion
}