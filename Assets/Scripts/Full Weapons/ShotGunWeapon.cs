using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the Shotgun, child class of WeaponModule
/// 
/// Primarily used for player character
/// 
/// <br/>
/// 
/// Authors: Zane O'Dell (2023)
/// </summary>
public class ShotGunWeapon : WeaponModule
{
    #region Variables
    [Header("Alt Fire Settings")]
    [Tooltip("Power with which the player is sent flying")]
    [SerializeField]
    private float pushPower = 20f;

    [Tooltip("Rigidbody of player to use for alt fire")]
    public Rigidbody2D player;
    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.ShotgunName;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Overrides the alt fire function of weapon module - Shotgun Movement Blast
    /// </summary>
    override public void AltFire()
    {
        base.AltFire();
        PushPlayer();
    }

    /// <summary>
    /// Sends the player flying as part of the alt fire
    /// </summary>
    private void PushPlayer()
    {
        //Gets the player mouse position and sends the player in the opposite direction
        Vector3 dir = Vector3.Normalize(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // player.gameObject.GetComponent<CharacterMovementModule>().ShotgunAltActive();
        player.velocity = dir * pushPower;
    }
    #endregion
 

}
