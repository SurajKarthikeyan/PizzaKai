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

        public bool PeekInto(EnemyControlModule enemy)
        {
            TargetToken target = targeting.GetTarget();

            return target != null && decision.CheckDecision(enemy, target);
        }

        public Branch UpdateAI(EnemyControlModule enemy)
        {
            TargetToken target = targeting.GetTarget();

            if (!action.UpdateAI(enemy, target))
            {
                // Action has ended. Go onto next.

                if (!branches.IsNullOrEmpty())
                {
                    // Select from branches.
                    var validBranches = branches
                        .Where(b => b.decision.CheckDecision(enemy, target));

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
            return this;
        }
    }
    #endregion

    #region Variables
    public Branch root;

    private Branch current = null;
    #endregion


    public void AIUpdate(EnemyControlModule enemy)
    {
        current = (current ??= root).UpdateAI(enemy);
    }
}