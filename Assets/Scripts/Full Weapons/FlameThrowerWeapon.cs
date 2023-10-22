using System;
using Unity.VisualScripting;
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
    public AltFlameProjectile altFireball;
    public AudioSource flameSource;
    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.FlamethrowerName;
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Overrides the alt fire function of weapon module - Flame Shot
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();
        altFireball.Spawn(this);
    }

    override public void PlayAudio()
    {
        if (weaponAction == WeaponAudioStrings.Shoot)
        {
            if (flameSource.isPlaying)
            {
                return;
            }
            AudioDictionary.aDict.PlayAudioClipRemote(weaponName + weaponAction, flameSource);
        }
        else
        {
            flameSource.Stop();
            base.PlayAudio();
        }
    }

    private void OnMouseUp()
    {
        flameSource.Stop();
    }

    #endregion
}
