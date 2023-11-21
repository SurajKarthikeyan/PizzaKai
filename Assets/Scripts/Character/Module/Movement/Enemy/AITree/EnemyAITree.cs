using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAITree : Module
{
    #region Classes
    [System.Serializable]
    public class TreeElement
    {
        [Tooltip("The decision for entering this element.")]
        public AIDecision decision;

        public List<AIAction> actions = new();

        public List<TreeElement> subtrees = new();

        public TreeElement AIUpdate(EnemyControlModule enemy)
        {
            if (subtrees.IsNullOrEmpty())
            {
                return enemy.decisionTree.root;
            }
            else
            {
                foreach (var branch in subtrees)
                {
                    if (branch.decision.CheckDecision(enemy))
                        return branch;
                }

                return this;
            }
        }
    }
    #endregion

    #region Variables
    public TreeElement root;
    #endregion

    // private void OnValidate()
    // {
    //     foreach (var tree in FlattenedElements())
    //     {
    //         tree.
    //     }
    // }



    /// <summary>
    /// Iterates through all the subtrees.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TreeElement> FlattenedElements()
    {
        Queue<TreeElement> q = new(root.subtrees);

        while (q.Count > 0)
        {
            var branch = q.Dequeue();
            yield return branch;

            foreach (var subtree in branch.subtrees)
            {
                q.Enqueue(subtree);
            }
        }
    }
}