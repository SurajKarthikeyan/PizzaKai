using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls weapon switching, aiming, etc for the player.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang, Zane O'Dell (2023)
/// </summary>
public class WeaponMasterModule : Module
{
    #region Variables
    [Header("Weapon Settings")]
    [Tooltip("The weapons available to the weapon master.")]
    public List<WeaponModule> weapons;

    [Tooltip("The weapons available to the player.")]
    [ReadOnly]
    public List<WeaponModule> availableWeapons;

    [Tooltip("Which weapon is the character holding?")]
    public int weaponIndex;

    [Header("Animation Settings")]
    public Animator characterAnimator;

    [Tooltip("Animator Parameter - Fire")]
    [AnimatorParam(nameof(characterAnimator), AnimatorControllerParameterType.Bool)]
    [SerializeField]
    private string animParamFire;

    public bool isGrappling = false;

    [SerializeField]
    private bool weaponsAvailable;
    #endregion

    #region Properties
    public WeaponModule CurrentWeapon => availableWeapons[weaponIndex];
    #endregion

    #region Methods
    #region Instantiation
    private void Awake()
    {
        foreach(var weapon in weapons)
        {
            if (weapon.accessible)
            {
                availableWeapons.Add(weapon);
            }
        }
        SetComponents();
    }

    private void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex != 0 && sceneIndex != 1)
        {
            weaponsAvailable = true;
        }
        else
        {
            weaponsAvailable = false;
        }
    }

    private void OnValidate()
    {
        SetComponents();
    }

    /// <summary>
    /// Sets components for master module
    /// </summary>
    private void SetComponents()
    {
        GetComponentsInChildren(true, weapons);
    }
    #endregion

    #region Main Loop
    public void Update()
    {
        if (Master.IsPlayer)
        {
            // Do player aiming.
            Vector2 mousePos = Input.mousePosition;
            AimAt(Camera.main.ScreenToWorldPoint(mousePos));

            if (weaponsAvailable)
            {
                if (Input.GetMouseButton(0))
                {
                    // Firing.
                    TryFire();
                }
                else
                {
                    CurrentWeapon.ReleaseTrigger();
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    //Alt fire
                    TryAltFire();
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    TryAltFireUp();
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    //Reload Current Weapon
                    TryReload();
                }

                //Next two if blocks cycle through weapons
                if (Input.mouseScrollDelta.y >= 1f)
                {
                    NextWeapon();
                    Master.GetComponent<CharacterMovementModule>().isShotgunDashing = false;
                    Master.gameObject.layer = 7;
                }
                if (Input.mouseScrollDelta.y <= -1f)
                {
                    PrevWeapon();
                    Master.GetComponent<CharacterMovementModule>().isShotgunDashing = false;
                    Master.gameObject.layer = 7;
                }

                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    SwitchToWeapon(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    SwitchToWeapon(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    SwitchToWeapon(2);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    SwitchToWeapon(3);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                {
                    SwitchToWeapon(4);
                }
            }
        }
    }
    #endregion

    #region Aiming
    /// <summary>
    /// Aims the weapons at <paramref name="target"/>.
    /// </summary>
    /// <param name="target">Where to aim the weapons.</param>
    public void AimAt(Vector2 target)
    {
        Debug.DrawLine(target, transform.position, Color.blue);
        if (!isGrappling)
        {
            // Do some basic trig to get the weapons pointed at the target.
            Vector2 disp = target - (Vector2)transform.position;
            float zRot = Mathf.Atan2(disp.y, disp.x) * Mathf.Rad2Deg;


            // Send data to the flip module.

            Master.SetLookAngle(zRot);
            var scale = transform.localScale;
            scale.x = Master.flipModule.FlipMultiplier;
            scale.y = Master.flipModule.FlipMultiplier;
            transform.localScale = scale;


            Vector3 eulerAngles = new(
                0,
                0,
                zRot
            );

            transform.eulerAngles = eulerAngles;
        }
    }
    #endregion

    #region Firing
    /// <summary>
    /// Tries to fire the weapon
    /// </summary>
    /// <returns>true if able to fire, false if unable</returns>
    public bool TryFire()
    {
        if (!isGrappling)
        {
            return CurrentWeapon.TryFireWeapon();
        }
        return false;
    }

    /// <summary>
    /// Tries to alt fire the weapon
    /// </summary>
    public void TryAltFire()
    {
        if (CurrentWeapon.altFireDelay.IsDone || CurrentWeapon.dashReset)
        {
            if(CurrentWeapon.dashReset)
            {
                CurrentWeapon.dashReset = false;
            }
            CurrentWeapon.AltFire();
        }
    }

    public void TryAltFireUp()
    {
        CurrentWeapon.AltFireKeyUp();
    }
    #endregion

    #region Reloading
    /// <summary>
    /// Tries to reload the weapon
    /// </summary>
    public void TryReload()
    {
        CurrentWeapon.ReloadWeapon();
    }
    #endregion

    #region Weapon Switching
    /// <summary>
    /// Switches to the next weapon.
    /// </summary>
    public void NextWeapon()
    {
        CurrentWeapon.ResetAlt();
        SwitchToWeapon(++weaponIndex);
    }

    /// <summary>
    /// Switches to the previous weapon.
    /// </summary>
    public void PrevWeapon()
    {
        CurrentWeapon.ResetAlt();
        SwitchToWeapon(--weaponIndex);
    }

    /// <summary>
    /// Switches to the specified weapon index.
    /// </summary>
    /// <param name="index">The new weapon index. Will be wrapped so it remains
    /// within range of <see cref="weapons"/>.</param>
    public void SwitchToWeapon(int index)
    {
        weaponIndex = index.WrapAroundLength(availableWeapons);
        EnableCurrentWeapon();
    }

    /// <summary>
    /// Enables current weapon the player has selected.
    /// </summary>
    public void EnableCurrentWeapon()
    {
        availableWeapons.ForEach(weap => weap.gameObject.SetActive(false));
        CurrentWeapon.ResetAlt();
        CurrentWeapon.gameObject.SetActive(true);
    }
    #endregion

    #region Weapon Management
    /// <summary>
    /// Adds <paramref name="weapon"/>, which is an instance, to the weapon master.
    /// </summary>
    /// <param name="weapon">The weapon to add.</param>
    public WeaponModule AddWeapon(WeaponModule weapon)
    {
        weapon.transform.Localize(transform);
        weapon.accessible = true;
        availableWeapons.Add(weapon);
        SwitchToWeapon(availableWeapons.Count - 1);
        return weapon;
    }

    public void SetWeaponsAvailable(bool available)
    {
        weaponsAvailable = available;
    }
    #endregion
    #endregion
}