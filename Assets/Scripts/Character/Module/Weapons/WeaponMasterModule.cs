using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Controls weapon switching, aiming, etc.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class WeaponMasterModule : Module
{
    #region Variables
    [Tooltip("The weapons available to the weapon master.")]
    public List<WeaponModule> weapons;

    [Tooltip("Which weapon is the character holding?")]
    [ReadOnly]
    public int weaponIndex;
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
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Aims the weapons at <paramref name="target"/>.
    /// </summary>
    /// <param name="target">Where to aim the weapons.</param>
    public void AimAt(Vector2 target)
    {
        // Do some basic trig to get the weapons pointed at the target.
        Vector2 disp = target - transform.position.ToVector2();
        Vector3 eulerAngles = new(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            Mathf.Atan2(disp.y, disp.x)
        );
        transform.localEulerAngles = eulerAngles;
    }

    /// <summary>
    /// Switches to the next weapon.
    /// </summary>
    public void NextWeapon()
    {
        weaponIndex++;
        weaponIndex %= weapons.Count;

        EnableCurrentWeapon();
    }
    #endregion

    #region Helper Methods
    private void EnableCurrentWeapon()
    {
        weapons.ForEach(weap => weap.gameObject.SetActive(false));

        CurrentWeapon.gameObject.SetActive(true);
    }
    #endregion
    #endregion
}