using NaughtyAttributes;

/// <summary>
/// Use to instantly kill the character this is attached to.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class DieModule : Module
{
    [Button]
    private void GoAndDie()
    {
        Master.TakeDamage(int.MaxValue);
    }
}