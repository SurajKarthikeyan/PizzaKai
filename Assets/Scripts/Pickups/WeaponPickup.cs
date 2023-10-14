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
                if (master.weapons.Any(w => w.weaponID.Equals(weapon.weaponID)))
                {
                    // Ensure that we don't already have this weapon.
                    return false;
                }

                WeaponModule newweapon = master.AddWeapon(Instantiate(weapon));
                newweapon.LinkToMaster(character);
                newweapon.gameObject.transform.localScale = new Vector3(.6f, .6f);
                DialogueManager.Instance.CallDialogueBlock(weaponTutorialBlock);
            }
            catch (System.Exception e)
            {
                // Must return true, even if an error occurs, to insure
                // the game object is destroyed.
                Debug.LogError(e.Message);
            }

            return true;
        }

        return false;
    }
    #endregion
}