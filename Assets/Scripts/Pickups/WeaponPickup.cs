using Fungus;
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
            WeaponModule newweapon = master.AddWeapon(Instantiate(weapon));
            newweapon.LinkToMaster(character);
            newweapon.gameObject.transform.localScale = new Vector3(.6f, .6f);
            DialogueManager.Instance.CallDialogueBlock(weaponTutorialBlock);
            return true;
        }

        return false;
    }
    #endregion
}