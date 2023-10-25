using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Either decides true or false, based on the value of 
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIDescStatic : AIDecision
{
    #region Variables
    [InfoBox("Determines whether or not this decision will " +
        "evaluate to true or false. If true, the decision is " +
        "always taken. Vice versa for false.")]
    public bool decision;
    #endregion

    #region Decision Implementation
    public override void InitializeDecision(EnemyControlModule enemy)
    {
        // Do nothing.
    }

    protected override bool CheckDecisionInternal(EnemyControlModule enemy,
        TargetToken target) => decision;
    #endregion
}