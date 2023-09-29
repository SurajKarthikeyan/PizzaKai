using NaughtyAttributes;
using UnityEngine;

public class RespawnModule : DeathModule
{
    #region Variables
    [Tooltip("The last touched respawn point.")]
    [SerializeField]
    [ReadOnly]
    public CheckpointScript lastRespawnPoint;
    #endregion

    protected override void OnDeath()
    {
        if (lastRespawnPoint)
        {
            transform.position = lastRespawnPoint.transform.position;
        }
    }
}