using System.Collections;
using System.Collections.Generic;
using Fungus;
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

    [Tooltip("Checks if Tommy is alting")]
    [SerializeField] public bool isAlting { get;  private set; }
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
    public override void AltFire()
    {
        //base.AltFire();
        StartCoroutine(TommyRapidFire());
        
    }

    public override void ResetAlt()
    {
        StopAllCoroutines();
        isAlting = false;
    }

    IEnumerator TommyRapidFire()
    {
        if (!isAlting)
        {
            isAlting = true;
            ReloadWeapon();
            yield return new WaitForSeconds(AudioDictionary.aDict.playerSource.clip.length);
            weaponAction = WeaponAudioStrings.Alt;
            altFireDelay.Reset();
            PlayAudio();
            yield return new WaitForSeconds(AudioDictionary.aDict.playerSource.clip.length);
            Duration savedDelay = new Duration(firingDelay.maxTime);
            firingDelay.maxTime = altWaitBetweenShots;
            //yield return new WaitForSeconds(0.5f);
            while (currentAmmo > 0)
            {
                TryFireWeapon();
                yield return new WaitForSeconds(altWaitBetweenShots);
            }
            ReloadWeapon();
            firingDelay = savedDelay;
            isAlting = false;
        }
    }
    #endregion
}
