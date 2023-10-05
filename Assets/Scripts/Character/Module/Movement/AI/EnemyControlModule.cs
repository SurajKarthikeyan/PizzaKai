using System;
using System.Collections;
using NaughtyAttributes;
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

    // public Arc sightline = new(-160, 160, 10);

    [Tooltip("How long to wait before recomputing the path " +
        "(after arrival at target)?")]
    [SerializeField]
    private Range pathRecomputationDelay = new(2, 5);

    [Tooltip("Delay for recalculating the movement vectors.")]
    [SerializeField]
    private Range targetTokenRefreshDelay = new(0.5f, 2);

    private Transform currentTarget;
    private IEnumerator tokenRefreshCR;
    #endregion

    #region Property
    public Transform CurrentTarget
    {
        get => currentTarget;
        set
        {
            if (currentTarget != value)
            {
                currentTarget = value;
                pathAgent.SetTarget(
                    currentTarget
                );
            }
        }
    }
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

    private void OnEnable()
    {
        // For now, just have them chase the player.
        CurrentTarget = FindObjectOfType<PlayerControlModule>().transform;
    }
    #endregion

    #region Main Logic
    public void AcceptToken(TargetToken token)
    {
        var heading = token.GetHeading(transform.position).normalized;
        movement.inputtedMovement = heading;
        movement.inputtedJump = heading.y > 0;

        if (!heading.x.Approx(0, 0.1f))
            Master.SetLookAngle(heading.x > 0 ? 0 : 180);
    }

    public void ArrivedAtDestination()
    {
        StartCoroutine(AnotherTarget_CR());
    }
    #endregion

    #region Helpers
    private IEnumerator AnotherTarget_CR()
    {
        do
        {
            yield return new WaitForSeconds(pathRecomputationDelay.Evaluate());
        } while (transform.position.TaxicabDistance(CurrentTarget.position) < 2);

        pathAgent.SetTarget(
            currentTarget
        );
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