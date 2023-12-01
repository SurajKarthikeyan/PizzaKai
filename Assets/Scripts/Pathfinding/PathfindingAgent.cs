using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Contains information on how the agent will navigate through the environment.
/// This receives targeting information from <see cref="EnemyControlModule"/>,
/// then returns a token that will tell the enemy which way to move.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[RequireComponent(typeof(EnemyControlModule))]
public class PathfindingAgent : MonoBehaviour
{
    #region State
    public enum NavigationState
    {
        /// <summary>
        /// The agent is idle.
        /// </summary>
        Idle,
        /// <summary>
        /// The agent is waiting for its path to be generated.
        /// </summary>
        WaitingForPath,
        /// <summary>
        /// The agent is navigating towards its destination.
        /// </summary>
        NavigatingToDestination,
        /// <summary>
        /// The agent has arrived at its destination grid square.
        /// </summary>
        ArrivedAtDestination
    }
    #endregion

    #region Variables
    /// <summary>
    /// The attached enemy control module.
    /// </summary>
    private EnemyControlModule enemyControl;

    [SerializeField]
    private Tracer<Vector3Int> visualizer;

    [Tooltip("After how long after arriving at the previous node is this " +
        "agent considered stuck?")]
    [SerializeField]
    [ReadOnly]
    private Duration stuckTimer = new(8);

    [Header("Debug")]
    private Coroutine navigationCR;
    private NavigationState state;
    #endregion

    #region Properties
    /// <summary>
    /// Current waiting state of the agent.
    /// </summary>
    public NavigationState State
    {
        get => state;
        set
        {
            if (state == value)
                return;

            switch (value)
            {
                case NavigationState.Idle:
                    this.StopCoroutineIfExists(navigationCR);
                    navigationCR = null;
                    CurrentPath = null;
                    FinalToken = null;
                    break;

                case NavigationState.WaitingForPath:
                    stuckTimer.Reset();
                    break;

                case NavigationState.ArrivedAtDestination:
                    NextNode = null;
                    if (PathAgentManager.Instance.isVerbose)
                    {
                        Debug.Log(
                            "Arrived at final destination " + FinalToken,
                            enemyControl
                        );
                    }
                    enemyControl.ArrivedAtDestination(FinalToken);
                    visualizer.Clear();

                    break;
            }
            state = value;
        }
    }

    /// <summary>
    /// Current path of the agent, if available. Otherwise, this is set to null.
    /// </summary>
    public Path<Vector3Int> CurrentPath { get; private set; }

    /// <summary>
    /// The final target token, if available. Otherwise, this is set to null.
    /// </summary>
    private TargetToken FinalToken { get; set; }

    /// <summary>
    /// Position of this agent in world coordinates.
    /// </summary>
    public Vector3 WorldPosition => transform.position;

    /// <summary>
    /// Position of this agent in grid coordinates.
    /// </summary>
    public Vector3Int GridPosition => PathfindingManager.Instance
        .WorldToCell(WorldPosition);

    public Vertex<Vector3Int> NextNode { get; private set; }

    public Vector3 NextNodeWorldPosition => PathfindingManager.Instance
        .CellToWorld(NextNode.Value);
    #endregion

    #region MonoBehavior Functions
    private void Awake()
    {
        this.RequireComponent(out enemyControl);
        stuckTimer = new(PathAgentManager.Instance.stuckCheckDelay);
    }

    private void OnEnable()
    {
        PathAgentManager.Instance.ActiveAgents.Add(this);
    }

    private void OnDisable()
    {
        PathAgentManager.Instance.ActiveAgents.Remove(this);
    }

    private void OnDestroy()
    {
        visualizer.Clear(true);
    }

    private void Update()
    {
        if (State == NavigationState.NavigatingToDestination)
        {
            stuckTimer.IncrementUpdate(false);
        }
    }
    #endregion

    #region Navigation
    public void ResetTarget()
    {
        SetTarget(FinalToken);
    }

    /// <inheritdoc cref="SetTarget(TargetToken, int)"/>
    public void SetTarget(TargetToken target)
    {
        SetTarget(target, 0);
    }

