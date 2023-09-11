using NaughtyAttributes;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnModule : Module
{
    #region Variables
    [Tooltip("The last touched respawn point.")]
    [SerializeField]
    [ReadOnly]
    private Checkpoint lastRespawnPoint;
    #endregion

    #region MonoBehavior
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Checkpoint") &&
            other.HasComponent(out Checkpoint checkpoint))
        {
            lastRespawnPoint = checkpoint;
            checkpoint.AssociatedRespawner = this;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Clears the last respawn point and returns its value.
    /// </summary>
    /// <returns></returns>
    public Checkpoint ClearCheckpoint()
    {
        var temp = lastRespawnPoint;
        lastRespawnPoint = null;
        return temp;
    }
    #endregion
}