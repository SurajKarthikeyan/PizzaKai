using UnityEngine;

/// <summary>
/// Provides a transform which will be the target for the AI.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AITargetTransform : AITargeting
{
    public Transform targetTransform;

    #region AITargeting Implementation
    public override void InitializeTargeting(EnemyControlModule enemy)
    {
        
    }

    public override TargetToken GetTarget()
    {
        return new TargetToken(targetTransform);
    }
    #endregion
}