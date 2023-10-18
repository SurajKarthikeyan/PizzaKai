using System;
using NaughtyAttributes;
using UnityEngine;

public abstract class AIDecision :MonoBehaviour
{
    #region Flags
    public enum FlagCombinationStyle
    {
        And, Or
    }

    [System.Flags]
    /// <inheritdoc cref="flags"/>
    public enum Flags
    {
        Never = 0,
        Timer = 1,
        DistanceToTarget = 2
    }
    #endregion

    #region Variables
    /// <summary>
    /// Priority of the AI, used for tie-breaks.
    /// </summary>
    public int priority;

    [Tooltip("Determines how multiple flags are treated.")]
    public FlagCombinationStyle flagCombo;

    /// <summary>
    /// Determines how to come up with a decision.
    /// <br/>
    /// Never: Returns false always.
    /// <br/>
    /// Timer: Based on a timer.
    /// <br/>
    /// DistanceFromTarget: If target satisfies the distance requirement.
    /// <br/>
    /// ArrivedAtDestination: If AI state is set to ArrivedAtDestination.
    /// </summary>
    public Flags flags;


    [AllowNesting]
    [Tooltip("The time to wait for this action to complete.")]
    [ShowIf(nameof(flags), Flags.Timer)]
    public Duration time = new(5);
    #endregion

    #region Decision Methods
    /// <summary>
    /// Initializes the decision.
    /// </summary>
    /// <param name="enemy">The enemy who this decision belongs to.</param>
    public void InitializeDecision(EnemyControlModule enemy)
    {
        //Debug.Log("");
    }

    /// <summary>
    /// Determines whether or not the decision should be selected or not.
    /// </summary>
    /// <param name="enemy">The enemy.</param>
    /// <param name="target">The target token.</param>
    /// <returns></returns>
    public bool CheckDecision(EnemyControlModule enemy, TargetToken target)
    {
        if (target == null || flags == Flags.Never)
            return false;

        return CheckDecisionInternal(enemy, target);
    }

    protected abstract bool CheckDecisionInternal(EnemyControlModule enemy,
        TargetToken target);
    #endregion

    #region ToString
    public override string ToString()
    {
        return "AIDecision " +
            $"[flagCombinationStyle: {flagCombo}, " +
            $"flags: {flags}]";
    } 
    #endregion
}