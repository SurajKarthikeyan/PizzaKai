using UnityEngine;

/// <summary>
/// Flees from the target
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIActFlee : AIAction
{
    #region Variables
    [Tooltip("How far should this AI flee?")]
    public float preferredFleeDistance = 10;

    [Tooltip("How often to recalculate the flee point?")]
    public Duration recalculationDelay = new(2.5f);
    #endregion

    #region Action Implementation
    public override void StartAI(EnemyControlModule enemy, TargetToken target)
    {
        // Start recalculation.
        recalculationDelay.CreateCallback(this, SetFlee, true);
    }

    protected override void PerformAction(EnemyControlModule enemy, TargetToken target)
    {
        switch (enemy.pathAgent.State)
        {
            case PathfindingAgent.NavigationState.Idle:
            case PathfindingAgent.NavigationState.ArrivedAtDestination:
                // Restart navigation.
                ExitAI(enemy);
                StartAI(enemy, target);
                break;
        }
    }

    public override void ExitAI(EnemyControlModule enemy)
    {
        recalculationDelay.ClearCallback();
        enemy.ClearMoveTarget();
    }
    #endregion

    #region Helpers
    private void SetFlee()
    {

    }
    #endregion
}