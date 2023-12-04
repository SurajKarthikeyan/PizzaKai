using NaughtyAttributes;
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

    protected override void StartDeath()
    {
        // Need to play the death animation here.
        Master.characterAnimator.Play("PizzaGuy_Idle");
        base.StartDeath();
    }

    protected override void OnDeath()
    {
        Debug.Log("Hey You're in respawnOnDeath");
        Master.Revive(RespawnPosition);
        Master.characterAnimator.SetBool("Death", false);
    }
    #endregion
}
