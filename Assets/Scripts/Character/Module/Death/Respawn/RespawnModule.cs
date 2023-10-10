using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnModule : DeathModule
{
    #region Variables
    [Tooltip("The respawn point the player has reached.")]
    [ReadOnly]
    public Transform lastRespawnPoint;

    private Vector3 initialPosition; 
    #endregion

    #region Properties
    /// <summary>
    /// The initial position of the respawn.
    /// </summary>
    public Vector3 RespawnPosition
    {
        get
        {
            if (lastRespawnPoint)
                return lastRespawnPoint.position;
            else if (GameManager.Instance.defaultRespawnPoint)
                return GameManager.Instance.defaultRespawnPoint.position;
            else
                return initialPosition;
        }
    }
    #endregion

    #region Methods
    private void Start()
    {
        initialPosition = transform.position;
    }

    protected override void OnDeath()
    {
        Master.transform.position = RespawnPosition;
        Master.Revive();
    } 
    #endregion
}
