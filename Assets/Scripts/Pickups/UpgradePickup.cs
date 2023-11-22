using UnityEngine;
public class UpgradePickup : Pickup
{
    #region Variables
    public WeaponUpgrade upgrade;

    [Tooltip("The weaponID of the weapon to target.")]
    public string weaponTarget;
    #endregion

    protected override bool ReceiveCharacter(Character character)
    {
        if (character.IsPlayer && !string.IsNullOrEmpty(weaponTarget))
        {
            // Player is doing the pickup. Apply to targeted weapon.
            var wep = character.weaponMasterModule.weapons
                .Find(w => w.weaponID == weaponTarget);
            
            if (wep)
            {
                upgrade.ApplyUpgrade(wep);
                return true;
            }

            // Player does not have the specified weapon.
            return false;
        }

        return false;
    }
}
