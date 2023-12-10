using Fungus;
using System.Linq;
using UnityEngine;

public class WeaponPickup : Pickup
{
    #region Variables
    public WeaponModule weapon;
    public string weaponTutorialBlock;
    #endregion

    #region Pickup Implementation
    protected override bool ReceiveCharacter(Character character)
    {
        if (character.HasComponentInChildren(out WeaponMasterModule master))
        {
            try
            {
                if (master.availableWeapons.Any(w => w.weaponID.Equals(weapon.weaponID)))
                {
                    // Ensure that we don't already have this weapon.
                    return false;
                }

                WeaponModule newweapon = master.AddWeapon(weapon);
                newweapon.LinkToMaster(character);
                if (newweapon.weaponID == "Shotgun")
                {
                    newweapon.gameObject.transform.localScale = new Vector3(.5f, .5f);
                }
                else if (newweapon.weaponID == "Flamethrower")
                {
                    newweapon.gameObject.transform.localScale = new Vector3(.55f, .55f);
                }
                
                character.HP = 999999;
                DialogueManager.Instance.CallDialogueBlock(weaponTutorialBlock);
                return true;
            }
            catch (System.Exception e)
            {
                // Must return true, even if an error occurs, to insure
                // the game object is destroyed.
                Debug.LogError(e.Message);
                return true;
            }
        }

        return false;
    }
    #endregion
}