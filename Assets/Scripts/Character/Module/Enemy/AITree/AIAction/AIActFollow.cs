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
    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        switch (enemy.pathAgent.State)
        {
            case PathfindingAgent.NavigationState.Idle:
            case PathfindingAgent.NavigationState.ArrivedAtDestination:
                // Enemy idle. Force it not to be idle anymore.
                enemy.SetMoveTarget(target);
                break;
        }
    }

    public override void ExitAI(EnemyControlModule enemy)
    {
        enemy.ClearMoveTarget();
    }
    #endregion
}