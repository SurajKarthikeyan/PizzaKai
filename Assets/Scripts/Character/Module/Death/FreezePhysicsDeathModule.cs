using System;
using UnityEngine;

/// <summary>
/// Freezes the x and/or y velocity on death.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class FreezePhysicsDeathModule : DeathModule
{
    #region Variables
    [Tooltip("What axis to freeze? Can select multiple.")]
    public RigidbodyConstraints2D axis;

    private RigidbodyConstraints2D ogAxis;
    #endregion

    #region Implementation
    protected override void OnDeath()
    {
        if (Master.r2d)
        {
            ogAxis = Master.r2d.constraints;
            Master.r2d.constraints = axis;
        }
    }

    protected override void OnRevive()
    {
        if (Master.r2d)
        {
            Master.r2d.constraints = ogAxis;
        }
    }
    #endregion
}