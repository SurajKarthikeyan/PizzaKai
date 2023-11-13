using System.Collections;
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
    #region Variables
    [Tooltip("The maximum number of AI updates to run in one update.")]
    public int maxConcurrentDecisions = 32;

    [Tooltip("How often to update the AI.")]
    public Duration refreshRate = new(0.2f);
    #endregion
}