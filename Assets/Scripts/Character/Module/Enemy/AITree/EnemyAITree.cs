using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAITree
{
    #region Classes
    [System.Serializable]
    public class TreeElement
    {
        [Tooltip("Determines what to target.")]
        public AITargeting targeting;

        [Tooltip("The decision for entering this element.")]
        public AIDecision decision;

        public AIAction action;

        public List<TreeElement> subtrees = new();

        public bool PeekInto(EnemyControlModule enemy)
        {
            TargetToken target = targeting.GetTarget();

            return target != null && decision.CheckDecision(enemy);
        }

        public void UpdateAI(EnemyControlModule enemy, out TreeElement next)
        {
            TargetToken target = targeting.GetTarget();

            if (!action.UpdateAI(enemy, target))
            {
                // TODO: Go onto next area.
            }

            next = this;
        }

        ///// <summary>
        ///// Updates all the actions within this element.
        ///// </summary>
        ///// <param name="enemy">The enemy controller.</param>
        ///// <param name="next">The next tree element. May be set to this if this
        ///// has any remaining actions left.</param>
        //public void UpdateAI(EnemyControlModule enemy, out TreeElement next)
        //{
        //    if (!action.UpdateAI(enemy))
        //    {
        //        next = GoToNext(enemy);
        //    }
        //    else
        //    {
        //        next = this;
        //    }
        //}

        //private TreeElement GoToNext(EnemyControlModule enemy)
        //{
        //    if (subtrees.IsNullOrEmpty())
        //    {
        //        return enemy.decisionTree.root;
        //    }
        //    else
        //    {
        //        foreach (var branch in subtrees)
        //        {
        //            if (branch.decision.CheckDecision(enemy))
        //                return branch;
        //        }

        //        return this;
        //    }
        //}
    }
    #endregion

    #region Variables
    public TreeElement root;

    private TreeElement current = null;
    #endregion


    public void AIUpdate(EnemyControlModule enemy)
    {
        if (current == null)
            current = root;

        current.UpdateAI(enemy, out current);
    }
}