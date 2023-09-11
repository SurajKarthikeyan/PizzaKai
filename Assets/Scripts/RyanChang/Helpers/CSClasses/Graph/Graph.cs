using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// A weighted, undirected graph data structure using adjacency map.
/// </summary>
/// <typeparam name="T">Any IEquatable type.</typeparam>
[System.Serializable]
public class Graph<T> : ISerializationCallbackReceiver, IEnumerable<Vertex<T>>,
    IEnumerable<GraphEdge<T>>, ITraceable<T>
    where T : IEquatable<T>
{
    #region Enums
    public enum EnumerationMethod
    {
        /// <summary>
        /// https://en.wikipedia.org/wiki/Breadth-first_search
        /// </summary>
        BreathFirst,
        /// <summary>
        /// https://en.wikipedia.org/wiki/Depth-first_search
        /// </summary>
        DepthFirst
    }
    #endregion

    #region Properties
    /// <summary>
    /// Selects the method of enumeration
    /// </summary>
    public EnumerationMethod EnumMethod { get; private set; }

    /// <summary>
    /// The root vertex. This determines where graph enumeration will begin.
    /// </summary>
    public Vertex<T> Root { get; private set; }

    /// <summary>
    /// Dictionary of vertices, <id, vertex>.
    /// </summary>
    public Dictionary<T, Vertex<T>> Vertices { get; set; }

    public ICollection<T> Keys => Vertices.Keys;

    public ICollection<Vertex<T>> Values => Vertices.Values;

    public int Count => Vertices.Count;

    public Vertex<T> this[T key]
    {
        get => Vertices[key];
        set => Vertices[key] = value;
    }
    #endregion

    [SerializeField]
    // [HideInInspector]
    private UnityDictionary<T, Vertex<T>> serializedVertices;


    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public Graph()
    {
        Vertices = new Dictionary<T, Vertex<T>>();
    }

    /// <summary>
    /// Creates a graph.
    /// </summary>
    /// <param name="vertices">Vertices to assign to the graph.</param>
    public Graph(Dictionary<T, Vertex<T>> vertices)
    {
        this.Vertices = vertices;
        Root = vertices.Values.First();
    }

    /// <summary>
    /// Constructs a new graph from a path. See AStarSearch.
    /// </summary>
    /// <param name="path">A dictionary, where the second vertex is what precedes the first vertex.</param>
    public Graph(Dictionary<Vertex<T>, Vertex<T>> path)
    {
        Vertices = new Dictionary<T, Vertex<T>>();
        foreach (var first in path.Keys)
        {
            Add(path[first].id, first.id);
        }
    }
    #endregion

    #region Methods
    #region Addition of Vertices
    /// <summary>
    /// Adds a new vertex directly, without any connections.
    /// </summary>
    /// <param name="newVertex">The new vertex to add.</param>
    public void Add(Vertex<T> newVertex)
    {
        Vertices[newVertex.id] = newVertex;

        if (Root == null)
        {
            Root = newVertex;
        }
    }

    /// <summary>
    /// Adds an edge to the graph between <paramref name="from"/> and
    /// <paramref name="to"/>.
    /// </summary>
    /// <param name="from">The starting vertex.</param>
    /// <param name="to">The ending vertex.</param>
    /// <param name="weight">Weight of edge.</param>
    public void Add(Vertex<T> from, Vertex<T> to, float weight = 0)
    {
        T fromID = from.id;
        T toID = to.id;

        if (!Vertices.ContainsKey(fromID))
        {
            Add(from);
        }

        if (!Vertices.ContainsKey(toID))
        {
            Add(to);
        }

        from.Adjacent[toID] = weight;
    }

    /// <summary>
    /// Creates a single vertex with no connections and no heuristic.
    /// </summary>
    /// <inheritdoc cref="Add(T, Vertex{T}.Heuristic)"/>
    public Vertex<T> Add(T newId)
    {
        var newV = new Vertex<T>(newId);
        Add(newV);
        return newV;
    }

    /// <summary>
    /// Creates a single vertex with no connections.
    /// </summary>
    /// <param name="newId">The new id to add.</param>
    /// <param name="heuristic">Heuristic to use for A*.</param>
    /// <returns>The created vertex.</returns>
    public Vertex<T> Add(T newId, float heuristic)
    {
        var newV = new Vertex<T>(newId, heuristic);
        Add(newV);
        return newV;
    }

    /// <summary>
    /// Adds an edge to the graph between fromId and toId. Inserts the vertices
    /// as needed.
    /// </summary>
    /// <param name="fromId">The starting vertex.</param>
    /// <param name="toId">The ending vertex.</param>
    /// <param name="weight">Weight of edge.</param>
    /// <returns>A tuple containing the created vertices in the order of
    /// (<paramref name="fromId"/>, <paramref name="toId"/>).</param>
    public Tuple<Vertex<T>, Vertex<T>> Add(T fromId, T toId, float weight = 0)
    {
        Vertex<T> fromV, toV;

        if (!Vertices.TryGetValue(fromId, out fromV))
        {
            fromV = Add(fromId);
        }
        if (!Vertices.TryGetValue(toId, out toV))
        {
            toV = Add(toId);
        }

        Vertices[fromId].Adjacent[toId] = weight;

        return new(fromV, toV);
    }

    /// <param name="fromHeuristic">Heuristic for <paramref
    /// name="fromId"/>.</param>
    /// <param name="toHeuristic">Heuristic for <paramref name="toId"/>.</param>
    /// <inheritdoc cref="Add(T, T, float)"/>
    public Tuple<Vertex<T>, Vertex<T>> Add(T fromId, T toId, float weight,
            float fromHeuristic, float toHeuristic)
    {
        Vertex<T> fromV, toV;

        if (!Vertices.TryGetValue(fromId, out fromV))
        {
            fromV = Add(fromId);
        }
        if (!Vertices.TryGetValue(toId, out toV))
        {
            toV = Add(toId);
        }

        Vertices[fromId].Adjacent[toId] = weight;

        return new(fromV, toV);
    }
    #endregion

    #region Visiting
    /// <summary>
    /// Resets the visited boolean for each vertex in the graph.
    /// </summary>
    /// <param name="id">The visit id for the vertex.</param>
    public void ResetVisitedVertices(Guid id)
    {
        foreach (var vertex in Vertices)
        {
            vertex.Value.ResetVisited(id);
        }
    }

    public void ResetAggregateCostVertices(Guid id)
    {
        foreach (var vertex in Vertices)
        {
            vertex.Value.ResetAggregateCost(id);
        }
    }
    #endregion

    #region Getters/Setters
    /// <summary>
    /// Checks if this graph has a vertex at <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The coordinates of the supposed vertex.</param>
    /// <returns>True if there is a vertex there, else false.</returns>
    public bool HasVertex(T key)
    {
        return Vertices.ContainsKey(key);
    }

    /// <summary>
    /// Gets a vertex by its key.
    /// </summary>
    /// <param name="key">key of the vertex.</param>
    /// <returns>A vertex if one exists at the key specified, else null.</returns>
    public Vertex<T> GetVertex(T key)
    {
        return Vertices.TryGetValue(key, out var vertex) ? vertex : null;
    }

    /// <summary>
    /// Attempts to get the vertex by its key.
    /// </summary>
    /// <param name="key">key of the vertex.</param>
    /// <param name="vertex">The vertex, if one exists.</param>
    /// <returns>True if value was found, false otherwise.</returns>
    public bool TryGetVertex(T key, out Vertex<T> vertex)
    {
        return Vertices.TryGetValue(key, out vertex);
    }

    /// <summary>
    /// Attempts to get the vertex by its key, then attempts to cast it into the
    /// specified vertex type.
    /// </summary>
    /// <inheritdoc cref="TryGetVertex(T, out Vertex{T})"/>
    public bool TryGetVertex<TVertex>(T key, out TVertex vertex)
        where TVertex : Vertex<T>
    {
        vertex = GetVertex(key) as TVertex;
        return vertex != null;
    }

    /// <summary>
    /// Determines if an edge exists between fromID and toID.
    /// </summary>
    /// <param name="fromID">ID to come from.</param>
    /// <param name="toID">ID to go to.</param>
    /// <returns>True if there exists an edge between fromID and toID, else false.</returns>
    public bool HasEdge(T fromID, T toID)
    {
        if (HasVertex(fromID) && HasVertex(toID))
        {
            var fromV = Vertices[fromID];
            if (fromV.Adjacent.ContainsKey(toID))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the edge weight from <paramref name="fromID"/> to <paramref
    /// name="toID"/>.
    /// </summary>
    /// <param name="fromID">Coordinates of the beginning vertex.</param>
    /// <param name="toID">Coordinates of the end vertex.</param>
    /// <returns>A tuple <from vertex, to vertex, edge weight> if such an edge
    /// exists, else null.</returns>
    public GraphEdge<T> GetEdge(T fromID, T toID)
    {
        if (HasVertex(fromID) && HasVertex(toID))
        {
            var fromV = Vertices[fromID];
            var toV = Vertices[toID];
            if (fromV.Adjacent.ContainsKey(toID))
            {
                return new(fromV, toV, fromV.Adjacent[toID]);
            }
        }

        return new(null, null, float.NaN);
    }

    /// <summary>
    /// Gets the edge weight from fromID to toID.
    /// </summary>
    /// <param name="from">The beginning vertex.</param>
    /// <param name="to">The end vertex.</param>
    /// <returns>A tuple <from vertex, to vertex, edge weight> if such an edge
    /// exists, else null.</returns>
    public GraphEdge<T> GetEdge(Vertex<T> from, Vertex<T> to)
    {
        if (HasVertex(from.id) && HasVertex(to.id))
        {
            if (from.Adjacent.ContainsKey(to.id))
            {
                return new(from, to, from.Adjacent[to.id]);
            }
        }

        return new(null, null, float.NaN);
    }

    /// <summary>
    /// Sets the new enumeration method and root.
    /// </summary>
    /// <param name="enumerationMethod">New method of enumeration.</param>
    /// <param name="root">New root node.</param>
    /// <returns>A tuple containing the original enumeration method and
    /// root.</returns>
    public Tuple<EnumerationMethod, Vertex<T>> SetEnumeration(
        EnumerationMethod enumerationMethod, Vertex<T> root)
    {
        Tuple<EnumerationMethod, Vertex<T>> og = new(this.EnumMethod, this.Root);

        this.EnumMethod = enumerationMethod;
        this.Root = root;

        return og;
    }

    /// <inheritdoc cref="SetEnumeration(EnumerationMethod, Vertex{T})"/>
    private void SetEnumeration(Tuple<EnumerationMethod, Vertex<T>> method)
    {
        SetEnumeration(method.Item1, method.Item2);
    }

    /// <summary>
    /// Using the <paramref name="enumerationMethod"/> and <paramref name="root"/> vertex,
    /// do an iteration of the graph.
    /// </summary>
    /// <returns>An IEnumerator over a <see cref="Vertex{T}"/>.</returns>
    public IEnumerator<Vertex<T>> GetEnumerator()
    {
        return EnumMethod switch
        {
            EnumerationMethod.BreathFirst => BFS_Vertex(Root).GetEnumerator(),
            _ => DFS_Vertex(Root).GetEnumerator()
        };
    }

    /// <returns>An IEnumerator over a <see cref="GraphEdge{T}"/>.</returns>
    /// <inheritdoc cref="GetEnumerator"/>
    IEnumerator<GraphEdge<T>> IEnumerable<GraphEdge<T>>.GetEnumerator()
    {
        return EnumMethod switch
        {
            EnumerationMethod.BreathFirst => BFS_Edge(Root).GetEnumerator(),
            _ => DFS_Edge(Root).GetEnumerator()
        };
    }

    /// <summary>
    /// Using the <paramref name="enumerationMethod"/> and <paramref name="root"/> vertex,
    /// do an iteration of the graph.
    /// </summary>
    /// <returns>An IEnumerator that is a vertex.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion

    #region Traversal
    #region DFS
    /// <summary>
    /// Breath first traversal. See
    /// https://en.wikipedia.org/wiki/Breadth-first_search.
    /// </summary>
    /// <param name="start">Where to start traversal.</param>
    /// <returns>An IEnumerator over <see cref="Vertex{T}"/>.</returns>
    private IEnumerable<Vertex<T>> BFS_Vertex(Vertex<T> start,
        bool includeAll = false)
    {
        Guid g = Guid.NewGuid();
        // Breadth first traversal
        Queue<Vertex<T>> q = new();
        q.Enqueue(start);

        while (q.Count > 0)
        {
            Vertex<T> currV = q.Dequeue();

            if (currV.GetVisited(g))
            {
                if (includeAll && q.Count <= 0)
                {
                    // If empty, search for an unvisited vertex and add that to
                    // the collection.
                    var unvisited = Values.FirstOrDefault(v => !v.GetVisited(g));

                    if (unvisited != null)
                        q.Enqueue(unvisited);
                }

                continue;
            }

            currV.SetVisited(g, true);

            yield return currV;

            foreach (var adjP in currV.Adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                q.Enqueue(adjV);
            }
        }

        ResetVisitedVertices(g);
    }

    /// <returns>An IEnumerator over <see cref="GraphEdge{T}"/>.</returns>
    /// <inheritdoc cref="BFS_Vertex(Vertex{T}, bool)"/>
    private IEnumerable<GraphEdge<T>> BFS_Edge(Vertex<T> start,
        bool includeAll = false)
    {
        Guid g = Guid.NewGuid();
        // Breadth first traversal
        Queue<Vertex<T>> q = new();
        q.Enqueue(start);

        while (q.Count > 0 || includeAll)
        {
            if (q.Count <= 0)
            {
                // Empty. Try to find another vertex.
                if (includeAll)
                {
                    // If empty, search for an unvisited vertex and add that to
                    // the collection.
                    var unvisited = Values.FirstOrDefault(v => !v.GetVisited(g));

                    if (unvisited != null)
                    {
                        q.Enqueue(unvisited);
                    }
                    else
                    {
                        // Cannot find another vertex.
                        break;
                    }
                }
                else
                {
                    throw new Exception("Should not be thrown");
                }
            }

            Vertex<T> currV = q.Dequeue();

            if (currV.GetVisited(g))
            {
                continue;
            }

            currV.SetVisited(g, true);

            foreach (var adjP in currV.Adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                yield return new(currV, adjV, adjP.Value);

                q.Enqueue(adjV);
            }
        }

        ResetVisitedVertices(g);
    }
    #endregion

    #region BFS
    /// <summary>
    /// Depth first traversal. See
    /// https://en.wikipedia.org/wiki/Depth-first_search.
    /// </summary>
    /// <inheritdoc cref="BFS_Vertex(Vertex{T})"/>
    public IEnumerable<Vertex<T>> DFS_Vertex(Vertex<T> start,
        bool includeAll = false)
    {
        Guid g = Guid.NewGuid();
        // Depth first traversal
        Stack<Vertex<T>> s = new Stack<Vertex<T>>();
        s.Push(start);

        while (s.Count > 0)
        {
            Vertex<T> currV = s.Pop();

            if (currV.GetVisited(g))
            {
                if (includeAll && s.Count <= 0)
                {
                    // If empty, search for an unvisited vertex and add that to
                    // the collection.
                    var unvisited = Values.FirstOrDefault(v => !v.GetVisited(g));

                    if (unvisited != null)
                        s.Push(unvisited);
                }

                continue;
            }

            currV.SetVisited(g, true);

            yield return currV;

            foreach (var adjP in currV.Adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                s.Push(adjV);
            }
        }

        ResetVisitedVertices(g);
    }

    /// <summary>
    /// Depth first traversal. See
    /// https://en.wikipedia.org/wiki/Depth-first_search.
    /// </summary>
    /// <inheritdoc cref="BFS_Edge(Vertex{T})"/>
    public IEnumerable<GraphEdge<T>> DFS_Edge(Vertex<T> start, bool includeAll = false)
    {
        Guid g = Guid.NewGuid();
        // Depth first traversal
        Stack<Vertex<T>> s = new Stack<Vertex<T>>();
        s.Push(start);

        while (s.Count > 0)
        {
            Vertex<T> currV = s.Pop();

            if (currV.GetVisited(g))
            {
                if (includeAll && s.Count <= 0)
                {
                    // If empty, search for an unvisited vertex and add that to
                    // the collection.
                    var unvisited = Values.FirstOrDefault(v => !v.GetVisited(g));

                    if (unvisited != null)
                        s.Push(unvisited);
                }

                continue;
            }

            currV.SetVisited(g, true);

            foreach (var adjP in currV.Adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                yield return new(currV, adjV, adjP.Value);

                s.Push(adjV);
            }
        }

        ResetVisitedVertices(g);
    }
    #endregion

    // Methods to call when we are finished added vertices.
    #region Graph Finalization
    /// <summary>
    /// Removes all vertices without any outgoing paths.
    /// </summary>
    public void TrimVertices()
    {
        var toRemove = Vertices
            .Where(kvp => kvp.Value.Degree <= 0)
            .Select(kvp => kvp.Key)
            .ToList();

        bool reassignRoot = false;

        foreach (var rm in toRemove)
        {
            if (rm.Equals(Root.id))
                reassignRoot = true;

            Vertices.Remove(rm);
        }

        if (reassignRoot)
            Root = Values.FirstOrDefault();
    }

    /// <summary>
    /// Removes all edges of weight zero where the start and end vertices are
    /// the same.
    /// </summary>
    public void RemoveSelfPaths()
    {
        foreach (var vertex in Values)
        {
            var toRemove = vertex.Adjacent
                .Where(a => a.Key.Equals(vertex.id) && a.Value == 0)
                .ToList();      // Important! Needs a copy.

            foreach (var rm in toRemove)
            {
                vertex.Adjacent.TryRemove(rm.Key, out _);
            }
        }
    }

    /// <summary>
    /// Label each vertex with their section identifier (which are randomly
    /// generated).
    /// </summary>
    /// <param name="trim">If true, remove all but the largest section.</param>
    /// <remarks>
    /// A section is defined as the exhaustive set of vertices of which all are
    /// connected. All vertices that are connected are of the same section.
    /// </remarks>
    public void DetectSections(bool trim)
    {
        Guid visitID = Guid.NewGuid();

        Vertex<T> unvisited = Root;

        Dictionary<Guid, List<T>> sectionIDs = new();

        while (unvisited != default)
        {
            Guid sectionID = Guid.NewGuid();
            sectionIDs[sectionID] = new();

            foreach (var vertex in BFS_Vertex(unvisited))
            {
                vertex.SetVisited(visitID, true);
                vertex.sectionID = sectionID;
                sectionIDs[sectionID].Add(vertex.id);
            }

            unvisited = Values.FirstOrDefault(v => !v.GetVisited(visitID));
        }

        if (trim)
        {
            Guid largest = sectionIDs
                .Aggregate((s1, s2) =>
                    s1.Value.Count > s2.Value.Count ?
                    s1 : s2)
                .Key;

            foreach (var sectionKVP in sectionIDs)
            {
                if (sectionKVP.Key != largest)
                {
                    foreach (var tea in sectionKVP.Value)
                    {
                        Vertices.Remove(tea);
                    }
                }
            }
        }
    }
    #endregion

    #region Graph Validation
    /// <summary>
    /// Returns true if a path exists between <paramref name="start"/> and
    /// <paramref name="end"/>.
    /// </summary>
    /// <param name="start">Starting vertex.</param>
    /// <param name="end">Ending vertex.</param>
    /// <returns>True if a path exists, false otherwise.</returns>
    public bool VerticesConnected(Vertex<T> start, Vertex<T> end)
    {
        var old = SetEnumeration(EnumerationMethod.BreathFirst, start);

        List<Vertex<T>> visited = new();

        foreach (var current in this)
        {
            visited.Add(current);
            if (current == end)
            {
                SetEnumeration(old);
                return true;
            }
        }

        SetEnumeration(old);
        return false;
    }

    public bool PathExists(T start, T end)
    {
        if (HasVertex(start) && HasVertex(end))
        {
            return VerticesConnected(Vertices[start], Vertices[end]);
        }

        return false;
    }
    #endregion

    #region Vertex
    #region Vertex Affordability
    /// <summary>
    /// Get a list of all vertices that are affordable to traverse to with a
    /// fixed cost maxCost.
    /// </summary>
    /// <param name="root">Where to start the iteration.</param>
    /// <param name="maxCost">Maximum cost of traversal.</param>
    /// <returns></returns>
    public IEnumerable<Vertex<T>> AffordableVertices(Vertex<T> root, float maxCost)
    {
        Guid g = Guid.NewGuid();
        // Breadth first traversal
        List<Vertex<T>> elements = new();
        Queue<Vertex<T>> q = new();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            Vertex<T> currV = q.Dequeue();

            // if (currV.GetVisited(g)) continue;
            // currV.SetVisited(g, true);

            if (currV.GetAggregateCost(g) > maxCost) continue;
            elements.Add(currV);

            foreach (var adjP in currV.Adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                if (!adjV.GetVisited(g))
                {
                    float adjCost = adjV.heuristic + currV.GetAggregateCost(g);
                    adjV.SetAggregateCost(g, adjCost);
                    adjV.SetVisited(g, true);
                    q.Enqueue(adjV);
                }
            }
        }

        ResetVisitedVertices(g);
        return elements;
    }

    /// <summary>
    /// Get a list of all Ts that are affordable to traverse to with a
    /// fixed cost maxCost.
    /// </summary>
    /// <param name="root">Where to start the iteration.</param>
    /// <param name="maxCost">Maximum cost of traversal.</param>
    /// <returns></returns>
    public IEnumerable<T> AffordableVertices(T root, float maxCost)
    {
        return AffordableVertices(GetVertex(root), maxCost).
            Select(t => t.id);
    }
    #endregion
    #endregion

    #region AStar
    /// <summary>
    /// Performs an A* search of the graph. Assumes graph is fully connected.
    /// </summary>
    /// <param name="startID">What coordinate to start at?</param>
    /// <param name="endID">What coordinate to end at?</param>
    /// <param name="cost">The cost of the traversal.</param>
    /// <returns>The path from startID to endID.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref
    /// name="endID"/> or <paramref name="startID"/> cannot be found within the
    /// graph.</exception>
    /// <exception cref="CannotFindPathException">If path cannot be
    /// generated.</exception>
    public Path<T> AStarSearch(T startID, T endID,
        out float cost)
    {
        if (!HasVertex(startID))
            throw new VertexNotInGraphException(startID, this);
        else if (!HasVertex(endID))
            throw new VertexNotInGraphException(endID, this);
        else if (Count < 2)
            throw new PathfindingException("Cannot make graph with less than 2 vertices");
        else if (startID.Equals(endID))
            throw new PathfindingException("Start and end vertices cannot " +
                $"be the same value ({startID})");

        var endV = Vertices[endID];
        var startV = Vertices[startID];

        if (endV.sectionID != startV.sectionID)
        {
            throw new DisjointGraphException(
                "Graph is disjoint. Trying to navigate from section " +
                $"{startV.sectionID} to section {endV.sectionID}."
            );
        }
        
        PriorityQueue<Vertex<T>> unvisited = new();

        foreach (var v in Values)
        {
            float priority = v.Equals(startV) ? 0 : float.PositiveInfinity;
            unvisited.Enqueue(priority, v);
        }

        // These are the shortest paths from startV. The keys are the vertex IDs
        // can can be reached from startV, and the values are the cost it takes
        // to get there.
        Dictionary<T, float> totalCosts = new()
        {
            [startID] = 0
        };
        cost = 0;

        // The backwards path, with keys starting at endV and values "pointing"
        // at startV. We can navigate through the path by starting at endV, then
        // repeatably select the next dictionary entry by using the value from
        // the previous KeyValuePair.
        Dictionary<Vertex<T>, Vertex<T>> backwardsPath = new();

        // Set to true if we get through the entire graph (that is, we end
        // iteration on endV).
        bool pathComplete = false;

        // GUID for visitation.
        Guid visitID = Guid.NewGuid();

        while (unvisited.Count > 0)
        {
            var currentPQE = unvisited.DequeueElement();
            // Current vertex.
            var currentV = currentPQE.value;
            // Current ID of vertex.
            var currentID = currentV.id;

            if (!float.IsFinite(currentPQE.priority))
                throw new DisjointGraphException("Graph disjoint detected.");

            // Current cost of traversal, from startV to currentV.
            float currentCostTotal = totalCosts[currentID];

            try
            {
                currentCostTotal = totalCosts[currentID];
            }
            catch (KeyNotFoundException e)
            {
                // This should not be hit under any circumstances. If the graph
                // is disjoint, it should be caught earlier. If it does get hit,
                // try rebuilding the graph.
                throw new PathfindingException(
                    $"Cannot find cost for {currentID}.",
                    e
                );
                // continue;
            }

            currentV.SetVisited(visitID, true);

            if (currentID.Equals(endID))
            {
                // Found end. Break to go to return.
                pathComplete = true;
                break;
            }

            foreach (var adjKVP in currentV.Adjacent)
            {
                // Adjacent ID of vertex.
                T adjID = adjKVP.Key;
                // Adjacent vertex.
                Vertex<T> adjV = Vertices[adjID];
                // Cost required to move from currentV to adjV.
                float adjCostSingle = adjKVP.Value + adjV.heuristic;


                // Try to calculate a adjacent cost, from startV to adjV.
                // ogAdjCostTotal is the original cost from startV to adjV, as
                // specified in totalCosts. newAdjS is the new cost, calculated
                // by adding [the cost from moving from startV to currentV] and
                // [the cost from moving from currentV to adjV].
                float ogAdjCostTotal = totalCosts.GetValueOrDefault(adjID,
                    float.PositiveInfinity);
                float newAdjCostTotal = currentCostTotal + adjCostSingle;

                if (newAdjCostTotal < ogAdjCostTotal)
                {
                    // Found a better path.
                    totalCosts[adjID] = newAdjCostTotal;
                    unvisited.Update(newAdjCostTotal, adjV);
                    cost = Mathf.Max(cost, totalCosts[adjID]);
                    backwardsPath[adjV] = currentV;
                }
            }
        }

        if (!pathComplete)
            Debug.LogWarning("Path has been generated, but it is not complete.");

        ResetVisitedVertices(visitID);
        return new(startV, endV, backwardsPath, totalCosts, this);
    }

    /// <inheritdoc cref="AStarSearch(T, T, out float)"/>
    public Path<T> AStarSearch(T startID, T endID)
    {
        return AStarSearch(startID, endID, out _);
    }
    #endregion
    #endregion
    #endregion

    #region ITraceable Implementation
    public IEnumerator<GraphEdge<T>> GetTraces() => BFS_Edge(Root, true).GetEnumerator();
    #endregion

    #region ISerializationCallbackReceiver Implementation
    public void OnBeforeSerialize()
    {
        serializedVertices = new(Vertices);
    }

    public void OnAfterDeserialize()
    {
        Vertices = new(serializedVertices);
    }
    #endregion
}