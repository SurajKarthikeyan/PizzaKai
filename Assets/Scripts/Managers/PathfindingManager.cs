using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static PathMarker;
using static PathNode;
using Vertex3Int = Vertex<UnityEngine.Vector3Int>;

/// <summary>
/// Manager script that handles the creation and maintenance of the navigation
/// graph.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[ExecuteAlways]
public class PathfindingManager : MonoBehaviour
{
    #region Instance
    /// <summary>
    /// The static reference to an PathfindingManager instance.
    /// </summary>
    private static PathfindingManager instance;

    /// <summary>
    /// The static reference to an PathfindingManager instance.
    /// </summary>
    public static PathfindingManager Instance => instance;
    #endregion

    private void Awake()
    {
        if (Application.isPlaying)
        {
            this.InstantiateSingleton(ref instance); 
        }
        else
        {
            InitData();
        }
    }

    #region Variables
    [Header("Main")]
    [Tooltip("Unity grid, used to help generate the graph.")]
    [ReadOnly]
    public Grid grid;

    [Tooltip("The visualizer for the graph.")]
    [SerializeField]
    private Tracer<Vector3Int> visualizer;

    [Tooltip("The tilemaps contained by grid.")]
    [ReadOnly]
    public List<Tilemap> tilemaps;

    /// <summary>
    /// Scriptable object to store the data.
    /// </summary>
    [SerializeField]
    [ReadOnly]
    private PathfindingData data;

    [Header("Maximum Iterations")]
    [Tooltip("How far to check upwards for platforms (in tiles)?")]
    [SerializeField]
    private int platformCheckCeiling = 8;

    // [Tooltip("How far to check for neighboring vertices?")]
    // [SerializeField]
    // private int neighborCheckMaxDistance = 16;

    [Header("Path Costs")]
    [Tooltip("The cost of jumping up one tile's worth of distance.")]
    public float jumpUpCost = 5;

    [Tooltip("The cost of jumping down one tile. Use this to control how " +
        "often the AI jumps down from things.")]
    public float jumpDownCost = 0;

    [Tooltip("The cost of moving 1 normal tile.")]
    public int defaultCost = 1;
    #endregion

    #region Properties
    /// <inheritdoc cref="data"/>
    private PathfindingData Data => data;

    /// <summary>
    /// The pathfinding graph.
    /// </summary>
    public Graph<Vector3Int> Pathfinding => Data.pathData;
    public BoundsInt CellBounds { get; private set; }
    #endregion

    #region Methods
    #region Instantiation
    private void Start()
    {
        if (!grid)
        {
            throw new NullReferenceException("Value of grid not set! " +
                "Press the Build Graph button (Managers > PathfindingManager).");
        }

        BoundsInt cellBounds = new();
        cellBounds.SetMinMax(
            VectorExt.WithMinComponents(
                tilemaps.Select(tm => tm.cellBounds.min).ToArray()
            ),
            VectorExt.WithMaxComponents(
                tilemaps.Select(tm => tm.cellBounds.max).ToArray()
            )
        );

        CellBounds = cellBounds;
    }

    private void InitData()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (!Data || Data.name != sceneName)
        {
            data = ScriptableObject.CreateInstance<PathfindingData>();

            string path = sceneName;
            path = $"Assets/ScriptableObjects/LevelData/{path}.asset";
            // path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
        }
    }
    #endregion

    #region Graph Instantiation
