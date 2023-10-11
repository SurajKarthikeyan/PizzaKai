using UnityEngine;

public class AIMoveToPoint : AIAction
{
    public Vector2 target;

    public override void ExitAI(EnemyControlModule enemy)
    {
        // Do nothing.
    }

    public override void StartAI(EnemyControlModule enemy)
    {
        enemy.pathAgent.SetTarget(target);
    }

    public override bool UpdateAI(EnemyControlModule enemy)
    {
        return PathfindingManager.Instance.InSameCell(
            enemy.transform.position.ToVector2(),
            target
        );
    }
}