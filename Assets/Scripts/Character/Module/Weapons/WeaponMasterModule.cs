using System;
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
    [ReadOnly]
    public List<WeaponAbstractionModule> weapons;

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
    public bool HasWeapon => !weapons.IsNullOrEmpty();

    public WeaponAbstractionModule CurrentWeapon => weapons[weaponIndex];

    /// <summary>
    /// What was the last mouse button to be pressed?
    /// </summary>
    public int LastMouseBtnClick { get; private set; }
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
    private void Update()
    {
        if (Master.IsPlayer)
        {
            // Do player aiming.
            Vector2 mousePos = Input.mousePosition;
            AimAt(Camera.main.ScreenToWorldPoint(mousePos));

            // Parse mouse inputs.
            ParseMouseInput(0);
            ParseMouseInput(1);

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

    private void ParseMouseInput(int mouseBtn)
    {
        if (HasWeapon)
        {
            LastMouseBtnClick = mouseBtn;
    
            if (Input.GetMouseButton(mouseBtn))
            {
                CurrentWeapon.TryFireWeapon();
            }
            else
            {
                CurrentWeapon.ResetBurst();
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
        scale.x = Master.flipModule.Flipped;
        scale.y = Master.flipModule.Flipped;
        transform.localScale = scale;

        Vector3 eulerAngles = new(
            0,
            0,
            zRot
        );

        transform.eulerAngles = eulerAngles;
    }
    #endregion

    #region Reloading
    /// <summary>
    /// Tries to reload the weapon
    /// </summary>
    public void TryReload()
    {
        CurrentWeapon.TryReloadWeapon();
    }
    #endregion

    #region Weapon Switching
    /// <summary>
    /// Switches to the next weapon.
    /// </summary>
    public void NextWeapon()
    {
        NthWeapon(weaponIndex + 1);
    }

    /// <summary>
    /// Switches to the previous weapon.
    /// </summary>
    public void PrevWeapon()
    {
        NthWeapon(weaponIndex - 1);
    }

    /// <summary>
    /// Switches to the nth weapon, wrapping if necessary.
    /// </summary>
    /// <param name="n"></param>
    public void NthWeapon(int n)
    {
        n = weapons.BoundToLength(n);

        var oldWeapon = CurrentWeapon;
        weapons.ForEach(w => w.SetActive(false));
        CurrentWeapon.SetActive(true);

        EventManager.Instance.onWeaponSwitch.Invoke(this, oldWeapon, CurrentWeapon);
    }
    #endregion

    #region Adding
    /// <summary>
    /// Adds a weapon if a weapon of the same name doesn't already exist in <see
    /// cref="weapons"/>.
    /// </summary>
    /// <param name="weapon">The weapon to add.</param>
    public void AddWeapon(WeaponAbstractionModule weapon)
    {
        if (!weapons.Exists(w => w.WeaponName == weapon.WeaponName))
        {
            weapon.transform.Localize(transform);
            weapons.Add(weapon);
        }
    }
    #endregion

    #endregion
}