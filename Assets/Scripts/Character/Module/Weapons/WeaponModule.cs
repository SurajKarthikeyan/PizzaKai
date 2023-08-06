using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Represents a singular weapon.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class WeaponModule : Module
{
    #region Enums
    /// <summary>
    /// Describes the current status of the weapon.
    /// </summary>
    public enum WeaponState
    {
        /// <summary>
        /// When no inputs are provided to the weapon, or if the Firing input is
        /// provided but there is no ammo left.
        /// </summary>
        Idle,
        /// <summary>
        /// When the Firing input is provided to the weapon and the weapon is
        /// able to be fired.
        /// </summary>
        Firing,
        /// <summary>
        /// When the Reload input is provided to the weapon and the weapon is
        /// able to be reloaded.
        /// </summary>
        Reloading
    }
    #endregion

    #region Variables
    #region User Settings
    /// <inheritdoc cref="WeaponState"/>
    [Header("Global")]
    [Tooltip("Describes the current status of the weapon.")]
    public WeaponState weaponStatus;

    [Header("Animation")]
    [Tooltip("The animator for the weapon")]
    public Animator weaponAnimator;

    [Tooltip("The name of the animation parameter that controls firing.")]
    [AnimatorParam(nameof(weaponAnimator), AnimatorControllerParameterType.Bool)]
    public string animFiringBool;

    [Tooltip("The name of the animation parameter that controls reloading.")]
    [AnimatorParam(nameof(weaponAnimator), AnimatorControllerParameterType.Bool)]
    public string animReloadingBool;

    [Header("Firing Settings")]
    [Tooltip("The point where the projects originate from.")]
    public Transform firePoint;

    [Tooltip("If true, keep firing the weapon when the trigger is held down.")]
    public bool autofire = true;

    [Tooltip("How long to wait between rounds fired.")]
    public Duration firingDelay = new(1f);

    [Header("Ammo/Reloading Settings")]
    [Tooltip("The number of bullets/projectiles/etc per clip/mag/whatever.")]
    public int ammoCount = 10;

    [Tooltip("How much ammo is currently in the weapon?")]
    [ReadOnly]
    public int currentAmmo;

    [Tooltip("How long is the reload?")]
    public Duration reloadDelay = new(1f);
    #endregion
    #endregion

    #region Parameters

    #endregion

    #region Methods
    #region Instantiation

    #endregion

    #region Main Loop
    private void Update()
    {
        // Increment the delays.
        firingDelay.IncrementUpdate();

        // Determine if reload has completed.
        if (weaponStatus == WeaponState.Reloading)
        {
            if (reloadDelay.IncrementUpdate())
            {
                reloadDelay.Reset();
                weaponStatus = WeaponState.Idle;
            }
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Attempts to fire the weapon.
    /// </summary>
    /// <returns>True if firing was successful, false otherwise.</returns>
    public bool FireWeapon()
    {
        switch (weaponStatus)
        {
            case WeaponState.Idle:
                return FiringDelayDone();
            case WeaponState.Firing:
                // If autofire is disabled, then do not allow the weapon to be
                // fired.
                return autofire && FiringDelayDone();
            default:
                return false;
        }
    }
    #endregion

    #region Helper Methods
    private bool FiringDelayDone()
    {
        if (firingDelay.IsDone)
        {
            firingDelay.Reset();
            return true;
        }

        return false;
    }
    #endregion
    #endregion
}