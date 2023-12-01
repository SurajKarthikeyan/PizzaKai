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
    private WeaponModule tommygun;
    private WeaponModule shotgun;
    private WeaponModule flamethrower;
    private WeaponModule sniper;

    private static int endingWeapon;

    private WeaponMasterModule WMM;

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
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        WMM = FindObjectOfType<WeaponMasterModule>();
        foreach(WeaponModule weaponM in WMM.weapons)
        {
            switch (weaponM.weaponID)
            {
                case "SMG":
                    tommygun = weaponM;
                    break;
                case "Shotgun":
                    shotgun = weaponM;
                    break;
                case "Flamethrower":
                    flamethrower = weaponM;
                    break;
                //case "Sniper":
                    //sniper = weaponM;
                    //break;
            }
        }
        //shotgun.bullet.gameObject.GetComponent<Multishot>().upAmount = 0;
        //flamethrower.bullet.gameObject.GetComponent<FlameProjectile>().upRange = 0;
        

        //these are for re-applying the upgrades to the new player in the scene
        if (tommyUP)
        {
            tommyUP = false;
            UpgradeTommyGun();
        }
        if (shotgunUP)
        {
            shotgunUP = false;
            UpgradeShotgun();
        }
        if (flamethrowerUP)
        {
            flamethrowerUP = false;
            UpgradeFlamethrower();
        }
        if (sniperUP)
        {
            sniperUP = false;
            UpgradeSniper();
        }

        if(WMM.weaponIndex != endingWeapon)
        {
            WMM.SwitchToWeapon(endingWeapon);
        }
    }
    #endregion

    #region UpgradeCalls
    //these are called by the upgrade pickup once picked up, and this script to re-apply the upgrades in a new scene
    public void UpgradeTommyGun()
    {
        if (!tommyUP)
        {
            tommygun.ammoCount += extraTommyAmmo;
            tommyUP = true;
        }
    }

    public void UpgradeShotgun()
    {
        if (!shotgunUP)
        {
            shotgun.bullet.gameObject.GetComponent<Multishot>().upAmount = extraShotgunPellets;
            shotgunUP = true;
        }
    }

    public void UpgradeFlamethrower()
    {
        if (!flamethrowerUP)
        {
            flamethrower.bullet.gameObject.GetComponent<FlameProjectile>().upRange = extraFlamethrowerRange;
            flamethrowerUP = true;
        }
    }

    public void UpgradeSniper()
    {
        if (!sniperUP)
        {

            sniperUP = true;
        }
    }

    #endregion

    public void RememberWeapon()
    {
        endingWeapon = WMM.weaponIndex;
    }
    
}
