using System.Collections;
using System.Linq;
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

    [Tooltip("How high can this thing jump?")]
    public int maxJumpHeight = 4;

    [SerializeField]
    private Tracer<Vector3Int> visualizer;

    [Tooltip("How often to check for navigation updates?")]
    [SerializeField]
    private float aiUpdateRate = 2f;

    [Tooltip("After how long after arriving at the previous node is this " +
        "agent considered stuck?")]
    [SerializeField]
    private Duration stuckTimer = new(8);

    [Header("Debug")]
    [SerializeField]
    private TargetToken nextToken;

    private Coroutine navigationCR;
    private Coroutine checkTargetDistanceCR;
    #endregion

    #region Properties
    /// <summary>
    /// Current waiting state of the agent.
    /// </summary>
    public NavigationState State { get; private set; }

    /// <summary>
    /// Current path of the agent, if available.
    /// </summary>
    public Path<Vector3Int> CurrentPath { get; private set; }

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
    }

    private void OnEnable()
    {
        PathAgentManager.Instance.ActiveAgents.Add(this);
    }

    private void OnDisable()
    {
        PathAgentManager.Instance.ActiveAgents.Remove(this);
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
    /// <inheritdoc cref="SetTarget(TargetToken, int)"/>
    public void SetTarget(TargetToken target)
    {
        SetTarget(target, 9);
    }

    /// <summary>
    /// Requests that a path be generate from the agent's current position to
    /// <see cref="target"/>. If another request is pending, stop that request
    /// first.
    /// </summary>
    /// <param name="target">Where to navigate this agent.</param>
    /// <param name="recoverAttemptsLeft">On caught exception, how many times do
    /// we try restarting the navigation before we give up?</param>
    public void SetTarget(TargetToken target, int recoverAttemptsLeft)
    {
        // Requesting a new path while one is already being generated may not be
        // desirable.
        if (State != NavigationState.Idle)
            StopCurrentNavigation();

        stuckTimer.Reset();

        State = NavigationState.WaitingForPath;

        if (target.GridTarget == GridPosition)
        {
            // We've already arrived. This avoid a StartIsEndVertexException.
            ArrivedAtDestination();
            return;
        }

        try
        {
            PathAgentManager.Instance.Schedule(this, target);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            print("Attempting to recover");

            StartCoroutine(RecoverFromError_CR(target, recoverAttemptsLeft));
        }
    }

    private IEnumerator RecoverFromError_CR(TargetToken token,
        int recoverAttemptsLeft)
    {
        yield return new WaitForSeconds(10f);
        SetTarget(token);
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
    /// Stops the current running navigation coroutines, allowing the agent to
    /// compute a new path to the target.
    /// </summary>
    public void StopCurrentNavigation()
    {
        if (navigationCR != null)
        {
            StopCoroutine(navigationCR);
            StopCoroutine(checkTargetDistanceCR);
        }
        navigationCR = null;
        checkTargetDistanceCR = null;
        State = NavigationState.Idle;
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
            visualizer.Trace(
                CurrentPath,
                (vector) => PathfindingManager.Instance.CellToWorld(vector.Value),
                $"Agent {gameObject.name}"
            );
            navigationCR = StartCoroutine(Navigation_CR(token));
        }
        else
            State = NavigationState.Idle;
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

            int cnt = 0;
            while (!CheckAlongPath())
            {
                cnt++;
                float waitTime = Mathf.Min(
                    aiUpdateRate,
                    cnt * Time.deltaTime
                );

                yield return new WaitForSecondsRealtime(waitTime);
            }

            print($"Arrived at {NextNode}");
            stuckTimer.Reset();
            StopCoroutine(checkTargetDistanceCR);
            checkTargetDistanceCR = StartCoroutine(CheckTargetDistance_CR(token));
        }

        ArrivedAtDestination();
    }

    private void ArrivedAtDestination()
    {
        NextNode = null;
        State = NavigationState.ArrivedAtDestination;
        enemyControl.ArrivedAtDestination();
        visualizer.Clear();

        if (checkTargetDistanceCR != null)
        {
            StopCoroutine(checkTargetDistanceCR);
            checkTargetDistanceCR = null;
        }
    }

    private IEnumerator CheckTargetDistance_CR(TargetToken token)
    {
        Vector3 originalTargetPosition = token.Target;

        // Makes sure the updates are staggered, rather than occurring all at
        // once.
        Range updateOffset = new(-aiUpdateRate, aiUpdateRate);
        yield return new WaitForSecondsRealtime(updateOffset.Evaluate());

        while (enabled)
        {
            // Would want to batch this somehow so this doesn't all run at the
            // same time.
            yield return new WaitForSecondsRealtime(aiUpdateRate);

            if (Vector3.Distance(originalTargetPosition, token.Target) > 1 ||
                stuckTimer.IsDone)
            {
                // Restart navigation.
                if (enemyControl.CurrentTarget &&
                    !PathfindingManager.Instance.InSameCell(
                        enemyControl.CurrentTarget.position,
                        enemyControl.transform.position
                    ))
                {
                    print("Restarting nav");
                    SetTarget(enemyControl.CurrentTarget);
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