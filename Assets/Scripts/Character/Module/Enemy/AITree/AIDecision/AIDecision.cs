using System;
using NaughtyAttributes;
using UnityEngine;

public abstract class AIDecision :MonoBehaviour
{
    #region Decision Methods
    /// <summary>
    /// Initializes the decision.
    /// </summary>
    /// <param name="enemy">The enemy who this decision belongs to.</param>
    public abstract void InitializeDecision(EnemyControlModule enemy);

    /// <summary>
    /// Determines whether or not the decision should be selected or not.
    /// </summary>
    /// <param name="enemy">The enemy.</param>
    /// <param name="target">The target token.</param>
    /// <returns></returns>
    public bool CheckDecision(EnemyControlModule enemy, TargetToken target)
    {
        if (target == null)
            return false;

        return CheckDecisionInternal(enemy, target);
    }

    protected abstract bool CheckDecisionInternal(EnemyControlModule enemy,
        TargetToken target);
    #endregion
}