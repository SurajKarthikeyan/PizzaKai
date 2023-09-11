using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Testing script that displays the current cell coordinates where the
/// gameobject is.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[ExecuteInEditMode]
public class GridCellDebugger : MonoBehaviour
{
    public PathfindingManager pathManager;

    [SerializeField]
    private Color colorPrimary = Color.red;
    [SerializeField]
    private Color colorSecondary = Color.blue;

    [SerializeField]
    [ReadOnly]
    private Vector3Int currentCoordinates;

    [SerializeField]
    [ReadOnly]
    private List<PathNode> currentNodes;

    [SerializeField]
    [ReadOnly]
    private bool hasVertex;

    [SerializeField]
    [ReadOnly]
    private Vertex<Vector3Int> vertex;

    private void OnDrawGizmos()
    {
        if (!pathManager.grid)
            return;

        currentCoordinates = pathManager.WorldToCell(transform.position);
        DebugExt.UseGizmos(colorPrimary, colorSecondary);

        Bounds bounds = new(
            pathManager.grid.GetCellCenterWorld(currentCoordinates),
            pathManager.grid.cellSize
        );
        DebugExt.DrawCrossBounds(bounds);


        if (pathManager.Pathfinding.HasVertex(currentCoordinates))
        {
            vertex = pathManager.Pathfinding[currentCoordinates];
            currentNodes = vertex.Nodes;
            hasVertex = true;
        }
        else
        {
            vertex = null;
            currentNodes = null;
            hasVertex = false;
        }
    }
}