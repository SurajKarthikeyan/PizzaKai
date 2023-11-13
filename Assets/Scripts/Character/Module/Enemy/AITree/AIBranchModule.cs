using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Defines a branch on an AI decision tree. The actual control of this branch
/// is controlled by the subcomponents, <see cref="AITargeting"/>,
/// <see cref="AIDecision"/>, and <see cref="AIAction"/>.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public class AIBranchModule : Module
{
    #region Enums
    public enum State
    {
        Stopped,
        Starting,
        Updating,
        Exiting
    }
    #endregion

    #region Variables
    [Tooltip("Optional identifier for this branch.")]
    public string id;

    [ReadOnly]
    [SerializeField]
    [Tooltip("Current state of the branch.")]
    private State state;

    [Tooltip("The priority of the branch. Higher priority branches are " +
        "selected over lower priority ones.")]
    public int priority;

    [Tooltip("Determines what to target.")]
    public AITargeting targeting;

    [Tooltip("The decision for entering this element.")]
    public AIDecision decision;

    public AIAction[] actions = new AIAction[0];

    public List<AIBranchModule> branches = new();

    [HideInInspector]
    public AITreeModule tree;
    #endregion

    #region Properties
    /// <summary>
    /// True if this branch has <paramref name="tree"/> and if it is the current
    /// running branch on <paramref name="tree"/>.
    /// </summary>
    public bool IsSelected => tree && tree.Current == this;

    /// <summary>
    /// The current state of the branch's execution.
    /// </summary>
    public State BranchState => state;
    #endregion

    #region Main Methods
    /// <summary>
    /// Initializes the AI branch. Called once from the AI tree.
    /// </summary>
    /// <param name="enemy">The enemy the AI branch is associated with.</param>
    public void InitializeAI(EnemyControlModule enemy)
    {
        targeting.InitializeTargeting(enemy);
        decision.InitializeDecision(enemy);
        foreach (var action in actions)
        {
            action.InitializeAction(enemy);
        }

        foreach (var branch in branches)
        {
            branch.InitializeAI(enemy);
        }
    }

    /// <summary>
    /// Selects the AI to run next. This will either be one of this branch's sub-branches
    /// or the root of the tree this belongs to.
    /// </summary>
    /// <returns>The selected branch.</returns>
    /// <inheritdoc cref="InitializeAI(EnemyControlModule)"/>
    public AIBranchModule SelectAI(EnemyControlModule enemy)
    {
        TargetToken target = targeting.GetTarget();

        if (decision.CheckDecision(enemy, target))
        {
            // Action has not yet ended. Update action and return.
            UpdateAI(enemy, target);
            return this;
        }

        // Action has ended. Go onto next.
        ExitAI(enemy);
        var actNames = string.Join(
            ", ",
            actions.Select(a => a.ToString())
        );
        Debug.Log(
            $"Actions [{actNames}] has ended."
        );

        // Default case if nothing is selected.
        AIBranchModule selected = enemy.decisionTree.root;

        if (!branches.IsNullOrEmpty())
        {
            // Select from branches.
            var validBranches = branches
                .Where(b => b.decision.CheckDecision(
                    enemy, b.targeting.GetTarget()
                ));

            if (validBranches.Any())
            {
                // Selecting new AI.
                selected = validBranches
                    .Aggregate(
                        (b1, b2) =>
                            b1.priority > b2.priority
                            ? b1 : b2
                    );

                Debug.Log(
                    $"Switching to subbranch {selected}"
                );
            }
        }

        if (enemy.decisionTree.root == selected)
        {
            // No more valid branches. Return root.
            Debug.Log(
                "No more branches. Return to root " +
                $"[{selected}]"
            );
        }

        selected.StartAI(enemy, target);

        return selected;
    }

    /// <summary>
    /// Starts all AI controls belonging to the branch.
    /// </summary>
    private void StartAI(EnemyControlModule enemy, TargetToken target)
    {
#if UNITY_EDITOR
        Debug.Assert(
            BranchState == State.Stopped,
            $"{this} StartAI called when state not Stopped!"
        );
        print($"{this} StartAI.");
#endif

        state = State.Starting;
        foreach (var action in actions)
        {
            action.selected = true;
            action.StartAI(enemy, target);
        }
    }

    /// <summary>
    /// Updates all AI controls belonging to the branch.
    /// </summary>
    private void UpdateAI(EnemyControlModule enemy, TargetToken target)
    {
        if (state == State.Updating || state == State.Starting)
        {
#if UNITY_EDITOR
            if (state == State.Starting) print($"{this} UpdateAI.");
#endif

            state = State.Updating;
            foreach (var action in actions)
            {
                action.UpdateAI(enemy, target);
            }
        }
        else if (state == State.Stopped)
        {
#if UNITY_EDITOR
            print($"{this} UpdateAI deferred. Running StartAI instead.");
#endif
            // Start has not been called yet. Do that now.
            StartAI(enemy, target);
        }
    }

    /// <summary>
    /// Exits all AI controls belonging to the branch.
    /// </summary>
    private void ExitAI(EnemyControlModule enemy)
    {
        Debug.Assert(
            BranchState == State.Updating,
            $"{this} ExitAI called when state not Updating!"
        );

        print($"{this} ExitAI.");
        state = State.Exiting;
        foreach (var action in actions)
        {
            action.selected = false;
            action.ExitAI(enemy);
        }
    }
    #endregion

    #region Object Overrides
    public override string ToString()
    {
        return $"[Branch] {id}";
    }
    #endregion
}