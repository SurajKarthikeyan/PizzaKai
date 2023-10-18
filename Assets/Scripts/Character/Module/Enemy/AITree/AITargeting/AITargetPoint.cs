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
    public override void InitializeTargeting(EnemyControlModule enemy)
    {
        
    }

    public override TargetToken GetTarget()
    {
        return new TargetToken(targetPoint);
    }
    #endregion
}