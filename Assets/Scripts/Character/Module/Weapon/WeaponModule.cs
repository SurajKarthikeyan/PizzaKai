using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Represents a singular weapon. This can be used by both players and enemies.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang, Zane O'Dell (2023)
/// </summary>
public class WeaponModule : Module
{
    #region Static Classes

    /// <summary>
    /// Class holding strings for playing weapon audio clips
    /// </summary>
    public readonly struct WeaponAudioStrings
    {
        //Strings for the weapon names
        public static readonly string TommyName = "tommy";
        public static readonly string ShotgunName = "shotgun";
        public static readonly string FlamethrowerName = "flamethrower";
        public static readonly string PistolName = "pistol";

        public static readonly string Shoot = "Shoot";
        public static readonly string Alt = "Alt";
        public static readonly string Reload = "Reload";
    }
    #endregion

    #region Enums
    /// <summary>
    /// Describes the current input status of the weapon.
    /// </summary>
    public enum WeaponInputState
    {
        /// <summary>
        /// When no inputs are provided to the weapon.
        /// </summary>
        Idle,
        /// <summary>
        /// The first frame of firing.
        /// </summary>
        FiringSingle,
        /// <summary>
        /// The later frames of firing. Used for auto-fire.
        /// </summary>
        FiringHeld,
        /// <summary>
        /// When the Reload input is provided to the weapon.
        /// </summary>
        Reloading,
        /// <summary>
        /// When the alt fire input is provided to the weapon
        /// </summary>
        Alting
    }

    #endregion

    #region Variables
    #region User Settings
    #region Global
    [Header("Global")]
    [Tooltip("The ID of the weapon. Used to determine what " +
    "weapons the player has.")]
    public string weaponID = "[Not Set]";

    /// <inheritdoc cref="WeaponInputState"/>
    [Tooltip("Describes the current status of the weapon.")]
    [ReadOnly]
    [SerializeField]
    private WeaponInputState inputState;

    [Tooltip("Bool stating whether this player has access to weapon or not")]
    public bool accessible = false;
    #endregion

    #region Animation
    [Header("Animation")]
    [Tooltip("The animator for the weapon")]
    public Animator weaponAnimator;

    [Tooltip("The animator for the player")]
    public Animator playerAnimator;

    [Tooltip("The name of the animation parameter that controls firing.")]
    [AnimatorParam(nameof(weaponAnimator), AnimatorControllerParameterType.Bool)]
    public string animFiringBool;

    [Tooltip("The name of the animation parameter that controls alt firing.")]
    [AnimatorParam(nameof(weaponAnimator), AnimatorControllerParameterType.Bool)]
    public string animAltingBool;

    [Tooltip("The name of the animation parameter that controls reloading.")]
    [AnimatorParam(nameof(weaponAnimator), AnimatorControllerParameterType.Bool)]
    public string animReloadingBool;
    #endregion

    #region Audio
    [Header("Audio Source")]
    [Tooltip("Audio source for the weapon.")]
    public AudioSource weaponAudio;

    [Tooltip("The audio clip to play when the weapon is fired.")]
    public AudioClip firingClip;

    [Tooltip("The audio clip to play when the weapon is alt firing.")]
    public AudioClip altingClip;

    [Tooltip("The audio clip to play when the weapon is reloading.")]
    public AudioClip reloadClip;
    #endregion

    #region Firing
    [Header("Firing Settings")]
    [Tooltip("The point where the projectiles originate from.")]
    public Transform firePoint;

    [Tooltip("If true, keep firing the weapon when the trigger is held down.")]
    public bool autofire = true;

    [Tooltip("If true and weapon is autofire, then always fire the first " +
        "shot of any burst with no spread.")]
    [ShowIf(nameof(autofire))]
    public bool firstShotAccurate = true;

    [Tooltip("How long to wait between rounds fired.")]
    public Duration firingDelay = new(1f);

    [Tooltip("How long to wait between alt fires")]
    public Duration altFireDelay = new(1f);

