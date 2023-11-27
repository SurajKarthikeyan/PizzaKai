using UnityEngine;

/// <summary>
/// Returns a static point which will be the target for the AI.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AITargetPoint : AITargeting
{
    public Vector3 targetPoint;

    #region AITargeting Implementation
    public override TargetToken GetTarget(EnemyControlModule enemy)
    {
        return new TargetToken(targetPoint);
    }
    #endregion
}