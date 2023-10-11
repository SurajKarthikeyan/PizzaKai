using UnityEngine;

public class AIFollowTransform : AIAction
{
    public Transform target;

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
            target.position.ToVector2()
        );
    }
}