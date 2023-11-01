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
    [SerializeField]
    private TargetToken nextToken;

    private Coroutine navigationCR;
    private Coroutine checkTargetDistanceCR;
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
                    this.StopCoroutineIfExists(navigationCR, checkTargetDistanceCR);
                    navigationCR = null;
                    checkTargetDistanceCR = null;
                    CurrentPath = null;
                    CurrentToken = null;
                    break;

                case NavigationState.WaitingForPath:
                    stuckTimer.Reset();
                    break;

                case NavigationState.ArrivedAtDestination:
                    NextNode = null;
                    print("Arrived at position " + CurrentToken);
                    enemyControl.ArrivedAtDestination(CurrentToken);
                    visualizer.Clear();

                    if (checkTargetDistanceCR != null)
                    {
                        StopCoroutine(checkTargetDistanceCR);
                        checkTargetDistanceCR = null;
                    }

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
    /// The current target token, if available. Otherwise, this is set to null.
    /// </summary>
    public TargetToken CurrentToken { get; private set; }

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

            // print($"{GridPosition} | {NextNode} | {GridPosition == NextNode.id}");
        }
    }
    #endregion

    #region Navigation
    public void ResetTarget()
    {
        SetTarget(CurrentToken);
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
        catch (PathfindingException e)
        {
            Debug.LogError(e);
            print("Attempting to recover");

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
            CurrentToken = token;
            visualizer.Trace(
                CurrentPath,
                (vector) => PathfindingManager.Instance.CellToWorld(vector.Value),
                $"Agent {gameObject.name}"
            );
            navigationCR = StartCoroutine(Navigation_CR(token));
        }
    }

    private IEnumerator Navigation_CR(TargetToken token)
    {
        yield return new WaitUntil(() => enemyControl);

        State = NavigationState.NavigatingToDestination;
        checkTargetDistanceCR = StartCoroutine(CheckTargetDistance_CR(token));
        NextNode = CurrentPath.Start;

        while (NextNode != CurrentPath.End)
        {
            NextNode = CurrentPath.Next(NextNode);
            nextToken = new(NextNode.Value);
            enemyControl.AcceptToken(nextToken);

            while (!CheckAlongPath())
            {
                yield return new WaitForSecondsRealtime(
                    PathAgentManager.Instance.aiUpdateRate
                );
            }

            print($"Arrived at {NextNode}");
            stuckTimer.Reset();
            StopCoroutine(checkTargetDistanceCR);
            checkTargetDistanceCR = StartCoroutine(CheckTargetDistance_CR(token));
        }

        State = NavigationState.ArrivedAtDestination;
    }

    private IEnumerator CheckTargetDistance_CR(TargetToken token)
    {
        Vector3 originalTargetPosition = token.Position;

        // Makes sure the updates are staggered, rather than occurring all at
        // once.
        yield return new WaitForSecondsRealtime(
            RNGExt.RandomFloat(0f, PathAgentManager.Instance.aiUpdateRate)
        );

        while (enabled)
        {
            // Would want to batch this somehow so this doesn't all run at the
            // same time.
            yield return new WaitForSecondsRealtime(
                PathAgentManager.Instance.aiUpdateRate
            );

            if (Vector3.Distance(originalTargetPosition, token.Position) > 1 ||
                stuckTimer.IsDone)
            {
                // Restart navigation.
                if (!PathfindingManager.Instance.InSameCell(
                        CurrentToken.Position,
                        enemyControl.transform.position
                    ))
                {
                    // print("Restarting nav");
                    SetTarget(CurrentToken);
                }
            }
            else
            {
                // Resend the token again.
                enemyControl.AcceptToken(token);
            }
        }
    }

    /// <summary>
    /// Check if we've arrived at a later node down the path.
    /// </summary>
    /// <returns></returns>
    private bool CheckAlongPath()
    {
        var currentGridPos = GridPosition;
        foreach (var node in CurrentPath.GetVertices(NextNode))
        {
            if (node.id == currentGridPos)
            {
                // Found the node we need to be on.
                NextNode = node;
                return true;
            }
        }

        return false;
    }
    #endregion
}