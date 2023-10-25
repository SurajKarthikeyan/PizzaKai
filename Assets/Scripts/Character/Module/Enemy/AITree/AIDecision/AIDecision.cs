using System;
using System.Text.RegularExpressions;
using NaughtyAttributes;
using UnityEngine;

public abstract class AIDecision :MonoBehaviour
{
    #region Decision Methods
    /// <summary>
    /// Initializes the decision. This is called when <see cref="AITreeModule"/>
    /// is first initialized.
    /// </summary>
    /// <param name="enemy">The enemy who this decision belongs to.</param>
    public abstract void InitializeDecision(EnemyControlModule enemy);

    /// <summary>
    /// Determines whether or not the decision should be selected or not.
    /// </summary>
    /// <param name="enemy">The enemy.</param>
    /// <param name="target">The target token.</param>
    /// <returns>True if the decision should be selected (or should
    /// continue being selected), false otherwise.</returns>
    public bool CheckDecision(EnemyControlModule enemy, TargetToken target)
    {
        if (target == null)
            return false;

        return CheckDecisionInternal(enemy, target);
    }

    protected abstract bool CheckDecisionInternal(EnemyControlModule enemy,
        TargetToken target);
    #endregion

    #region ToString
    private static readonly Regex actTypeRX = new(@"AIDesc(\w+)");

    public override string ToString()
    {
        string actType = GetType().Name;
        actType = actTypeRX.Match(actType).Groups[1].Value;
        return $"[Decision:{actType}]";
    }
    #endregion
}