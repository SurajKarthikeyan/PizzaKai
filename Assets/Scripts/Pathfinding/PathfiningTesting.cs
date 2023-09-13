using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Lmao I spelt this incorrectly.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class PathfiningTesting : MonoBehaviour
{
    #region Variables
    public Transform startMarker;

    public Transform endMarker;

    public Tracer<Vector3Int> visualizer;
    #endregion

    #region Instantiate
    private void Start()
    {
        TestPathfinding();
    }
    #endregion

    #region Testing
    /// <summary>
    /// Tests the pathfinding.
    /// </summary>
    [Button]
    public void TestPathfinding()
    {
        var path = PathfindingManager.Instance.AStarSearch(
            startMarker.position, endMarker.position
        );

        Grid grid = PathfindingManager.Instance.grid;
        visualizer.Trace(
            path,
            (vertex) => vertex.Value + grid.GetCellCenterWorld(Vector3Int.zero),
            $"Pathfinding Testing {gameObject.name}"
        );
    }
    #endregion
}