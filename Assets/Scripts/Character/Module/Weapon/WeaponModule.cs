using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

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
        FiringStart,
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
    /// <inheritdoc cref="WeaponInputState"/>
    [Header("Global")]
    [Tooltip("The ID of the weapon. Used to determine what " +
        "weapons the player has.")]
    public string weaponID = "[Not Set]";

    [Tooltip("Bool stating whether this player has access to weapon or not")]
    public bool accessible = false;

    [Tooltip("Describes the current status of the weapon.")]
    [ReadOnly]
    [SerializeField]
    private WeaponInputState inputState;

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

    [Header("Audio Source")]
    [Tooltip("Audio source for the weapon.")]
    public AudioSource weaponAudio;

    [Tooltip("The audio clip to play when the weapon is fired.")]
    public AudioClip firingClip;

    [Tooltip("The audio clip to play when the weapon is alt firing.")]
    public AudioClip altingClip;

    [Tooltip("The audio clip to play when the weapon is reloading.")]
    public AudioClip reloadClip;


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

    [Header("HDR Color Settings")]
    [Tooltip("HDR Color for URP Shading CURRENTLT TESTING")]
    [ColorUsage(true, true)]
    public Color HDRColorIntense;
    
    [Tooltip("HDR Color for URP Shading CURRENTLT TESTING")]
    [ColorUsage(true, true)]
    public Color HDRColorDefault;

    //[Tooltip("Updated bullet spawn this weapon uses.")]
    //public bulletScript bulletSpawn;

    [Header("Audio Settings")]
    [Tooltip("Prefix used to help get proper audio clip from audio dict")]
    [ReadOnly]
    public string weaponName;

    [Tooltip("Action used to help get proper audio clip from audio dict")]
    [ReadOnly]
    public string weaponAction;

    [Header("UI Attributes")]
    public Sprite weaponUIImage;

    protected WeaponMasterModule weaponMaster;
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
        weaponMaster = GetComponentInParent<WeaponMasterModule>();
    }
    #endregion

    #region Parameters
    /// <summary>
    /// True if <see cref="currentAmmo"/> is zero and if the input state is not
    /// reloading.
    /// </summary>
    public bool OutOfAmmo => currentAmmo <= 0 &&
        InputState != WeaponInputState.Reloading;

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
                        InputState == WeaponInputState.FiringStart ||
                        InputState == WeaponInputState.FiringHeld
                    );

                    playerAnimator.SetBool(
                        animFiringBool,
                        InputState == WeaponInputState.FiringStart ||
                        InputState == WeaponInputState.FiringHeld
                    );
                }

                if (!string.IsNullOrEmpty(animAltingBool))
                {
                    weaponAnimator.SetBool(
                        animAltingBool,
                        InputState == WeaponInputState.Alting
                    );
                }
            }

            if (weaponAudio)
            {
                if (firingClip &&
                    (InputState == WeaponInputState.FiringStart ||
                    InputState == WeaponInputState.FiringHeld))
                {
                    weaponAudio.PlayOneShot(firingClip);
                }

                if (reloadClip &&
                    InputState == WeaponInputState.Reloading)
                {
                    weaponAudio.PlayOneShot(reloadClip);
                }

                if (altingClip &&
                    InputState == WeaponInputState.Alting)
                {
                    weaponAudio.PlayOneShot(altingClip);
                }
            }
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

        if (!altFireDelay.IsDone) altFireDelay.IncrementUpdate(false);

        // Determine if reload has completed.
        if (InputState == WeaponInputState.Reloading)
        {
            if (reloadDelay.IncrementUpdate(true))
            {
                // Done reloading.
                RefillAmmo();
            }
        }
    }
    #endregion

    #region Weapon Firing
    /// <summary>
    /// Attempts to fire the weapon.
    /// </summary>
    /// <returns>True if firing was successful, false otherwise.</returns>
    public bool TryFireWeapon()
    {
        bool canFire = CheckCanFire();
        Material weaponLitMat = weaponAnimator.gameObject.GetComponent<SpriteRenderer>().material;

        if (canFire)
        {
            
            //___________________TEST VFX SECTION__________________//
            weaponLitMat.color = HDRColorIntense;
            weaponAnimator.gameObject.GetComponent<SpriteRenderer>().material = weaponLitMat;
            //__________________END TEST VFX SEC___________________//
            firingDelay.Reset();

            switch (InputState)
            {
                // If the player is already firing, then we go to the firing
                // start state, otherwise we start firing.
                case WeaponInputState.FiringStart:
                    InputState = WeaponInputState.FiringHeld;
                    break;
                default:
                    InputState = WeaponInputState.FiringStart;
                    break;
            }

            // Actually fire now.
            FireProjectile();
            currentAmmo -= 1;
            PlayAudio();
        }

        // Set animations.
        if (weaponAnimator && !string.IsNullOrWhiteSpace(animFiringBool))
        {
            weaponAnimator.SetBool(animFiringBool, canFire);
        }

        if (playerAnimator && !string.IsNullOrWhiteSpace(animFiringBool))
        {
            playerAnimator.SetBool(animFiringBool, canFire);
        }
        //___________________TEST VFX SECTION__________________//
        weaponLitMat.color = HDRColorDefault;
        weaponAnimator.gameObject.GetComponent<SpriteRenderer>().material = weaponLitMat;
        //__________________END TEST VFX SEC___________________//
        return canFire;
        
    }

    /// <summary>
    /// Called by <see cref="WeaponMasterModule"/> to stop firing.
    /// </summary>
    public void ReleaseTrigger()
    {
        if (InputState == WeaponInputState.FiringStart ||
            InputState == WeaponInputState.FiringHeld)
        {
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
            case WeaponInputState.Idle:
                return firingDelay.IsDone;
            case WeaponInputState.FiringStart:
            case WeaponInputState.FiringHeld:
                // If autofire is disabled, then do not allow the weapon to be fired
                return firingDelay.IsDone && autofire;
            default:
                return false;
        }
    }

    /// <summary>
    /// Actually fires the weapon.
    /// </summary>
    protected void FireProjectile()
    {
        //Spawned projectile, need to look into refactoring bullets themselves
        weaponAction = WeaponAudioStrings.Shoot;

        burstCount++;

        bullet.Spawn(this);
    }

    /// <summary>
    /// Handles the alt fire for each of the weapons
    /// </summary>
    public virtual void AltFire()
    {
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
        AudioDictionary.aDict.PlayAudioClip(weaponName + weaponAction, AudioDictionary.Source.Player);
    }
    #endregion

    #endregion
}