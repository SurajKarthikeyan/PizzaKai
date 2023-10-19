using UnityEngine;

/// <summary>
/// Either decides true or false, based on the value of 
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIStaticDesc : AIDecision
{
    #region Variables
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