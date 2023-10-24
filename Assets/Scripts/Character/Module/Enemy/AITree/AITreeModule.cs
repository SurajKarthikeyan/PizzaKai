using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class AITreeModule : Module
{
    #region Variables
    public AIBranchModule root;

    [SerializeField]
    [ReadOnly]
    [Tooltip("The current running branch.")]
    private AIBranchModule current = null;
    #endregion

    #region Validate
    private void OnValidate()
    {
        if (!root)
        {
            // Create root.
            GameObject rootGO = new("[Branch] Root", typeof(AIBranchModule));
            rootGO.RequireComponent(out root);
            root.id = "Root";
        }
    }
    #endregion

    #region Methods
    public void AIInitialize(EnemyControlModule enemy)
    {
        root.InitializeAI(enemy);
    }

    public void AIUpdate(EnemyControlModule enemy, float deltaTime)
    {
        if (!current)
            current = root;

        current.tree = this;
        current = current.SelectAI(enemy);
    }

    /// <summary>
    /// Performs a BFS over all the branches of the tree.
    /// </summary>
    /// <returns>Branches</returns>
    public IEnumerable<AIBranchModule> AllBranches()
    {
        Queue<AIBranchModule> q = new();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            var curr = q.Dequeue();
            yield return curr;

            foreach (var child in q)
            {
                q.Enqueue(child);
            }
        }
    }
    #endregion
}