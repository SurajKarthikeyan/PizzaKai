using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponModule;

public class MolotovWeapon : WeaponModule
{
    #region Variables
    [Header("Alt Fire Settings")]
    [Tooltip("Time between alt fire shots")]
    [SerializeField]
    private float altWaitBetweenShots;

    #endregion

    #region Init
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region Methods

   
    #endregion
}
