
/// <summary>
/// Interface for AI actions. These are responsible for doing the AI actions.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public abstract class AIAction
{
    /// <summary>
    /// Called when the AI is started.
    /// </summary>
    /// <param name="enemy"></param>
    public abstract void StartAI(EnemyControlModule enemy);

    /// <summary>
    /// Called when the AI is updated with FixedUpdate.
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns>True if the action has completed, false otherwise.</returns>
    public abstract bool UpdateAI(EnemyControlModule enemy);

    /// <summary>
    /// Called when it's time to exit control of the AI action.
    /// </summary>
    /// <param name="enemy"></param>
    public abstract void ExitAI(EnemyControlModule enemy);
}