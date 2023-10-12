
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
    [ShowIf(nameof(moveFlags), MovementFlags.KeepDistance)]
    public MinMax moveRange = new(5, 10);

    [AllowNesting]
    [ShowIf(nameof(miscFlags), MiscFlags.ShootAtTarget)]
    public MinMax shootingRange = new(10, 15);
    #endregion

    #region Private Variables
    private TargetToken targetToken;
    #endregion

    /// <summary>
    /// Called when the AI is started.
    /// </summary>
    /// <param name="enemy"></param>
    public void StartAI(EnemyControlModule enemy)
    {
        targetToken = targetFlags switch
        {
            TargetFlags.TargetPlayer => new(GameManager.Instance.Player.transform),
            TargetFlags.TargetTransform => new(targetTransform),
            TargetFlags.TargetPoint => new(targetPoint),
            _ => null
        };

        if (targetToken != null)
        {
            switch (moveFlags)
            {
                case MovementFlags.SimpleFollow:
                    enemy.SetTarget(targetToken);
                    break;
            }
        }
    }

    /// <summary>
    /// Called when the AI is updated with FixedUpdate.
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns>True if the action has completed, false otherwise.</returns>
    public bool UpdateAI(EnemyControlModule enemy)
    {
        

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