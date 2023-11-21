
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
    public enum HaltingFlags
    {
        NoHalting,
        Timer,
        DistanceFromTarget,
    }

    [System.Flags]
    public enum ActionFlags
    {
        Nothing = 0,
        Follow = 1,
        FireWeapon = 2
    }
    #endregion

    #region Variables
    [InfoBox("Determines when the action is considered to be done.")]
    public HaltingFlags haltingFlags;

    [InfoBox("Determines what actions to perform. Can select multiple.")]
    public ActionFlags actionFlags;

    [AllowNesting]
    [Tooltip("The time to wait for this action to complete.")]
    [ShowIf(nameof(haltingFlags), HaltingFlags.Timer)]
    public Duration time = new(5);

    [AllowNesting]
    [Tooltip("The preferred distance to the target.")]
    [ShowIf(nameof(haltingFlags), HaltingFlags.DistanceFromTarget)]
    public MinMax preferredDistance = new(5, 10);

    //[AllowNesting]
    //[Tooltip("The preferred range to start shooting.")]
    //[ShowIf(nameof(miscFlags), MiscFlags.ShootAtTarget)]
    //public MinMax shootingRange = new(10, 15);
    #endregion

    /// <summary>
    /// Called when the AI is updated with FixedUpdate.
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns>True if the action has completed, false otherwise.</returns>
    public bool UpdateAI(EnemyControlModule enemy, TargetToken target, float deltaTime)
    {
        if (actionFlags.HasFlag(ActionFlags.Follow))
        {
            if (enemy.pathAgent.State == PathfindingAgent.NavigationState.Idle)
            {

            }
        }

        if (target == null)
            return true;

        return CheckIfDone(enemy, target, deltaTime);
    }



    /// <summary>
    /// Called when it's time to exit control of the AI action.
    /// </summary>
    /// <param name="enemy"></param>
    public void ExitAI(EnemyControlModule enemy)
    {

    }

    #region Helpers
    private bool CheckIfDone(EnemyControlModule enemy, TargetToken target, float deltaTime)
    {
        PathfindingManager pm = PathfindingManager.Instance;

        switch (haltingFlags)
        {
            case HaltingFlags.Timer:
                return time.Increment(deltaTime, true);

            case HaltingFlags.DistanceFromTarget:
                int dist;

                // Check if the path agent has a path.
                if (enemy.pathAgent.CurrentPath != null)
                {
                    // If it does, use the path length to calculate the
                    // distance.
                    dist = enemy.pathAgent.CurrentPath.GetLength(
                        pm.WorldToCell(enemy.transform.position)
                    );
                }
                else
                {
                    // Otherwise, use the taxicab distance.
                    dist = pm.WorldToCell(enemy.transform.position)
                        .TaxicabDistance(target.GridTarget);
                }

                // Arrived at ideal distance.
                return preferredDistance.Evaluate(dist);

            case HaltingFlags.NoHalting:
            default:
                return false;
        }
    }
    #endregion
}