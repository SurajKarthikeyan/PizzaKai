using UnityEngine;


/// <summary>
/// Represents the Flamethrower, child class of WeaponModule
/// 
/// Primarily used for player character
/// 
/// <br/>
/// 
/// Authors: Zane O'Dell (2023)
/// </summary>
public class FlameThrowerWeapon : WeaponModule
{
    #region Variables
    [Header("Alt Fire Settings")]
    [Tooltip("Fireball used for alt fire")]
    public GameObject altFireball;
    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.FlamethrowerName;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Overrides the alt fire function of weapon module - Flame Shot
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();
        Instantiate(altFireball, firePoint.position, Quaternion.identity);
    }
    #endregion
}
