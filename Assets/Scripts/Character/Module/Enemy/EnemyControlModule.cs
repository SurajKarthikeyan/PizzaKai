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
    [SerializeField]
    [Tooltip("What must the y input be for the control module to request " +
        "a jump?")]
    [BoxGroup("Settings")]
    private float jumpThreshold = 0.5f;

    [ReadOnly]
    [BoxGroup("Required Modules")]
    public CharacterMovementModule movement;

    [ReadOnly]
    [BoxGroup("Required Modules")]
    public PathfindingAgent pathAgent;

    [Tooltip("The root element of the decision tree.")]
    [BoxGroup("Required Modules")]
    public AITreeModule decisionTree;

    // [Tooltip("Delay for recalculating the movement vectors.")]
    // [SerializeField]
    // private Range targetTokenRefreshDelay = new(0.5f, 2);
    //private IEnumerator tokenRefreshCR;
    #endregion

    #region Member Variables
    /// <summary>
    /// Callback for the <see cref="SetMoveTarget(TargetToken,
    /// Action{TargetToken})"/> function.
    /// </summary>
    private Action<TargetToken> cachedCallback;
    #endregion

    #region Instantiation
    private void Awake()
    {
        SetVars();
    }

    private void OnValidate()
    {
        SetVars();

        if (!decisionTree && !this.HasComponentInChildren(out decisionTree))
        {
            decisionTree = transform.CreateChildComponent<AITreeModule>("Decision Tree");
        }
    }

    private void SetVars()
    {
        this.RequireComponent(out movement);
        this.RequireComponent(out pathAgent);
    }

    private void Start()
    {
        AIManager.Instance.RegisterAI(this);
        decisionTree.AIInitialize(this);
    }
    #endregion

    #region Main Logic
    /// <summary>
    /// Performs a movement update.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void UpdateEnemyAIControl(float deltaTime)
    {
        decisionTree.AIUpdate(this, deltaTime);

        if (pathAgent.State == PathfindingAgent.NavigationState.NavigatingToDestination &&
            pathAgent.NextNode != null)
        {
            UpdateMovementDirection();
        }
    }

    /// <inheritdoc cref="PathfindingAgent.SetTarget(TargetToken)"/>
    public void SetMoveTarget(TargetToken target) => pathAgent.SetTarget(target);

    /// <inheritdoc cref="SetMoveTarget(TargetToken)"/>
    /// <param name="callOnArrival">Callback on arrival.</param>
    public void SetMoveTarget(TargetToken target,
        Action<TargetToken> callOnArrival)
    {
        pathAgent.SetTarget(target);
        cachedCallback = callOnArrival;
    }

    /// <summary>
    /// Clears the movement target token.
    /// </summary>
    public void ClearMoveTarget()
    {
        pathAgent.State = PathfindingAgent.NavigationState.Idle;

        // Stop all movement.
        movement.inputtedMovement = Vector2.zero;
        movement.inputtedDash = Vector2.zero;
        movement.inputtedJump = false;
    }

    /// <summary>
    /// Notifies the EnemyControlModule that it has arrived at its destination.
    /// </summary>
    /// <param name="token">The token used to get to the destination.</param>
    public void ArrivedAtDestination(TargetToken token)
    {
        // // Stop all movement.
        // movement.inputtedMovement = Vector2.zero;
        // movement.inputtedDash = Vector2.zero;
        // movement.inputtedJump = false;

        // if (cachedCallback != null)
        // {
        //     cachedCallback(token);
        //     cachedCallback = null;
        // }
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Accepts a movement token, which contains the data required to move the
    /// character.
    /// </summary>
    /// <param name="token">The movement/target token.</param>
    private void UpdateMovementDirection()
    {
        Vector2 heading = pathAgent.GetHeading();
        movement.inputtedMovement = heading;
        movement.inputtedJump = heading.y > jumpThreshold;

        if (-jumpThreshold < heading.y && heading.y < 0)
        {
            // Avoids pressing down unnecessarily.
            heading.y = 0;
        }

        if (!heading.x.Approx(0, 0.1f))
            Master.SetLookAngle(heading.x > 0 ? 0 : 180);
    }

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
    }
    #endregion

#if UNITY_EDITOR
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
#endif
}