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

    private void Start()
    {
        // // For testing
        // transform.position = RandomSpawnPosition();
        // var poiInst = poiReference.InstantiateComponent();
        // poiInst.transform.parent = transform;
        // poiInst.transform.position = transform.position;
        // poiInst.color = Color.green;

        // SetTarget(
        //     RandomSpawnGrid()
        // );
    }

    private Vector3 RandomSpawnPosition()
    {
        return PathfindingManager.Instance.CellToWorld(
            RandomSpawnGrid()
        );
    }

    private Vector3Int RandomSpawnGrid()
    {
        return PathfindingManager.Instance.CellBounds.RandomPointIn();
    }
    #endregion

    #region Navigation
    /// <summary>
    /// Requests that a path be generate from the agent's current position to
    /// <see cref="target"/>. If another request is pending, stop that request
    /// first.
    /// </summary>
    /// <param name="target">Where to navigate this agent.</param>
    public void SetTarget(TargetToken target)
    {
        // Requesting a new path while one is already being generated may not be
        // desirable.
        if (State != NavigationState.Idle)
            StopCurrentNavigation();

        State = NavigationState.WaitingForPath;

        PathAgentManager.Instance.Schedule(this, target);
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
        StopCoroutine(navigationCR);
        StopCoroutine(checkTargetDistanceCR);
        navigationCR = null;
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
            // visualizer.Trace(
            //     CurrentPath,
            //     (vector) => PathfindingManager.Instance.CellToWorld(vector.Value)
            // );
            navigationCR = StartCoroutine(Navigation_CR(token));
        }
        else
            State = NavigationState.Idle;
    }

    private IEnumerator Navigation_CR(TargetToken token)
    {
        State = NavigationState.NavigatingToDestination;
        checkTargetDistanceCR = StartCoroutine(CheckTargetDistance_CR(token));
        var next = CurrentPath.Start;

        while (next != CurrentPath.End)
        {
            next = CurrentPath.Next(next);
            nextToken = new(next.Value);
            enemyControl.AcceptToken(nextToken);

            yield return new WaitUntil(() => GridPosition == next.Value);
        }

        State = NavigationState.ArrivedAtDestination;
        visualizer.Clear();
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

            if (Vector3.Distance(originalTargetPosition, token.Target) > 1)
            {
                // Restart navigation.
                if (enemyControl.CurrentTarget)
                {
                    print("Restarting nav");
                    SetTarget(enemyControl.CurrentTarget);
                }
            }
        }
    }
    #endregion
}