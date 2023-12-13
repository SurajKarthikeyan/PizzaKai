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

    public PlayerAnimationEvents playeraAnimEvent;
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
        weaponMaster.weaponsAvailable = true;
        if (!ranDeathAction)
        {
            playeraAnimEvent.RemoveGlowTex();
            StopCoroutine(Master.DamageFlash());
            Debug.Log("Hey You're in respawnOnDeath");
            ranDeathAction = true;
            Master.transform.position = RespawnPosition;
            Master.Revive();
            Master.characterAnimator.Play("PizzaGuy_Idle");
            DialogueManager.Instance.StopPlayer(false);
            weaponMaster.gameObject.SetActive(true);
        }
        
    } 

    protected override void StartDeath()
    {
        base.StartDeath();
    }
    #endregion
}
