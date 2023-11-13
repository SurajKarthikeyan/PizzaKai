using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Manager script that handles <see cref="PathfindingAgent"/> scheduling.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class PathAgentManager : MonoBehaviour
{
    #region Structs
    public struct AgentThread
    {
        public Vector3Int current;
        public Vector3Int target;

        public AgentThread(Vector3Int current, Vector3Int target)
        {
            this.target = target;
            this.current = current;
        }

        public readonly void ThreadProcess(out Path<Vector3Int> path)
        {
            path = PathfindingManager.Instance.AStarSearch(
                current,
                target
            );

            
        }
    }
    #endregion

    #region Instance
    /// <summary>
    /// The static reference to a(n) <see cref="PathAgentManager"/> instance.
    /// </summary>
    private static PathAgentManager instance;

    /// <inheritdoc cref="instance"/>
    public static PathAgentManager Instance => instance;
    #endregion

    private void Awake()
    {
        this.InstantiateSingleton(ref instance);
        ActiveAgents = new();
    }

    #region Variables
    [BoxGroup("AI Timing")]
    [Tooltip("How long will the path agent wait before recomputing the path " +
        "after the agent arrives at its target?")]
    public Range pathRecomputeDelay = new(0.2f, 1);

    [BoxGroup("AI Timing")]
    [Tooltip("How long between stuck checks?")]
    public float stuckCheckDelay = 0.7f;

    [BoxGroup("AI Timing")]
    [Tooltip("How often to check for navigation updates?")]
    [SerializeField]
    private float baseAiUpdateRate = 0.4f;

    [BoxGroup("Debug")]
    [Tooltip("If true, then use multithreading. Disable this if you want " +
        "to test pathfinding.")]
    public bool useThreads = true;
    #endregion

    #region Properties
    /// <summary>
    /// How often to check for navigation updates? Scales with
    /// current performance.
    /// </summary>
    public float AIUpdateRate { get; private set; }

    /// <summary>
    /// Collection of currently active agents.
    /// </summary>
    public HashSet<PathfindingAgent> ActiveAgents { get; private set; }

    /// <summary>
    /// Alias for <see cref="PathfindingManager.Instance.Pathfinding"/>.
    /// </summary>
    public Graph<Vector3Int> Pathfinding => PathfindingManager.Instance.Pathfinding;
    #endregion

    #region Main Logic
    private void Update()
    {
        float t = Mathf.Clamp(Time.deltaTime, 0.5f, 1);
        AIUpdateRate = baseAiUpdateRate * t;
    }

    /// <summary>
    /// Schedules this agent to generate its path.
    /// </summary>
    /// <param name="agent">The pathfinding agent.</param>
    /// <param name="target">The TargetToken to target.</param>
    public void Schedule(PathfindingAgent agent, TargetToken target)
    {
        var current = agent.GridPosition;

        // Make sure target valid.
        try
        {
            var startV = PathfindingManager.Instance.GetClosestNeighbor(current);
            var endV = PathfindingManager.Instance.GetClosestNeighbor(target.GridPosition, startV.sectionID);

            Pathfinding.ValidateStartEnd(true, startV, endV);
        }
        catch (PathfindingException e) when (e is StartIsEndVertexException)
        {
            // AI has already arrived.
            agent.State = PathfindingAgent.NavigationState.ArrivedAtDestination;
            return;
        }
        catch (PathfindingException)
        {
            // Do nothing.
        }

        try
        {
            StartCoroutine(WaitForTask_CR(agent, target,
                new(
                    current,
                    target.GridPosition
                )
            ));
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    private IEnumerator WaitForTask_CR(PathfindingAgent agent,
        TargetToken token, AgentThread thread)
    {
        Path<Vector3Int> path = null;
        if (useThreads)
        {
            // Tasks are managed by C#'s ThreadPool, so you can create as many as
            // you want (I think).
            var task = Task.Run(() => thread.ThreadProcess(out path));

            yield return new WaitUntil(() => task.IsCompleted);

            // // Forces task to actually complete.
            // task.Wait();

            if (!task.IsCompletedSuccessfully)
            {
                throw task.Exception;
            }
            else
            {
                agent.AcceptPath(path, token);
            }
        }
        else
        {
            // Test on main thread to better view errors n' such.
            thread.ThreadProcess(out path);
            agent.AcceptPath(path, token);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}