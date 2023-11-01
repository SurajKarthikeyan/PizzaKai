using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Represents the Tommy Gun, child class of WeaponModule
/// 
/// Primarily used for player character
/// 
/// <br/>
/// 
/// Authors: Zane O'Dell (2023)
/// </summary>
public class TommyGunWeapon : WeaponModule
{
    #region Variables
    [Header("Alt Fire Settings")]
    [Tooltip("Point from which tommy flash starts")]
    public Transform playerTransform;

    [Tooltip("Time between alt fire shots")] [SerializeField]
    private float altWaitBetweenShots;

    [Tooltip("Damage done by the tommy alt flash")]
    public int altFireDamage = 20;

    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.TommyName;
        //Sets the image for the tommy flash to be clear

        //Left shifts the layer num  to represent layer number by a single bit in the 32-bit integer
    }
    #endregion

    #region Methods
    /// <summary>
    /// Overrides the alt fire function of weapon module - Tommy Flash
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();
        StartCoroutine(TommyRapidFire());
        
    }

    IEnumerator TommyRapidFire()
    {
        ReloadWeapon();
        Duration savedDelay = new Duration(firingDelay.maxTime);
        firingDelay.maxTime = altWaitBetweenShots;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 24; i++)    //Had to make it 22 because at 20 there were 2 bullets left??
        {
            TryFireWeapon();
            yield return new WaitForSeconds(altWaitBetweenShots);
        }
        ReloadWeapon();
        firingDelay = savedDelay;
    }
    #endregion
}
