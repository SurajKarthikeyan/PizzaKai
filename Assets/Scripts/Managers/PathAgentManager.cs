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
        private readonly PathfindingAgent agent;
        public Vector3Int current;

        public Vector3Int target;

        public AgentThread(PathfindingAgent agent, Vector3Int current,
            Vector3Int target)
        {
            this.agent = agent;
            this.target = target;
            this.current = current;
        }

        public void ThreadProcess(out Path<Vector3Int> path)
        {
            // if (!PathfindingManager.Instance.Pathfinding.PathExists(current, target))
            //     throw new System.InvalidOperationException();


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
        if (instance == null)
        {
            this.InstantiateSingleton(ref instance);
            ActiveAgents = new();
        }
    }

    #region Variables
    [Tooltip("If true, then use multithreading. Disable this if you want " +
        "to test pathfinding.")]
    public bool useThreads = true;

    [BoxGroup("AI Timing")]
    [Tooltip("How long will the path agent wait before recomputing the path " +
        "after the agent arrives at its target?")]
    public Range pathRecomputeDelay = new(0.2f, 1);

    [BoxGroup("AI Timing")]
    [Tooltip("How long between stuck checks?")]
    public float stuckCheckDelay = 0.7f;

    [BoxGroup("AI Timing")]
    [Tooltip("How often to check for navigation updates?")]
    public float aiUpdateRate = 0.4f;
    #endregion

    #region Properties
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
    /// <summary>
    /// Schedules this agent to generate its path.
    /// </summary>
    /// <param name="agent">The pathfinding agent.</param>
    /// <param name="target">The TargetToken to target.</param>
    public void Schedule(PathfindingAgent agent, TargetToken target)
    {
        var current = agent.GridPosition;
        StartCoroutine(WaitForTask_CR(agent, target,
            new(
                agent,
                current,
                target.GridTarget
            )
        ));
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