using System.Collections;
using UnityEngine;

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
    [Tooltip("Time between alt fire shots")] [SerializeField]
    private float altWaitBetweenShots;

    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.TommyName;
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

    private IEnumerator TommyRapidFire()
    {
        ReloadWeapon();
        yield return new WaitForSeconds(AudioDictionary.aDict.playerSource.clip.length);
        weaponAction = WeaponAudioStrings.Alt;
        altFireDelay.Reset();
        PlayAudio();
        yield return new WaitForSeconds(AudioDictionary.aDict.playerSource.clip.length);
        Duration savedDelay = new Duration(firingDelay.maxTime);
        firingDelay.maxTime = altWaitBetweenShots;
        //yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 24; i++)    //Had to make it 22 because at 20 there were 2 bullets left??
        {
            PressTrigger();
            yield return new WaitForSeconds(altWaitBetweenShots);
        }
        //ReloadWeapon();
        firingDelay = savedDelay;
    }
    #endregion
}
