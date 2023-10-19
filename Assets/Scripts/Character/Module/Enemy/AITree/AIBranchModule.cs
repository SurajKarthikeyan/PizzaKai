using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AIBranchModule : Module
{
    #region Variables
    [Tooltip("Optional identifier for this branch.")]
    public string id;

    [Tooltip("The priority of the branch. Higher priority branches are " +
        "selected over lower priority ones.")]
    public int priority;

    [Tooltip("Determines what to target.")]
    public AITargeting targeting;

    [Tooltip("The decision for entering this element.")]
    public AIDecision decision;

    public AIAction action;

    public List<AIBranchModule> branches = new();

    [HideInInspector]
    public AITreeModule tree;
    #endregion

    #region Validate
    /// <summary>
    /// Sets the names of itself and all of its AI components.
    /// </summary>
    public void AnnotateGameObjects()
    {
        // // Just setting the names of the gameobjects.
        // gameObject.name = ToString();

        // if (targeting)
        // {
        //     targeting.gameObject.name = $"[Targeting] {id}";
        // }

        // if (decision)
        // {
        //     decision.gameObject.name = $"[Decision] {id}";
        // }

        // if (action)
        // {
        //     action.gameObject.name = $"[Action] {id}";
        // }

        // foreach (var branch in branches)
        // {
        //     branch.AnnotateGameObjects();
        // }
    }

    private void OnValidate()
    {
        gameObject.name = ToString();

        if (targeting)
        {
            targeting.gameObject.name = $"[Targeting] {id}";
        }

        if (decision)
        {
            decision.gameObject.name = $"[Decision] {id}";
        }

        if (action)
        {
            action.gameObject.name = $"[Action] {id}";
        }
    }
    #endregion

    #region Main Methods
    /// <summary>
    /// Updates the AI.
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public AIBranchModule UpdateAI(EnemyControlModule enemy)
    {
        TargetToken target = targeting.GetTarget();

        if (!decision.CheckDecision(enemy, target))
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
                        enemy, target
                    ));

                if (validBranches.Any())
                {
                    var selected = validBranches
                        .Aggregate(
                            (b1, b2) =>
                                b1.priority > b2.priority
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
        action.UpdateAI(enemy, target);
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
    #endregion

    public override string ToString()
    {
        return $"[Branch] {id}";
    }
}