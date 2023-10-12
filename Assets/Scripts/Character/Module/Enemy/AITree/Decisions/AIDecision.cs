using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class AIDecision
{
    #region Flags
    public enum DistanceFlags
    {
        Nothing,
        DistanceGreater
    }

    [Tooltip("Distance to target.")]
    public DistanceFlags distanceFlags;
    #endregion

    /// <summary>
    /// Priority of the AI, used for tie-breaks.
    /// </summary>
    public int priority;

    public virtual bool CheckDecision(EnemyControlModule enemy) => false;
}