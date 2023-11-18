/// <summary>
/// Provides the player as a target for the AI.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AITargetPlayer : AITargeting
{
    public override TargetToken GetTarget(EnemyControlModule enemy)
    {
        return new TargetToken(GameManager.Instance.Player.transform);
    }
}