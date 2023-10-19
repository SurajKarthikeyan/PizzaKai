using UnityEngine;

public class AIActFollow : AIAction
{
    #region Action Implementation
    public override void InitializeAction(EnemyControlModule enemy)
    {
        // Do nothing.
    }

    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        if (enemy.pathAgent.State == PathfindingAgent.NavigationState.Idle)
        {
            // In idle state, which means it can accept an move input.
            enemy.SetMoveTarget(target);
        }
    }

    public override void ExitAI(EnemyControlModule enemy)
    {
        enemy.ClearMoveTarget();
    }
    #endregion
}