#if UNITY_EDITOR
    [Button]
    private void BuildGraph()
    {
        if (!grid)
            grid = GameObject.FindObjectOfType<Grid>();

        tilemaps = grid.GetComponentsInChildren<Tilemap>()
            .Where(map => PathNode.groundMask.ContainsLayer(map.gameObject.layer))
            .Reverse()
            .ToList();

        Data.pathData = new();

        // First, initialize all the path nodes.
        InitializeNodes();


        // Then, link the nodes together. This will require multiple passes.

        // Pass 1: Detect ground.
        LinkPass1();

        // Pass 2: Connect ground.
        LinkPass2();

        // Pass 3: Allow movement through platforms and check for drops.
        LinkPass3();

        // Trim graph.
        Pathfinding.TrimVertices();

        // Remove self-edges.
        Pathfinding.RemoveSelfPaths();

        // Detect separated graph segments.
        Pathfinding.DetectSections(true);

        // Save data.
        UnityEditor.EditorUtility.SetDirty(Data);

        print("Done");
    }

    /// <summary>
    /// Initializes all path nodes.
    /// </summary>
    private void InitializeNodes()
    {
        foreach (var map in tilemaps)
        {
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                // Create node + vertex.
                if (!Pathfinding.TryGetVertex(pos, out Vertex3Int vertex))
                {
                    // Create vertex if we can't find one.
                    vertex = new(pos);

                    // Add vertex to graph.
                    Pathfinding.AddVertex(vertex);
                }

                PathNode nodeCurrent = new(pos, map);

                // Check if the node actually provides any information.
                if (nodeCurrent.flags != NodeFlags.None)
                {
                    // Now add the new node to the vertex.
                    vertex.Nodes.Add(nodeCurrent);
                }
            }
        }

        foreach (var vertex in Pathfinding.Values.Where(v => v.Nodes.Count <= 0))
        {
            // Add empty nodes to vertexes without nodes.
            vertex.Nodes.Add(new(vertex.Value, null));
        }
    }

    /// <summary>
    /// Pass 1: Detect ground.
    /// </summary>
    private void LinkPass1()
    {
        foreach (var vertex in Pathfinding.Values)
        {
            if (vertex.Nodes.Any(n => !n.CanWalkThrough))
            {
                // Make sure we are not including actual ground here.
                continue;
            }

            if (Pathfinding.TryGetVertex(
                vertex.Value + Vector3Int.down,
                out Vertex3Int below))
            {
                // There exists a vertex below this one. Check if it is walkable
                // ground.
                var bNodes = below.Nodes.ToList();
                NodeFlags flagsToSet = NodeFlags.None;
                foreach (var bNode in bNodes)
                {
                    switch (bNode.Solidity)
                    {
                        case PathMarker.Solidity.Solid:
                            flagsToSet |= NodeFlags.WalkableOnSolid;
                            break;
                        case PathMarker.Solidity.Platform:
                            flagsToSet |= NodeFlags.WalkableOnPlatform;
                            break;
                    }
                }

                // Now set the node types for each node.
                foreach (var node in vertex.Nodes)
                {
                    node.flags |= flagsToSet;
                }
            }
        }
    }

    /// <summary>
    /// Pass 2: Connect walkable tiles.
    /// </summary>
    private void LinkPass2()
    {
        foreach (var vertex in Pathfinding.Values)
        {
            // Connect walkable nodes.
            if (SomeFlagsSet(vertex, NodeFlags.Walkable))
            {
                foreach (var neighbor in GetCardinalNeighbors(vertex.Value))
                {
                    if (SomeFlagsSet(neighbor, NodeFlags.Walkable))
                    {
                        // Add edge (both ways).
                        Vector3Int offset = neighbor.Value - vertex.Value;
                        float distMult = offset.magnitude;
                        float cost = defaultCost * distMult;
                        Pathfinding.AddEdge(vertex, neighbor, cost);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Pass 3: Allow movement through platforms and check for drops.
    /// </summary>
    private void LinkPass3()
    {
        foreach (var vertex in Pathfinding.Values)
        {
            // Search for platforms (if walkable).
            if (SomeFlagsSet(vertex, NodeFlags.Walkable))
            {
                // Check for any platforms above this tile.
                for (int y = 1; y < platformCheckCeiling; y++)
                {
                    Vector3Int pos = vertex.Value + new Vector3Int(0, y);

                    if (Pathfinding.TryGetVertex(
                        pos, out Vertex3Int ground
                    ))
                    {
                        if (ground.Nodes.Exists(n => n.Solidity == Solidity.Solid))
                        {
                            // Make sure we aren't phasing through walls.
                            break;
                        }

                        if (ground.Nodes.Exists(n => n.Solidity == Solidity.Platform))
                        {
                            // We've got a platform. Next check if the tile
                            // above is walkable.
                            if (Pathfinding.TryGetVertex(
                                pos + Vector3Int.up,
                                out Vertex3Int walkable
                            ))
                            {
                                Pathfinding.AddDoubleEdge(
                                    vertex, walkable,
                                    jumpUpCost * y, jumpDownCost * y
                                );
                            }

                            break;
                        }
                    }
                    else
                    {
                        // Have gone out of bounds.
                        break;
                    }
                }

                // Check for any drops to the left and right of this tile.
                CheckDrop(vertex, vertex.Value + Vector3Int.left);
                CheckDrop(vertex, vertex.Value + Vector3Int.right);
            }
        }
    }

    private void CheckDrop(Vertex3Int vertex, Vector3Int from)
    {
        if (Pathfinding.TryGetVertex(from, out var fromV))
        {
            if (fromV.Nodes.Any(n => n.Solidity == Solidity.Solid))
                return;

            for (int y = 0; y > -platformCheckCeiling; y--)
            {
                Vector3Int pos = from + new Vector3Int(0, y);
                if (Pathfinding.TryGetVertex(pos, out var vCheck))
                {
                    if (SomeFlagsSet(vCheck, NodeFlags.Walkable))
                    {
                        // Found a solid tile to drop onto.
                        Pathfinding.AddDoubleEdge(fromV, vertex);
                        Pathfinding.AddDoubleEdge(
                            fromV, vCheck,
                            jumpDownCost * Mathf.Abs(y),
                            jumpUpCost * Mathf.Abs(y)
                        );

                        // Return from execution, we've found what we were
                        // looking for.
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Visualizes the graph.
    /// </summary>
    [Button]
    private void Visualize()
    {
        // Visualize graph.
        visualizer.Trace(
            Pathfinding,
            (vector) => vector.Value + grid.GetCellCenterWorld(Vector3Int.zero),
            "Pathfinding Manager"
        );
    }

    /// <summary>
    /// Clears all visuals.
    /// </summary>
    [Button]
    private void ClearVisual()
    {
        visualizer.Clear();
    }
#endif
    #endregion

    #region Search Functions
    /// <summary>
    /// Searches for a path from start to end.
    /// </summary>
    /// <param name="start">Where to start navigation from.</param>
    /// <param name="end">Where to navigate to.</param>
    /// <returns></returns>
    public Path<Vector3Int> AStarSearch(Vector3 start, Vector3 end)
    {
        return AStarSearch(WorldToCell(start), WorldToCell(end));
    }

    /// <inheritdoc cref="AStarSearch(Vector3, Vector3)"/>
    public Path<Vector3Int> AStarSearch(Vector3Int start, Vector3Int end)
    {
        if (start == end)
            throw new StartIsEndVertexException("Cannot create graph", start);

        var startV = GetClosestNeighbor(start);
        var endV = GetClosestNeighbor(end, startV.sectionID);

        var path = Pathfinding.AStarSearch(startV.id, endV.id);

        return path;
    }
    #endregion

    #region Translations
    public Vector3 CellToWorld(Vector3Int gridPosition)
    {
        return grid.GetCellCenterWorld(gridPosition);
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        var w2c = grid.WorldToCell(worldPosition);
        w2c.z = 0;
        return w2c;
    }

    /// <summary>
    /// Returns true if all <paramref name="vectors"/> lie on the same grid
    /// cell.
    /// </summary>
    /// <param name="vectors"></param>
    /// <returns></returns>
    public bool InSameCell(params Vector3[] vectors)
    {
        if (vectors.Length < 2)
            return true;

        Vector3 og = vectors[0];

        foreach (var vector in vectors.Skip(1))
        {
            if (og != vector)
                return false;
        }

        return true;
    }
    #endregion

    #region Cell Methods
    /// <summary>
    /// Returns a collection of vertices that match <paramref name="selector"/>.
    /// </summary>
    /// <param name="selector">The function used to determine what vertices to
    /// select.</param>
    /// <returns></returns>
    public IEnumerable<Vertex3Int> FilterVertices(
        Func<Vertex3Int, bool> selector)
    {
        List<Vertex3Int> output = new();

        foreach (var key in Pathfinding.Keys)
        {
            if (Pathfinding.TryGetVertex(key, out var vertex))
            {
                if (selector(vertex))
                    output.Add(vertex);
            }
        }

        return output;
    }

    /// <summary>
    /// Gets the closest vertex to <paramref name="position"/>, with no
    /// preference to sectionID.
    /// </summary>
    /// <inheritdoc cref="GetClosestNeighbor(Vector3Int, Guid)"/>
    public Vertex3Int GetClosestNeighbor(Vector3Int position)
    {
        return GetClosestNeighbor(position, Guid.Empty);
    }

    /// <summary>
    /// Gets the closest vertex to <paramref name="position"/> within the same
    /// section as <paramref name="section"/>.
    /// </summary>
    /// <param name="position">Where to being searching for neighbors.</param>
    /// <param name="section">The section of the graph to search in.</param>
    /// <returns>A relatively close neighboring vertex, or null if none can be
    /// found.</returns>
    public Vertex3Int GetClosestNeighbor(Vector3Int position, Guid section)
    {
        // Old, better performing code. Use again once it works. However, make
        // sure to keep the existing code.
        #region Old
        // // First see if position contains what we are looking for.
        // if (Pathfinding.TryGetVertex(position, out var vertex) &&
        //     (section == Guid.Empty || section == vertex.sectionID))
        // {
        //     return vertex;
        // }

        // List<Vertex3Int> selected = new();

        // for (int i = 1; i < neighborCheckMaxDistance; i++)
        // {
        //     for (int t = 0; t <= i; t++)
        //     {
        //         // We only need to check up to 8 vertexes here.
        //         foreach (var offset in TFOffsets(t, i))
        //         {
        //             Vector3Int pos = offset + position;

        //             if (Pathfinding.TryGetVertex(pos, out var v) &&
        //                 (section == Guid.Empty || section == v.sectionID))
        //             {
        //                 selected.Add(v);
        //             }
        //         }

        //         if (selected.Count > 0)
        //         {
        //             return selected.RandomSelectOne();
        //         }
        //     }
        // }
        #endregion

        var vertices = Pathfinding.Values.AsEnumerable();

        if (section != Guid.Empty)
            vertices = vertices.Where(v => v.sectionID == section);

        return vertices
            .Aggregate((v1, v2) =>
                v1.id.TaxicabDistance(position) <
                v2.id.TaxicabDistance(position) ?
                v1 : v2
            );
    }

    private IEnumerable<Vector3Int> TFOffsets(int t, int i)
    {
        yield return new(i, t);
        yield return new(t, i);
        yield return new(-i, t);
        yield return new(t, -i);

        if (t != 0 && t != i)
        {
            yield return new(i, -t);
            yield return new(-t, i);
            yield return new(-i, -t);
            yield return new(-t, -i);
        }
    }

    /// <summary>
    /// Returns a collection of vertices that surround a specified position.
    /// </summary>
    /// <param name="vertexPosition"></param>
    /// <returns></returns>
    public IEnumerable<Vertex3Int> GetCardinalNeighbors(
        Vector3Int vertexPosition)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Vector3Int offset = new(x, y);
                Vector3Int pos = vertexPosition + offset;

                if (Pathfinding.TryGetVertex(
                    pos,
                    out var neighbor))
                {
                    yield return neighbor;
                }
            }
        }
    }
    #endregion

    #region Private Helpers
    private bool SomeFlagsSet(
        Vertex3Int vertex,
        NodeFlags flags)
    {
        return vertex.Nodes.Any(n => n.flags.SomeFlagsSet(flags));
    }
    #endregion
    #endregion
}