    /// <summary>
    /// Requests that a path be generate from the agent's current position to
    /// <see cref="target"/>. If another request is pending, stop that request
    /// first.
    /// </summary>
    /// <param name="target">Where to navigate this agent.</param>
    /// <param name="recoverAttempts">On caught exception, how many times
    /// have we tried restarting?</param>
    public void SetTarget(TargetToken target, int recoverAttempts)
    {
        // Requesting a new path while one is already being generated may not be
        // desirable.
        State = NavigationState.Idle;
        State = NavigationState.WaitingForPath;

        if (target.GridPosition == GridPosition)
        {
            // We've already arrived. This avoids a StartIsEndVertexException.
            State = NavigationState.ArrivedAtDestination;
            return;
        }

        try
        {
            PathAgentManager.Instance.Schedule(this, target);
        }
        catch (StartIsEndVertexException)
        {
            // We've already arrived. This may not be caught by the above if
            // statement if start is interpolated to end (or vice versa).
            State = NavigationState.ArrivedAtDestination;
            return;
        }
        catch (PathfindingException e)
        {
            Debug.LogError(e);
            print("Attempting to recover from pathfinding error.");

            StartCoroutine(RecoverFromError_CR(target, recoverAttempts));
        }
        catch (AggregateException e)
        {
            Debug.LogError(e);

            if (e.InnerExceptions.Any(inner => inner is PathfindingException))
                print("Attempting to recover from aggregate error " +
                    "containing at least one pathfinding error.");
            else
                print("Attempting to recover from unknown aggregate error.");

            StartCoroutine(RecoverFromError_CR(target, recoverAttempts));
        }
        catch (Exception e)
        {
            Debug.LogError(e);

            print("Attempting to recover from unknown error");

            StartCoroutine(RecoverFromError_CR(target, recoverAttempts));
        }
    }

    private IEnumerator RecoverFromError_CR(TargetToken token,
        int recoverAttempts)
    {
        if (recoverAttempts > 0)
        {
            yield return new WaitForSecondsRealtime(
                PathAgentManager.Instance.stuckCheckDelay
            );
            SetTarget(token, --recoverAttempts);
        }
        else
        {
            Debug.LogError("Unable to recover. Used up all recover attempts.");
        }
    }

    /// <inheritdoc cref="SetTarget(TargetToken)"/>
    public void SetTarget(Transform target)
    {
        SetTarget(new TargetToken(target));
    }

    /// <inheritdoc cref="SetTarget(TargetToken)"/>
    public void SetTarget(Vector3 target)
    {
        SetTarget(new TargetToken(target));
    }


    /// <summary>
    /// Accepts the path generated by the <see cref="PathAgentManager"/>.
    /// </summary>
    /// <param name="path">The generated path.</param>
    /// <param name="token">The target token.</param>
    public void AcceptPath(Path<Vector3Int> path, TargetToken token)
    {
        if (enabled)
        {
            CurrentPath = path;
            FinalToken = token;
            visualizer.Trace(
                CurrentPath,
                (vector) => PathfindingManager.Instance.CellToWorld(vector.Value),
                $"Agent {gameObject.name}"
            );
            navigationCR = StartCoroutine(Navigation_CR());
        }
    }

    private IEnumerator Navigation_CR()
    {
        yield return new WaitUntil(() => enemyControl);

        State = NavigationState.NavigatingToDestination;
        NextNode = CurrentPath.Start;

        while (NextNode != CurrentPath.End)
        {
            while (!CheckAlongPath(NextNode))
            {
                yield return new WaitForSecondsRealtime(
                    PathAgentManager.Instance.AIUpdateRate
                );
            }

            if (PathAgentManager.Instance.isVerbose)
            {
                Debug.Log(
                    "Arrived at intermediate position " + NextNode,
                    enemyControl
                );
            }

            stuckTimer.Reset();

            // Go to next node.
            NextNode = CurrentPath.Next(NextNode);
        }

        State = NavigationState.ArrivedAtDestination;
    }

    /// <summary>
    /// Check if we've arrived at a later node down the path.
    /// </summary>
    /// <returns></returns>
    private bool CheckAlongPath(Vertex<Vector3Int> next)
    {
        var currentGridPos = GridPosition;
        currentGridPos.z = 0;

        // Iterate through all nodes starting past NextNode.
        foreach (var node in CurrentPath.GetVertices(next))
        {
            if (node.id == currentGridPos)
            {
                // Found the node we need to be on.
                return true;
            }
        }

        // Same thing, but do a "close enough" check instead.
        foreach (var node in CurrentPath.GetVertices(next))
        {
            if (node.id.TaxicabDistance(currentGridPos) <= 1)
            {
                // Found the node we need to be on.
                return true;
            }
        }

        return false;
    }
    #endregion

    #region Getters
    public Vector2 GetHeading()
    {
        return State switch
        {
            NavigationState.NavigatingToDestination =>
                GetNormalized(NextNodeWorldPosition),
            NavigationState.ArrivedAtDestination =>
                GetNormalized(FinalToken.Position),
            _ => Vector2.zero
        };

        Vector3 GetNormalized(Vector3 position)
        {
            return (position - transform.position).normalized;
        }
    }
    #endregion
}