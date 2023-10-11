using NaughtyAttributes;

[System.Serializable]
public class AIDecision
{


    /// <summary>
    /// Priority of the AI, used for tie-breaks.
    /// </summary>
    public int priority;

    public virtual bool CheckDecision(EnemyControlModule enemy) => false;
}