    [Header("Ammo/Reloading Settings")]
    [Tooltip("The number of bullets/projectiles/etc per clip/mag/whatever.")]
    public int ammoCount = 10;

    [Tooltip("How much ammo is currently in the weapon?")]
    [ReadOnly]
    public int currentAmmo;

    [Tooltip("How long is the reload?")]
    public Duration reloadDelay = new(1f);

    [Tooltip("Bullet that this weapon uses")]
    public WeaponSpawn bullet;
    #endregion

    #region Audio Dictionary
    [Header("Audio Settings")]
    [Tooltip("Prefix used to help get proper audio clip from audio dict")]
    [ReadOnly]
    public string weaponName;

    [Tooltip("Action used to help get proper audio clip from audio dict")]
    [ReadOnly]
    public string weaponAction;
    #endregion

    #region UI
    [Header("UI Attributes")]
    public Sprite weaponUIImage;
    #endregion
    #endregion

    #region Autogenerated
    private int burstCount;
    #endregion
    #endregion

    #region Init
    /// <summary>
    /// Initializes Ammo count
    /// </summary>
    protected virtual void Start()
    {
        currentAmmo = ammoCount;

        if (weaponID == "[Not Set]")
        {
            Debug.LogError("WeaponID is not set from its default value", gameObject);
        }
    }
    #endregion

    #region Parameters
    /// <summary>
    /// True if <see cref="currentAmmo"/> is zero and if the input state is not
    /// reloading.
    /// </summary>
    public bool OutOfAmmo => currentAmmo <= 0 &&
        InputState != WeaponInputState.Reloading;

    /// <summary>
    /// True if the weapon is in either
    /// <see cref="WeaponInputState.FiringSingle"/> or
    /// <see cref="WeaponInputState.FiringHeld"/>.
    /// </summary>
    public bool PrimaryTriggerDown => InputState == WeaponInputState.FiringSingle ||
        InputState == WeaponInputState.FiringHeld;

    public WeaponInputState InputState
    {
        get => inputState;
        set
        {
            inputState = value;

            if (weaponAnimator)
            {
                if (!string.IsNullOrEmpty(animReloadingBool))
                {
                    weaponAnimator.SetBool(
                        animReloadingBool,
                        InputState == WeaponInputState.Reloading
                    );
                }

                if (!string.IsNullOrEmpty(animFiringBool))
                {
                    weaponAnimator.SetBool(
                        animFiringBool,
                        PrimaryTriggerDown
                    );

                    playerAnimator.SetBool(
                        animFiringBool,
                        PrimaryTriggerDown
                    );
                }

                if (!string.IsNullOrEmpty(animAltingBool))
                {
                    weaponAnimator.SetBool(
                        animAltingBool,
                        InputState == WeaponInputState.Alting
                    );
                }
                else if (!string.IsNullOrEmpty(animFiringBool))
                {
                    // No alting bool specified, but there is a firing bool.
                    // Use it instead.
                    weaponAnimator.SetBool(
                        animFiringBool,
                        PrimaryTriggerDown ||
                        InputState == WeaponInputState.Alting
                    );
                }
            }

            //if (weaponAudio)
            //{
            //    if (firingClip &&
            //        (InputState == WeaponInputState.FiringStart ||
            //        InputState == WeaponInputState.FiringHeld))
            //    {
            //        weaponAudio.PlayOneShot(firingClip);
            //    }

            //    if (reloadClip &&
            //        InputState == WeaponInputState.Reloading)
            //    {
            //        weaponAudio.PlayOneShot(reloadClip);
            //    }

            //    if (altingClip &&
            //        InputState == WeaponInputState.Alting)
            //    {
            //        weaponAudio.PlayOneShot(altingClip);
            //    }
            //}
        }
    }

    public int BurstCount => burstCount;
    #endregion

