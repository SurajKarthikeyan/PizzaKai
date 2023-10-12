/// <summary>
/// Makes a decision if the range to target is within a certain value.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class RangeToTargetDcs : AIDecision
{
    #region Variables
    public float minRange;

    public float maxRange;
    #endregion

    #region Methods
    public override bool CheckDecision(EnemyControlModule enemy)
    {
        float range = enemy.transform.Distance2D(enemy.Target);

        return minRange <= range && range <= maxRange;
    }
    #endregion
}