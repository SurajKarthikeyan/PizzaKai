using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager script for scheduling AI behaviors.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class AIManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to a(n) 
    /// <see cref="AIManager"/> instance.
    /// </summary>
    private static AIManager instance;
    
    /// <inheritdoc cref="instance"/>
    public static AIManager Instance => instance;
    #endregion
    
    private void Awake()
    {
        this.InstantiateSingleton(ref instance, false);
    }

    #region Variables
    [Tooltip("The maximum number of AI updates to run in one update.")]
    public int maxConcurrentDecisions = 32;

    [Tooltip("How often to update the AI.")]
    public Duration refreshRate = new(0.2f);
    #endregion

    #region Member Variables
    /// <summary>
    /// The queue of AIs this manager will update.
    /// </summary>
    private Queue<EnemyControlModule> waiting = new();
    #endregion

    #region Instantiation
    private void Start()
    {
        refreshRate.CreateCallback(
            this, Loop,
            Duration.Options.Repeat | Duration.Options.UnscaledTime
        );
    }
    #endregion

    #region Main Logic
    /// <summary>
    /// Registers the provided AI tree module.
    /// </summary>
    /// <param name="ai"></param>
    public void RegisterAI(EnemyControlModule ai)
    {
        waiting.Enqueue(ai);
    }

    /// <summary>
    /// The main loop for the manager.
    /// </summary>
    private void Loop()
    {
        int length = Mathf.Min(maxConcurrentDecisions, waiting.Count);

        for (int i = 0; i < length; i++)
        {
            // Dequeue an element, update it, and requeue it. This allows each
            // enemy to be updated in order, avoiding starvation.
            var toUpdate = waiting.Dequeue();

            // Avoid referencing destroyed enemies. This also allows destroyed
            // enemies to be conveniently removed from the catalog.
            if (toUpdate && toUpdate.decisionTree)
            {
                toUpdate.UpdateEnemyAIControl(
                    refreshRate.maxTime
                );
                waiting.Enqueue(toUpdate);
            }
        }
    }
    #endregion
}