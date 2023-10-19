
using System;
using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// Responsible for defining the actions that AI will do.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public abstract class AIAction : Module
{
    #region Action Methods
    /// <summary>
    /// Initializes the action.
    /// </summary>
    /// <inheritdoc cref="UpdateAI(EnemyControlModule, TargetToken)"/>
    public abstract void InitializeAction(EnemyControlModule enemy);

    /// <summary>
    /// Called when the AI is updated with FixedUpdate.
    /// </summary>
    /// <param name="enemy">The enemy controlled by the AI.</param>
    /// <param name="target">The target of the enemy, generated by <see
    /// cref="AITargeting.GetTarget"/>.</param>
    /// <returns>True if the action has completed, false otherwise.</returns>
    public void UpdateAI(EnemyControlModule enemy, TargetToken target)
    {
        PerformAction(enemy, target);
    }

    /// <summary>
    /// Called when it's time to exit control of the AI action.
    /// </summary>
    /// <inheritdoc cref="UpdateAI(EnemyControlModule, TargetToken)"/>
    public abstract void ExitAI(EnemyControlModule enemy);
    #endregion

    #region Helpers
    /// <summary>
    /// Performs the selected functions.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="target"></param>
    protected abstract void PerformAction(EnemyControlModule enemy, TargetToken target);
    #endregion
}