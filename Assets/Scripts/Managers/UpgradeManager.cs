using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to a(n) 
    /// <see cref="UpgradeManager"/> instance.
    /// </summary>
    private static UpgradeManager instance;

    /// <inheritdoc cref="instance"/>
    public static UpgradeManager Instance => instance;
    #endregion

    #region Variables
    public FlameProjectile flameProjectile;
    public Multishot shotgunProjectile;

    [Header("Upgrade Values")]
    [Tooltip("the amount of extra bullets the upgrade adds to the tommygun's magazine")]
    public int extraTommyAmmo;
    [Tooltip("the amount of extra pellets per shotgun shot added by the upgrade")]
    public int extraShotgunPellets;
    [Tooltip("the increase in range of the flamethrower given by the upgrade")]
    public float extraFlamethrowerRange;
    [Tooltip("the decrease of the sniper's reload speed as a percentage given by the upgrade")]
    public float decreaseSniperReload;

    //whether or not a weapon has it's upgrade
    [Header("Enable before playing for testing")]
    [SerializeField]
    private bool tommyUP;
    [SerializeField]
    private bool shotgunUP, flamethrowerUP, sniperUP;

    

    #endregion

    #region Startup
    private void Awake()
    {
        this.InstantiateSingleton(ref instance);
    }

    private void Start()
    {
        //these are for re-applying the upgrades to the new player in the scene
        if (tommyUP)
        {
            tommyUP = false;
            TommyGunUpgrade();
        }
        if (shotgunUP)
        {
            shotgunUP = false;
            ShotgunUpgrade();
        }
        if (flamethrowerUP)
        {
            flamethrowerUP = false;
            FlamethrowerUpgrade();
        }
        if (sniperUP)
        {
            sniperUP = false;
            SniperUpgrade();
        }
    }
    #endregion

    #region UpgradeCalls
    //these are called by the upgrade pickup once picked up, and this script to re-apply the upgrades in a new scene
    public void TommyGunUpgrade()
    {
        if (!tommyUP)
        {
            FindObjectOfType<TommyGunWeapon>().ammoCount += extraTommyAmmo;
            tommyUP = true;
        }
    }

    public void ShotgunUpgrade()
    {
        if (!shotgunUP)
        {
            shotgunProjectile.shots[0].amount = new(shotgunProjectile.shots[0].amount.singleValue + extraShotgunPellets);
            shotgunUP = true;
        }
    }

    public void FlamethrowerUpgrade()
    {
        if (!flamethrowerUP)
        {
            flameProjectile.range = new(flameProjectile.range.singleValue + extraFlamethrowerRange);
            flamethrowerUP = true;
        }
    }

    public void SniperUpgrade()
    {
        if (!sniperUP)
        {

            sniperUP = true;
        }
    }
    #endregion
}
