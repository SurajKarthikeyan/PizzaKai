using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class AITreeModule : Module
{
    #region Variables
    public AIBranch root;

    private AIBranch current = null;

    [SerializeField]
    [ReadOnly]
    protected float deltaTime;
    #endregion

    #region Validate
    private void OnValidate()
    {
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
    public IEnumerable<AIBranch> AllBranches()
    {
        Queue<AIBranch> q = new();
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