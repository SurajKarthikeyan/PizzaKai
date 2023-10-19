using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class AITreeModule : Module
{
    #region Variables
    public AIBranchModule root;

    private AIBranchModule current = null;

    [SerializeField]
    [ReadOnly]
    protected float deltaTime;
    #endregion

    #region Validate
    private void OnValidate()
    {
        if (!root)
        {
            var rootGO = new GameObject(
                "",
                typeof(AIBranchModule)
            );
            rootGO.transform.Localize(transform);
            rootGO.RequireComponent(out root);
        }
        
        root.AnnotateGameObjects();
    }
    #endregion

    #region Methods
    public void AIInitialize(EnemyControlModule enemy)
    {
        root.InitializeAI(enemy);
    }

    public void AIUpdate(EnemyControlModule enemy, float deltaTime)
    {
        this.deltaTime = deltaTime;
        current ??= root;
        current.tree = this;
        current = current.UpdateAI(enemy);
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