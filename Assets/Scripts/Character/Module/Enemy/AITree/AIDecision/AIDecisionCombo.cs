using NaughtyAttributes;
using System.Collections.Generic;
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
    [Tooltip("Allows multiple decisions to be chained together. " +
        "Options are:\n" +
        "AND: all decisions must be true.\n" +
        "OR: at least one decision must be true.")]
    public ComboStyle comboStyle;

    [Tooltip("Place Decisions as children to add them to the list.")]
    [ReadOnly]
    [SerializeField]
    private List<AIDecision> decisions;
    #endregion

    #region Decision Implementation
    public override void InitializeDecision(EnemyControlModule enemy)
    {
        // Do nothing
    }

    protected override bool CheckDecisionInternal(EnemyControlModule enemy, TargetToken target)
    {
        bool select = comboStyle == ComboStyle.AND;
        foreach (var decision in decisions)
        {
            switch (comboStyle)
            {
                case ComboStyle.AND:
                    select &= decision.CheckDecision(enemy, target);
                    break;
                case ComboStyle.OR:
                    select |= decision.CheckDecision(enemy, target);
                    break;

            }
        }
        return select;
    }
    #endregion
}