using System.Collections;
using UnityEngine;

public class RangeToTargetDecision : AIDecision
{
    #region Variables
    public float minRange;

    public float maxRange;
    #endregion

    #region Methods
    public override bool CheckDecision(EnemyControlModule enemy)
    {
        float range = enemy.transform.Distance2D(enemy.Target);

        return minRange <= range && range <= minRange;
    }
    #endregion
}