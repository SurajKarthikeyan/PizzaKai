using UnityEngine;

/// <summary>
/// Evaluates the decision based on the distance between this AI and its target.
/// If no target, decides false.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIDescDistance : AIDecision
{
    #region Variables
    [SerializeField]
    private MinMax distanceToTarget = new(0, 5);
    #endregion

    #region Decision Implementation
    public override void InitializeDecision(EnemyControlModule enemy)
    {
        // Do nothing.
    }

    protected override bool CheckDecisionInternal(EnemyControlModule enemy, TargetToken target)
    {
        int dist = -1;

        // Check if the path agent has a path.
        if (enemy.pathAgent.CurrentPath != null)
        {
            try
            {
                // If it does, use the path length to calculate the
                // distance.
                dist = enemy.pathAgent.CurrentPath.GetLength(
                    enemy.pathAgent.NextNode
                );
            }
            catch (System.Exception)
            {
                // Do nothing
            }
        }

        if (dist < 0)
        {
            // Otherwise, use the taxicab distance.
            PathfindingManager pm = PathfindingManager.Instance;
            dist = pm.WorldToCell(enemy.transform.position)
                .TaxicabDistance(target.GridPosition);
        }

        if (PathAgentManager.Instance.isVerbose)
        {
            print(dist + ", " + distanceToTarget);
        }

        return distanceToTarget.Evaluate(dist);
    }
    #endregion
}