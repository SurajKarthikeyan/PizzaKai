
using NaughtyAttributes;
using UnityEngine;


/// <summary>
/// Interface for AI actions. These are responsible for doing the AI actions. I
/// tried using inheritance to make this "correctly," but I couldn't find a way
/// to do so while still making everything editable by the user.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public class AIAction
{
    #region Flags
    [System.Flags]
    public enum ActionFlags
    {
        Nothing = 0,
        Follow = 1,
        FireWeapon = 2
    }
    #endregion

    #region Variables
    #region Action
    [InfoBox("Determines what actions to perform. Can select multiple.")]
    public ActionFlags actionFlags;
    #endregion
    #endregion

    /// <summary>
    /// Called when the AI is updated with FixedUpdate.
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns>True if the action has completed, false otherwise.</returns>
    public void UpdateAI(EnemyControlModule enemy, TargetToken target, float deltaTime)
    {
        PerformActions(enemy, target, deltaTime);
    }

    /// <summary>
    /// Called when it's time to exit control of the AI action.
    /// </summary>
    /// <param name="enemy"></param>
    public void ExitAI(EnemyControlModule enemy)
    {
        if (actionFlags.HasFlag(ActionFlags.Follow))
        {
            enemy.ClearMoveTarget();
        }
    }

    #region Helpers
    private void PerformActions(EnemyControlModule enemy, TargetToken target, float deltaTime)
    {
        if (actionFlags.HasFlag(ActionFlags.Follow))
        {
            if (enemy.pathAgent.State == PathfindingAgent.NavigationState.Idle)
            {
                // In idle state, which means it can accept an move input.
                enemy.SetMoveTarget(target);
            }
        }
    }
    #endregion
}