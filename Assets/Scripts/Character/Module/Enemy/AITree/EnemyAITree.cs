using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyAITree
{
    #region Classes
    [System.Serializable]
    public class Branch
    {
        [Tooltip("Determines what to target.")]
        public AITargeting targeting;

        [Tooltip("The decision for entering this element.")]
        public AIDecision decision;

        public AIAction action;

        public List<Branch> branches = new();

        // public bool PeekInto(EnemyControlModule enemy)
        // {
        //     TargetToken target = targeting.GetTarget();

        //     return target != null && decision.CheckDecision(enemy, target, del);
        // }

        public Branch UpdateAI(EnemyControlModule enemy, float deltaTime)
        {
            TargetToken target = targeting.GetTarget();

            if (!decision.CheckDecision(enemy, target, deltaTime))
            {
                // Action has ended. Go onto next.
                action.ExitAI(enemy);

                if (!branches.IsNullOrEmpty())
                {
                    // Select from branches.
                    var validBranches = branches
                        .Where(b => b.decision.CheckDecision(enemy, target, deltaTime));

                    if (validBranches.Any())
                    {
                        return validBranches
                            .Aggregate(
                                (b1, b2) =>
                                    b1.decision.priority > b2.decision.priority
                                    ? b1 : b2
                            );
                    }
                }
                // No more valid branches. Return root.
                return enemy.decisionTree.root;
            }

            // Action still occurring.
            action.UpdateAI(enemy, target, deltaTime);
            return this;
        }
    }
    #endregion

    #region Variables
    public Branch root;

    private Branch current = null;
    #endregion


    public void AIUpdate(EnemyControlModule enemy, float deltaTime)
    {
        current = (current ??= root).UpdateAI(enemy, deltaTime);
    }
}