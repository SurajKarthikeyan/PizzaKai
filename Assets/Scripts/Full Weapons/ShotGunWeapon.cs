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
        StartCoroutine("ShotgunAltInfluence");
        player.velocity = dir * pushPower;
    }
    #endregion
    #region Enums
    //Determines how long the alt fire's movement wil have influence over the player
    //Created so that the alt fire function normally when the player is not moving
    private IEnumerator ShotgunAltInfluence()
    {
        player.GetComponent<CharacterMovementModule>().shotgunPushed = true;

        //wait for an amont of seconds to determine if grounded
        yield return new WaitForSeconds(0.25f);
        //if grounded end alt fire influence
        if (player.GetComponent<CharacterMovementModule>().TouchingGround)
        {
            player.GetComponent<CharacterMovementModule>().shotgunPushed = false;
        }
        //if not grounded wait another amount of seconds to disable influence
        else
        {
            yield return new WaitForSeconds(0.75f);
            player.GetComponent<CharacterMovementModule>().shotgunPushed = false;
        }

    }
    #endregion

}