    #region Methods
    #region Main Loop
    /// <summary>
    /// Update is called every frame
    /// </summary>
    public virtual void Update()
    {
        // Always increment firing delay.
        firingDelay.IncrementUpdate(false);

        // Fire if trigger is held down.
        TryFireProjectile();

        if (!altFireDelay.IsDone) altFireDelay.IncrementUpdate(false);

        // Determine if reload has completed.
        if (InputState == WeaponInputState.Reloading &&
            reloadDelay.IncrementUpdate(true))
        {
            // Done reloading.
            RefillAmmo();
        }
    }
    #endregion

    #region Weapon Firing
    /// <summary>
    /// Attempts to fire the weapon. The attempt fails if the weapon is out of
    /// ammo, the weapon's fire delay has not yet expired, or if the weapon is
    /// not autofire and the trigger has not been released.
    /// </summary>
    /// <returns>True the weapon has fired.</returns>
    /// <remarks>
    /// This method has been renamed from TryFireWeapon, as its new name,
    /// PressTrigger, is more appropriate for its behavior.
    /// </remarks>
    public bool PressTrigger()
    {
        InputState = autofire ?
            WeaponInputState.FiringHeld :
            WeaponInputState.FiringSingle;

        return TryFireProjectile();
    }

    /// <summary>
    /// Releases the trigger, causing the gun to stop firing. This needs to be
    /// called between instances of <see cref="PressTrigger"/> if <see
    /// cref="autofire"/> is false.
    /// </summary>
    public void ReleaseTrigger()
    {
        if (PrimaryTriggerDown)
        {
            // Only release trigger if actually firing.
            InputState = WeaponInputState.Idle;
            burstCount = 0;
        }
    }

    /// <summary>
    /// Checks if the weapon can fire.
    /// </summary>
    /// <returns>True if the weapon can fire, false otherwise.</returns>
    private bool CheckCanFire()
    {
        if (OutOfAmmo)
        {
            ReloadWeapon();
            return false;
        }

        switch (InputState)
        {
            case WeaponInputState.FiringSingle:
                return firingDelay.IsDone && BurstCount == 0;
            case WeaponInputState.FiringHeld:
                // If autofire is disabled, then do not allow the weapon to be fired
                return firingDelay.IsDone;
            default:
                return false;
        }
    }

    /// <summary>
    /// Tries to fire the projectile. Fails if firing delay has
    /// not expired, if out of ammo, etc.
    /// </summary>
    protected bool TryFireProjectile()
    {
        bool canFire = CheckCanFire();

        if (!canFire)
            return false;

        // Reset timer.
        firingDelay.Reset();

        PlayAudio();

        weaponAction = WeaponAudioStrings.Shoot;
        burstCount++;
        currentAmmo -= 1;
        bullet.Spawn(this);

        return true;
    }

    /// <summary>
    /// Handles the alt fire for each of the weapons
    /// </summary>
    public virtual void AltFire()
    {
        InputState = WeaponInputState.Alting;
        weaponAction = WeaponAudioStrings.Alt;
        altFireDelay.Reset();
        PlayAudio();
    }

    public virtual void AltFireKeyUp()
    {

    }

    #endregion

    #region Weapon Reloading
    /// <summary>
    /// Start process of reloading. Weapon is locked (cannot fire) whist
    /// reloading.
    /// </summary>
    public virtual void ReloadWeapon()
    {
        InputState = WeaponInputState.Reloading;
        weaponAction = WeaponAudioStrings.Reload;

        PlayAudio();
    }

    /// <summary>
    /// Refills the current weapon's current ammo count.
    /// </summary>
    public void RefillAmmo()
    {
        InputState = WeaponInputState.Idle;
        reloadDelay.Reset();
        burstCount = 0;
        currentAmmo = ammoCount;
    }
    #endregion

    #region Weapon Audio

    /// <summary>
    /// Plays the audio clips corresponding to the current weapon
    /// </summary>
    public virtual void PlayAudio()
    {
        //AudioDictionary.aDict.PlayAudioClip(weaponName + weaponAction, AudioDictionary.Source.Player);
    }
    #endregion

    #endregion
}