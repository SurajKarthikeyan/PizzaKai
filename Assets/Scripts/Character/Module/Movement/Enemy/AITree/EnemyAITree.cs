using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAITree
{
    #region Classes
    [System.Serializable]
    public class TreeElement
    {
        [Tooltip("The decision for entering this element.")]
        public AIDecision decision;

        public List<AIAction> actions = new();

        public List<TreeElement> subtrees = new();

        /// <summary>
        /// Updates all the actions within this element.
        /// </summary>
        /// <param name="enemy">The enemy controller.</param>
        /// <param name="next">The next tree element. May be set to this if this
        /// has any remaining actions left.</param>
        public void UpdateAI(EnemyControlModule enemy, out TreeElement next)
        {
            bool actionsAllDone = true;

            foreach (var act in actions)
            {
                if (!act.UpdateAI(enemy))
                {
                    actionsAllDone = false;
                }
            }

            if (actionsAllDone)
            {
                next = GoToNext(enemy);
            }
            else
            {
                next = this;
            }
        }

        private TreeElement GoToNext(EnemyControlModule enemy)
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
}