using UnityEngine;

/// <summary>
/// Allows multiple decisions to be chained together, using AND (all decisions
/// must be true), or OR (at least one decision must be true.)
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIDecisionCombo : AIDecision
{
    #region Enums
    /// <summary>
    /// Allows multiple decisions to be chained together, using AND (all
    /// decisions must be true), or OR (at least one decision must be true.)
    /// </summary>
    public enum ComboStyle
    {
        AND, OR
    }
    #endregion

    #region Variables
    public ComboStyle comboStyle;
    #endregion

    protected override bool CheckDecisionInternal(EnemyControlModule enemy, TargetToken target)
    {
        throw new System.NotImplementedException();
    }
}