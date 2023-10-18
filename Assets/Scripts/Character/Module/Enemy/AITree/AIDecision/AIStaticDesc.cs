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

    protected override bool CheckDecisionInternal(EnemyControlModule enemy, TargetToken target)
    {
        throw new System.NotImplementedException();
    }
}