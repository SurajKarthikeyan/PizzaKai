using UnityEngine;
using NaughtyAttributes;
using System;

/// <summary>
/// Represents a singular weapon. This can be used by both players and enemies.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang, Zane O'Dell (2023)
/// </summary>
public class WeaponModule : Module
{
    #region Classes
    /// <summary>
    /// A weapon mode splits the behaviors of the alt and normal firing modes
    /// into two separate objects. This will allow for the addition of cleaner
    /// code, as well as an arbitrary number of future alt fires.
    ///
    /// <br/>
    ///
    /// Authors: Ryan Chang (2023)
    /// </summary>
    [System.Serializable]
    public class WeaponMode
    {
        [Header("General")]
        [Tooltip("Name of this mode.")]
        [OnValueChanged(nameof(UpdateFakeKey))]
        public string name;

        private void UpdateFakeKey()
        {
            key = name;
        }

        [SerializeField]
        [HideInInspector]
        private string key;

        /// <summary>
        /// Used to make the animator show up.
        /// </summary>
        private Animator fakeAnimator;

        [Header("Animation")]
        [Tooltip("The name of the animation parameter that controls firing.")]
        [AnimatorParam(nameof(fakeAnimator), AnimatorControllerParameterType.Bool)]
        public string animFiringBool;

        [Tooltip("The name of the animation parameter that controls reloading.")]
        [AnimatorParam(nameof(fakeAnimator), AnimatorControllerParameterType.Bool)]
        public string animReloadingBool;

        [Header("Audio")]
        [Tooltip("The audio clip to play when the weapon is fired.")]
        public AudioClip firingClip;

        [Tooltip("The audio clip to play when the weapon is reloading.")]
        public AudioClip reloadClip;


        [Header("Firing Settings")]
        [Tooltip("Updated bullet spawn this weapon uses.")]
        public WeaponSpawn bulletSpawn;

        [Tooltip("The point where the projectiles originate from.")]
        public Transform firePoint;

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

        public void SetAnimatorPreview(Animator anim) => fakeAnimator = anim;
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
    [Tooltip("Describes the current status of the weapon.")]
    [ReadOnly]
    [SerializeField]
    private WeaponInputState inputState;

    [Tooltip("The defined subweapons.")]
    [SerializeField]
    private WeaponMode[] firingModes;

    [Header("Animation")]
    [Tooltip("The animator for the weapon")]
    public Animator weaponAnimator;

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


    [Header("UI")]
    [Tooltip("The graphic to use for the ammo in the UI.")]
    public Sprite ammoGraphic;


    [Header("Firing Settings")]
    [Tooltip("The point where the projectiles originate from.")]
    public Transform firePoint;

    [Tooltip("If true, keep firing the weapon when the trigger is held down.")]
    public bool autofire = true;

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
    public bulletScript bullet;

    [Tooltip("Updated bullet spawn this weapon uses.")]
    public WeaponSpawn bulletSpawn;
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
    }

    private void OnValidate()
    {
        if (!firingModes.IsNullOrEmpty())
        {
            foreach (var mode in firingModes)
            {
                mode.SetAnimatorPreview(weaponAnimator);
            }
        }
    }
    #endregion

    #region Parameters
    /// <summary>
    /// True if <see cref="currentAmmo"/> is zero and if the input state is not reloading.
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
    private void Update()
    {
        // Always increment firing delay.
        firingDelay.IncrementUpdate(false);

        if (!altFireDelay.IsDone) altFireDelay.IncrementUpdate(false);

        // Determine if reload has completed.
        if (InputState == WeaponInputState.Reloading)
        {
            if (reloadDelay.IncrementUpdate(true))
            {
                InputState = WeaponInputState.Idle;
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

        if (canFire)
        {
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
        }

        // Set animations.
        if (weaponAnimator && !string.IsNullOrWhiteSpace(animFiringBool))
        {
            weaponAnimator.SetBool(animFiringBool, canFire);
        }

        return canFire;
    }

    public void ResetBurst() => burstCount = 0;

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
        bulletSpawn.Spawn(this).Fire(this);
    }

    /// <summary>
    /// Handles the alt fire for each of the weapons
    /// </summary>
    public virtual void AltFire()
    {
        altFireDelay.Reset();
        Debug.Log("Doing alt fire");
    }

    #endregion

    #region Weapon Reloading
    /// <summary>
    /// Complete process of reloading
    /// </summary>
    public void ReloadWeapon()
    {
        InputState = WeaponInputState.Reloading;

        if (InputState == WeaponInputState.Reloading)
        {
            reloadDelay.Reset();
            RefillAmmo();
        }
    }

    /// <summary>
    /// Refills the current weapon's current ammo count
    /// </summary>
    private void RefillAmmo()
    {
        currentAmmo = ammoCount;
    }
    #endregion
    #endregion
}