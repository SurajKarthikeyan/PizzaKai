using System;
using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class AIDecision
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

    [SerializeField]
    [AllowNesting]
    [ShowIf(nameof(flags), Flags.DistanceToTarget)]
    private MinMax distanceToTarget = new(0, 5);

    [AllowNesting]
    [Tooltip("The time to wait for this action to complete.")]
    [ShowIf(nameof(flags), Flags.Timer)]
    public Duration time = new(5);
    #endregion

    public bool CheckDecision(EnemyControlModule enemy, TargetToken target, float deltaTime)
    {
        if (target == null || flags == Flags.Never)
            return false;

        bool decision = flagCombo == FlagCombinationStyle.And;

        void ModifyDecision(bool value)
        {
            switch (flagCombo)
            {
                case FlagCombinationStyle.And:
                    decision &= value;
                    break;

                case FlagCombinationStyle.Or:
                default:
                    decision |= value;
                    break;
            }
        }

        if (flags.HasFlag(Flags.Timer))
        {
            ModifyDecision(!time.Increment(deltaTime, true));
        }
        if (flags.HasFlag(Flags.DistanceToTarget))
        {
            int dist = -1;

            // Check if the path agent has a path.
            if (enemy.pathAgent.CurrentPath != null)
            {
                try
                {
                    // If it does, use the path length to calculate the
                    // distance.
                    dist = enemy.pathAgent.CurrentPath.GetLength(
                        enemy.pathAgent.NextNode
                    );
                }
                catch (System.Exception)
                {
                    // Do nothing
                }
            }

            if (dist < 0)
            {
                // Otherwise, use the taxicab distance.
                PathfindingManager pm = PathfindingManager.Instance;
                dist = pm.WorldToCell(enemy.transform.position)
                    .TaxicabDistance(target.GridTarget);
            }

            ModifyDecision(distanceToTarget.Evaluate(dist));
        }

        return decision;
    }

    public override string ToString()
    {
        return "AIDecision " +
            $"[flagCombinationStyle: {flagCombo}, " +
            $"flags: {flags}]";
    }

    public void InitializeDecision(EnemyControlModule enemy)
    {
        Debug.Log("");
    }
}