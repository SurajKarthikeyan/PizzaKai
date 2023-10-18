using NaughtyAttributes;
using UnityEngine;
using static AIAction;

[System.Serializable]
public class AITargeting
{
    #region Flags
    public enum Flags
    {
        Nothing,
        TargetPoint,
        TargetTransform,
        TargetPlayer,
    }
    #endregion

    #region variables
    public Flags flags;

    [AllowNesting]
    [ShowIf(nameof(flags), Flags.TargetPoint)]
    public Vector3 targetPoint;

    [AllowNesting]
    [ShowIf(nameof(flags), Flags.TargetTransform)]
    public Transform targetTransform;
    #endregion

    #region Targeting Methods
    /// <summary>
    /// Initializes the targeting.
    /// </summary>
    /// <param name="enemy">The enemy who this targeting belongs to.</param>
    public void InitializeTargeting(EnemyControlModule enemy)
    {

    }

    public TargetToken GetTarget()
    {
        return flags switch
        {
            Flags.TargetPlayer => new(GameManager.Instance.Player.transform),
            Flags.TargetTransform => new(targetTransform),
            Flags.TargetPoint => new(targetPoint),
            _ => null
        };
    }
    #endregion

    #region ToString
    public override string ToString()
    {
        return $"AITargeting [flags: {flags}]";
    } 
    #endregion
}