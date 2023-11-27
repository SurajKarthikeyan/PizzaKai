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
    public override TargetToken GetTarget(EnemyControlModule enemy)
    {
        return new TargetToken(targetTransform);
    }
    #endregion
}