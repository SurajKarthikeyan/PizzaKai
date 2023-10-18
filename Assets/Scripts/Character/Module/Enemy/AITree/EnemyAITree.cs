using System;
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
        [Tooltip("Optional identifier for this branch.")]
        public string id;

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

            action.actionFlags |= AIAction.ActionFlags.FireWeapon;

            if (!decision.CheckDecision(enemy, target, deltaTime))
            {
                // Action has ended. Go onto next.
                action.ExitAI(enemy);
                Debug.Log(
                    $"Action [{action}] has ended."
                );

                if (!branches.IsNullOrEmpty())
                {
                    // Select from branches.
                    var validBranches = branches
                        .Where(b => b.decision.CheckDecision(
                            enemy, target, deltaTime
                        ));

                    if (validBranches.Any())
                    {
                        var selected = validBranches
                            .Aggregate(
                                (b1, b2) =>
                                    b1.decision.priority > b2.decision.priority
                                    ? b1 : b2
                            );

                        Debug.Log(
                            $"Switching to subbranch {selected}"
                        );
                        return selected;
                    }
                }
                // No more valid branches. Return root.
                Debug.Log(
                    "No more branches. Return to root " +
                    $"[{enemy.decisionTree.root}]"
                );
                return enemy.decisionTree.root;
            }

            // Action still occurring.
            action.UpdateAI(enemy, target, deltaTime);
            return this;
        }

        public void InitializeAI(EnemyControlModule enemy)
        {
            targeting.InitializeTargeting(enemy);
            decision.InitializeDecision(enemy);
            action.InitializeAction(enemy);

            foreach (var branch in branches)
            {
                branch.InitializeAI(enemy);
            }
        }

        public override string ToString()
        {
            return $"AITreeBranch {id}";
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

    public void AIInitialize(EnemyControlModule enemy)
    {
        root.InitializeAI(enemy);
    }
}