
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
    public enum TargetFlags
    {
        Nothing,
        TargetPoint,
        TargetTransform,
        TargetPlayer,
    }

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
    public TargetFlags targetFlags;

    public MovementFlags moveFlags;

    public MiscFlags miscFlags;

    [AllowNesting]
    [ShowIf(nameof(targetFlags), TargetFlags.TargetPoint)]
    public Vector3 targetPoint;

    [AllowNesting]
    [ShowIf(nameof(targetFlags), TargetFlags.TargetTransform)]
    public Transform targetTransform;

    [AllowNesting]
    [Tooltip("The preferred range to move.")]
    [ShowIf(nameof(moveFlags), MovementFlags.KeepDistance)]
    public MinMax preferredMoveRange = new(5, 10);

    //[AllowNesting]
    //[Tooltip("The preferred range to start shooting.")]
    //[ShowIf(nameof(miscFlags), MiscFlags.ShootAtTarget)]
    //public MinMax shootingRange = new(10, 15);
    #endregion

    #region Private Variables
    private TargetToken targetToken;
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
            switch (moveFlags)
            {
                case MovementFlags.SimpleFollow:
                    if (PathfindingManager.Instance.InSameCell(
                            target.Target,
                            enemy.transform.position
                        ))
                    {
                        return true;
                    }
                    else if (PathfindingManager.Instance.InSameCell(
                                target.Target,
                                enemy.Target
                            ))
                    {
                        enemy.SetTarget(target);
                    }

                    break;
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