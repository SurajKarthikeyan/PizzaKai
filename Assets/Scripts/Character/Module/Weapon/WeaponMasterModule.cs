using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

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

    [Tooltip("Which weapon is the character holding?")]
    public int weaponIndex;

    [Header("Animation Settings")]
    public Animator characterAnimator;

    [Tooltip("Animator Parameter - Fire")]
    [AnimatorParam(nameof(characterAnimator), AnimatorControllerParameterType.Bool)]
    [SerializeField]
    private string animParamFire;
    #endregion

    #region Properties
    public WeaponModule CurrentWeapon => weapons[weaponIndex];
    #endregion

    #region Methods
    #region Instantiation
    private void Awake()
    {
        SetComponents();
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

            if (Input.GetMouseButton(0))
            {
                // Firing.
                TryFire();
            }
            else
            {
                CurrentWeapon.ResetBurst();
            }
            if (Input.GetMouseButton(1))
            {
                //Alt fire
                TryAltFire();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                //Reload Current Weapon
                TryReload();
            }

            //Next two if blocks cycle through weapons
            if (Input.mouseScrollDelta.y > 0)
            {
                NextWeapon();
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                PrevWeapon();
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
    #endregion

    #region Firing
    /// <summary>
    /// Tries to fire the weapon
    /// </summary>
    /// <returns>true if able to fire, false if unable</returns>
    public bool TryFire()
    {
        return CurrentWeapon.TryFireWeapon();
    }

    /// <summary>
    /// Tries to alt fire the weapon
    /// </summary>
    public void TryAltFire()
    {
        if (CurrentWeapon.altFireDelay.IsDone)
        {
            CurrentWeapon.AltFire();
        }
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
        weaponIndex++;
        weaponIndex %= weapons.Count;

        EnableCurrentWeapon();
    }

    /// <summary>
    /// Switches to the previous weapon.
    /// </summary>
    public void PrevWeapon()
    {
        if (weaponIndex > 0)
        {
            weaponIndex--;
        }
        else
        {
            weaponIndex = weapons.Count - 1;
        }
        weaponIndex %= weapons.Count;

        EnableCurrentWeapon();
    }

    /// <summary>
    /// Enables current weapon the player has selected.
    /// </summary>
    public void EnableCurrentWeapon()
    {
        weapons.ForEach(weap => weap.gameObject.SetActive(false));

        CurrentWeapon.gameObject.SetActive(true);
    }
    #endregion

    #region Weapon Management
    /// <summary>
    /// Adds <paramref name="weapon"/>, which is an instance, to the weapon master.
    /// </summary>
    /// <param name="weapon">The weapon to add.</param>
    public void AddWeapon(WeaponModule weapon)
    {
        weapon.transform.Localize(transform);
        weapons.Add(weapon);
    }
    #endregion
    #endregion
}