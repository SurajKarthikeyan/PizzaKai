using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
public class PistolWeapon : WeaponModule
{
    #region Variables
    
    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.PistolName;
        
     
    }
    #endregion

    #region Methods
    /// <summary>
    /// Overrides the alt fire function of weapon module - Tommy Flash
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();

        //// Probably fix the camera shake, but leave it here for now.
        ////StartCoroutine(cameraScript.Shake());
        //StartCoroutine(tommyAltFlash());

        //FindEnemies();

        ////TODO: GetComponent is expensive, maybe look into optimizing this. 
        //foreach (Collider2D enemy in visibleEnemies)
        //{
        //    enemy.GetComponent<EnemyBasic>().currentHP -= altFireDamage;
        //}
    }

    #endregion
}
