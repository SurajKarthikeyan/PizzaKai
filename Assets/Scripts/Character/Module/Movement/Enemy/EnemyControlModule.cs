using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This interfaces with <see cref="PathfindingAgent"/> and <see
/// cref="CharacterMovementModule"/>. It tells <see cref="PathfindingAgent"/>
/// where it wants to go, and gives that information to <see
/// cref="CharacterMovementModule"/>.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[RequireComponent(typeof(CharacterMovementModule), typeof(PathfindingAgent))]
public class EnemyControlModule : Module
{
    #region Variables
    [ReadOnly]
    public CharacterMovementModule movement;

    [ReadOnly]
    public PathfindingAgent pathAgent;

    public EnemyAITree decisionTree;

    // [Tooltip("Delay for recalculating the movement vectors.")]
    // [SerializeField]
    // private Range targetTokenRefreshDelay = new(0.5f, 2);
    //private IEnumerator tokenRefreshCR;
    #endregion

    #region Properties
    public Vector3 Target => pathAgent.CurrentToken?.Target
        ?? VectorExt.VECTOR3_NAN;
    #endregion

    #region Instantiation
    private void Awake()
    {
        SetVars();
    }

    private void OnValidate()
    {
        SetVars();
    }

    private void SetVars()
    {
        this.RequireComponent(out movement);
        this.RequireComponent(out pathAgent);
    }

    private void Start()
    {
        // For now, chase player.
        SetTarget(GameManager.Instance.Player.transform);
    }
    #endregion

    #region Main Logic
    /// <inheritdoc cref="PathfindingAgent.SetTarget(Vector3)"/>
    public void SetTarget(Vector3 target) => pathAgent.SetTarget(target);

    /// <inheritdoc cref="PathfindingAgent.SetTarget(Transform)"/>
    public void SetTarget(Transform target) => pathAgent.SetTarget(target);

    public void AcceptToken(TargetToken token)
    {
        var heading = token.GetHeading(transform.position).normalized;
        movement.inputtedMovement = heading;
        movement.inputtedJump = heading.y > 0;

        if (!heading.x.Approx(0, 0.1f))
            Master.SetLookAngle(heading.x > 0 ? 0 : 180);
    }

    /// <summary>
    /// Notifies the EnemyControlModule that it has arrived at its destination.
    /// </summary>
    public void ArrivedAtDestination()
    {
        // StartCoroutine(Idle_CR());
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Coroutine that monitors the distance between the enemy and target. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Idle_CR()
    {
        Debug.Assert(
            pathAgent.State == PathfindingAgent.NavigationState.Idle,
            $"{nameof(Idle_CR)} called when pathAgent.State is not idle!"
        );

        yield return new WaitForFixedUpdate();

        // do
        // {
        //     // As long as the enemy is within 2 tiles of the player, it will not
        //     // attempt to set a target.
        //     yield return new WaitForSeconds(
        //         PathAgentManager.Instance.pathRecomputeDelay.Evaluate()
        //     );
        // } while (transform.position.TaxicabDistance(pathAgent.) < 2);

        // pathAgent.ResetTarget();
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(
            transform.position + Vector3.up,
            movement.inputtedMovement.ToVector3()
        );
    }
    #endregion
}