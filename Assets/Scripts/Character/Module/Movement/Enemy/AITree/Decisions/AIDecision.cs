[System.Serializable]
public abstract class AIDecision
{
    /// <summary>
    /// Priority of the AI, used for tie-breaks.
    /// </summary>
    public int priority;

    public abstract bool CheckDecision(EnemyControlModule enemy);
}