
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
    public enum MovementFlags
    {
        Nothing,
        SimpleFollow,
        KeepDistance
    }

    public enum MiscFlags
    {
        Nothing,
        ShootAtTarget,
    }
    #endregion

    #region Variables
    public MovementFlags moveFlags;

    public MiscFlags miscFlags;

    [AllowNesting]
    [Tooltip("The preferred range to move.")]
    [ShowIf(nameof(moveFlags), MovementFlags.KeepDistance)]
    public MinMax preferredMoveRange = new(5, 10);

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
    public bool UpdateAI(EnemyControlModule enemy, TargetToken target)
    {
        if (target != null)
        {
            PathfindingManager pm = PathfindingManager.Instance;

            switch (moveFlags)
            {
                case MovementFlags.SimpleFollow:
                    if (pm.InSameCell(
                            target.Target,
                            enemy.transform.position
                        ))
                    {
                        // Arrived at target.
                        return true;
                    }

                    return false;
                case MovementFlags.KeepDistance:
                    if (enemy.pathAgent.HasPath)
                    {
                        int dist = enemy.pathAgent.CurrentPath.GetLength(
                            pm.WorldToCell(enemy.transform.position)
                        );
    
                        if (preferredMoveRange.Evaluate(dist))
                        {
                            // Arrived at ideal distance.
                            return true;
                        }
                        
                        dist = pm.WorldToCell(enemy.transform.position)
                            .TaxicabDistance(target.GridTarget);
                        
                        if (preferredMoveRange.Evaluate(dist))
                        {
                            // Arrived at ideal distance.
                            return true;
                        }
                    }

                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Called when it's time to exit control of the AI action.
    /// </summary>
    /// <param name="enemy"></param>
    public void ExitAI(EnemyControlModule enemy)
    {

    }
}