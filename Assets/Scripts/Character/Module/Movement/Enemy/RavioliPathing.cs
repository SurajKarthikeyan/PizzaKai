using System.Collections;
using UnityEngine;

public class RavioliPathing : EnemyControlModule
{
    #region Variables
    [Tooltip("Distance to the player at which to explode at")]
    [SerializeField] private float distanceCheck;
    #endregion

    #region Unity Methods    
    private void Update()
    {
        // If within the bounds to explode, explode.
        if (DistanceToPlayer() <= distanceCheck && !Master.IsDead)    
        {
            Master.Die();
        }
    }
    #endregion

    #region Private Methods
    private float DistanceToPlayer() => transform.Distance2D(GameManager.Instance.Player.transform);
    #endregion
}
