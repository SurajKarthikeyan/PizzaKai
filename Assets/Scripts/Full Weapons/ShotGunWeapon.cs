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
    private bool isDashing = false;
    [SerializeField] private int dashDamage = 3;
    private CharacterMovementModule character;
    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        weaponName = WeaponAudioStrings.ShotgunName;
        character = Master.GetComponent<CharacterMovementModule>();
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
        // Gets the player mouse position and sends the player in the opposite
        // direction.
        Vector3 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir.z = 0;
        dir.y = 0;

        dir.Normalize();

        // !IMPORTANT! NEVER set velocity directly. Instead, use AddForce with
        // !ForceMode2D.Impulse. Setting velocity directly causes a race
        // !condition with other things that may be modifying velocity.
        Master.r2d.AddForce(dir * pushPower, ForceMode2D.Impulse);

        character.isShotgunDashing = true;
        Master.gameObject.layer = 21;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Breadstick") && character.isShotgunDashing)
        {
            if(collision.gameObject.GetComponent<EnemyBasic>().currentHP <= dashDamage)
            {
                dashReset = true;
            }
            collision.gameObject.GetComponent<EnemyBasic>().TakeDamage(dashDamage);
        }
    }

    /*
    /// <summary>
    /// Sends the player flying as part of the alt fire
    /// </summary>
    private void PushPlayer()
    {
        // Gets the player mouse position and sends the player in the opposite
        // direction.
        Vector3 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir.z = 0;
        
        dir.Normalize();
        // !IMPORTANT! NEVER set velocity directly. Instead, use AddForce with
        // !ForceMode2D.Impulse. Setting velocity directly causes a race
        // !condition with other things that may be modifying velocity.
        Master.r2d.AddForce(dir * pushPower, ForceMode2D.Impulse);
    }
    */
    #endregion


}
