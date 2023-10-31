/// <summary>
/// Follows the target.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIActFollow : AIAction
{
    #region Action Implementation
    public override void StartAI(EnemyControlModule enemy, TargetToken target)
    {
        enemy.SetMoveTarget(target);
    }

    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        // Do nothing. Logic handled by the enemy control and agent.
    }

    public override void ExitAI(EnemyControlModule enemy)
    {
        enemy.ClearMoveTarget();
    }
    #endregion
}