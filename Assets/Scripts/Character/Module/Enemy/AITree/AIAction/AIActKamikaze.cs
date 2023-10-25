using UnityEngine;

/// <summary>
/// Calls the death function once this is within range of its target.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIActKamikaze : AIAction
{
    [SerializeField]
    [Tooltip("How far away from the target do we call the death function?")]
    private float distance = 0.5f;

    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        if (enemy.transform.Distance(target.Position) <= distance)
            enemy.Master.Die();
    }
}