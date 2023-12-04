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
        base.StartDeath();
    }

    protected override void OnDeath()
    {
        if (!ranDeathAction)
        {
            Debug.Log("Hey You're in respawnOnDeath");
            ranDeathAction = true;
            Master.transform.position = RespawnPosition;
            Master.Revive();
            Master.characterAnimator.Play("PizzaGuy_Idle");
            DialogueManager.Instance.StopPlayer(false);
            weaponMaster.gameObject.SetActive(true);
        }

    }
    #endregion
}
