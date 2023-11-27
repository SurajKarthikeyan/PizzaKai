using UnityEngine;
public class UpgradePickup : Pickup
{
    public enum eUpgrade
    {
        Tommygun, Shotgun, Flamethrower, Sniper, none
    }

    public eUpgrade upgradeType = eUpgrade.none;

    protected override bool ReceiveCharacter(Character character)
    {
        switch (upgradeType)
        {
            case eUpgrade.Tommygun:
                UpgradeManager.Instance.UpgradeTommyGun();
                return true;
            case eUpgrade.Shotgun:
                UpgradeManager.Instance.UpgradeShotgun();
                return true;
            case eUpgrade.Flamethrower:
                UpgradeManager.Instance.UpgradeFlamethrower();
                return true;
            case eUpgrade.Sniper:
                UpgradeManager.Instance.UpgradeSniper();
                return true;
            default:
                Debug.LogWarning("upgradeType was not set for " + gameObject.name);
                return false;
        }
    }
}
