using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class AIDecision
{
    #region Flags
    public enum Flags
    {
        Nothing,
        Always,
        DistanceToTarget
    }
    #endregion

    #region Variables
    /// <summary>
    /// Priority of the AI, used for tie-breaks.
    /// </summary>
    public int priority;

    public Flags flags;

    [SerializeField]
    [AllowNesting]
    [ShowIf(nameof(flags), Flags.DistanceToTarget)]
    private MinMax distanceToTarget = new(0, 5);
    #endregion

    public bool CheckDecision(EnemyControlModule enemy, TargetToken target)
    {
        float distance = enemy.Master.transform.Distance2D(target.Target);
        return flags switch
        {
            Flags.Always => true,
            Flags.DistanceToTarget => distanceToTarget.Evaluate(distance),
            _ => false
        };
    }
}