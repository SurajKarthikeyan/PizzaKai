using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// A weighted, undirected graph data structure using adjacency map.
/// </summary>
/// <typeparam name="T">Any IEquatable type.</typeparam>
public class Graph<T> : IEnumerable<Vertex<T>> where T : IEquatable<T>
{
    /* Variables */
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

    /// <summary>
    /// The number of elements in this graph.
    /// </summary>
    public int Count => Vertices.Count;

    /* Constructors */
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

    /* Methods */
    /// <summary>
    /// Creates a single vertex with no connections.
    /// </summary>
    /// <param name="newId">The new id to add.</param>
    public void Add(T newId)
    {
        Vertices[newId] = new Vertex<T>(newId);

        if (Root == null)
        {
            Root = Vertices[newId];
        }
    }

    /// <summary>
    /// Creates a single vertex with no connections.
    /// </summary>
    /// <param name="newId">The new id to add.</param>
    /// <param name="heuristic">Heuristic to use for A*.</param>
    public void Add(T newId, Vertex<T>.Heuristic heuristic)
    {
        Vertices[newId] = new Vertex<T>(newId, heuristic);

        if (Root == null)
        {
            Root = Vertices[newId];
        }
    }

    /// <summary>
    /// Adds an edge to the graph between fromId and toId. Inserts the vertices as needed.
    /// </summary>
    /// <param name="fromId">The starting vertex.</param>
    /// <param name="toId">The ending vertex.</param>
    /// <param name="weight">Weight of edge.</param>
    public void Add(T fromId, T toId, float weight)
    {
        if (!Vertices.ContainsKey(fromId))
        {
            Add(fromId);
        }

        if (!Vertices.ContainsKey(toId))
        {
            Add(toId);
        }

        Vertices[fromId].adjacent[toId] = weight;
    }

    /// <summary>
    /// Adds an edge with a weight of 0 to the graph between fromId and toId. Inserts the vertices as needed.
    /// </summary>
    /// <param name="fromId">The starting vertex.</param>
    /// <param name="toId">The ending vertex.</param>
    public void Add(T fromId, T toId)
    {
        Add(fromId, toId, 0);
    }

    /// <summary>
    /// Adds an edge with a weight of 0 to the graph between fromId and toId. Inserts the vertices as needed.
    /// </summary>
    /// <param name="fromId">The starting vertex.</param>
    /// <param name="toId">The ending vertex.</param>
    /// <param name="weight">Weight of edge.</param>
    /// <param name="fromHeuristic">Heuristic for fromId</param>
    /// <param name="toHeuristic">Heuristic for toId</param>
    public void Add(T fromId, T toId, float weight,
        Vertex<T>.Heuristic fromHeuristic, Vertex<T>.Heuristic toHeuristic)
    {
        if (!Vertices.ContainsKey(fromId))
        {
            Add(fromId, fromHeuristic);
        }

        if (!Vertices.ContainsKey(toId))
        {
            Add(toId, toHeuristic);
        }

        Vertices[fromId].adjacent[toId] = weight;
    }

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

    /// <summary>
    /// Checks if this graph has a vertex at <paramref name="coord"/>.
    /// </summary>
    /// <param name="coord">The coordinates of the supposed vertex.</param>
    /// <returns>True if there is a vertex there, else false.</returns>
    public bool HasVertex(T coord)
    {
        return Vertices.ContainsKey(coord);
    }

    /// <summary>
    /// Gets a vertex by it's key.
    /// </summary>
    /// <param name="key">key of the vertex.</param>
    /// <returns>A vertex if one exists at the key specified, else null.</returns>
    public Vertex<T> VertexByID(T key)
    {
        if (Vertices.ContainsKey(key))
            return Vertices[key];
        else
            return null;
    }

    /// <summary>
    /// Gets a vertex by it's key.
    /// </summary>
    /// <param name="key">key of the vertex.</param>
    /// <returns>A vertex if one exists at the key specified, else null.</returns>
    public Vertex<T> GetVertex(T key)
    {
        return VertexByID(key);
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
            if (fromV.adjacent.ContainsKey(toID))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the edge weight from fromID to toID.
    /// </summary>
    /// <param name="fromID">Coordinates of the beginning vertex.</param>
    /// <param name="toID">Coordinates of the end vertex.</param>
    /// <returns>A tuple <from vertex, to vertex, edge weight> if such an edge exists, else null.</returns>
    public Tuple<Vertex<T>, Vertex<T>, float> GetEdge(T fromID, T toID)
    {
        if (HasVertex(fromID) && HasVertex(toID))
        {
            var fromV = Vertices[fromID];
            var toV = Vertices[toID];
            if (fromV.adjacent.ContainsKey(toID))
            {
                return new Tuple<Vertex<T>, Vertex<T>, float>(fromV, toV, fromV.adjacent[toID]);
            }
        }

        return null;
    }

    /// <summary>
    /// Sets the new enumeration method and root.
    /// </summary>
    /// <param name="enumerationMethod">New method of enumeration.</param>
    /// <param name="root">New root node.</param>
    /// <returns>A tuple containing the original enumeration method and root.</returns>
    public Tuple<EnumerationMethod, Vertex<T>> SetEnumeration(EnumerationMethod enumerationMethod, Vertex<T> root)
    {
        Tuple<EnumerationMethod, Vertex<T>> og = new(this.EnumMethod, this.Root);

        this.EnumMethod = enumerationMethod;
        this.Root = root;

        return og;
    }

    /// <summary>
    /// Using the <paramref name="enumerationMethod"/> and <paramref name="root"/> vertex,
    /// do an iteration of the graph.
    /// </summary>
    /// <returns>An IEnumerator that is a vertex.</returns>
    public IEnumerator<Vertex<T>> GetEnumerator()
    {
        if (EnumMethod == EnumerationMethod.BreathFirst)
            return BFS();
        else
            return DFS();
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

    /// <summary>
    /// Breath first traversal, using root. See https://en.wikipedia.org/wiki/Breadth-first_search.
    /// </summary>
    /// <returns>An IEnumerator that is a vertex.</returns>
    private IEnumerator<Vertex<T>> BFS()
    {
        Guid g = Guid.NewGuid();
        // Breadth first traversal
        Queue<Vertex<T>> q = new Queue<Vertex<T>>();
        q.Enqueue(Root);

        while (q.Count > 0)
        {
            Vertex<T> currV = q.Dequeue();

            if (currV.GetVisited(g)) continue;
            currV.SetVisited(g, true);

            yield return currV;

            foreach (var adjP in currV.adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                q.Enqueue(adjV);
            }
        }

        ResetVisitedVertices(g);
    }

    /// <summary>
    /// Depth first traversal, using root. See https://en.wikipedia.org/wiki/Depth-first_search.
    /// </summary>
    /// <returns>An IEnumerator that is a vertex.</returns>
    public IEnumerator<Vertex<T>> DFS()
    {
        Guid g = Guid.NewGuid();
        // Depth first traversal
        Stack<Vertex<T>> s = new Stack<Vertex<T>>();
        s.Push(Root);

        while (s.Count > 0)
        {
            Vertex<T> currV = s.Pop();

            if (currV.GetVisited(g)) continue;
            currV.SetVisited(g, true);

            yield return currV;

            foreach (var adjP in currV.adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                s.Push(adjV);
            }
        }

        ResetVisitedVertices(g);
    }

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
        Queue<Vertex<T>> q = new Queue<Vertex<T>>();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            Vertex<T> currV = q.Dequeue();

            // if (currV.GetVisited(g)) continue;
            // currV.SetVisited(g, true);
            
            if (currV.GetAggregateCost(g) > maxCost) continue;
            elements.Add(currV);

            foreach (var adjP in currV.adjacent)
            {
                Vertex<T> adjV = Vertices[adjP.Key];

                if (!adjV.GetVisited(g))
                {
                    float adjCost = adjV.heuristic() + currV.GetAggregateCost(g);
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

    /// <summary>
    /// Performs an A* search of the graph. Assumes graph is fully connected.
    /// </summary>
    /// <param name="startID">What coordinate to start at?</param>
    /// <param name="endID">What coordinate to end at?</param>
    /// <returns>The path from startID to endID.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If startID and/or endID don't exists within the graph.</exception>
    public Path<T> AStarSearch(T startID, T endID)
    {
        if (!HasVertex(startID))
            throw new ArgumentOutOfRangeException(nameof(startID), startID, "StartID is out of range!");
        else if (!HasVertex(endID))
            throw new ArgumentOutOfRangeException(nameof(endID), endID, "EndID is out of range!");
        else if (Count < 2)
            return default;

        PriorityQueue<Vertex<T>> unvisited = new PriorityQueue<Vertex<T>>();
        Dictionary<Vertex<T>, Vertex<T>> path = new Dictionary<Vertex<T>, Vertex<T>>();

        foreach (var vPair in Vertices)
        {
            if ((IEquatable<T>)vPair.Key == (IEquatable<T>)startID)
            {
                unvisited.Enqueue(0, vPair.Value);
            }
            else
            {
                unvisited.Enqueue(float.PositiveInfinity, vPair.Value);
            }
        }

        // G scores are the shortest paths from startV.
        Dictionary<T, float> gScores = new Dictionary<T, float>();
        gScores[startID] = 0;

        var endV = Vertices[endID];
        var startV = Vertices[startID];

        while (unvisited.Count > 0)
        {
            var currentV = unvisited.Dequeue();
            var currentID = currentV.id;

            if ((IEquatable<T>)currentID == (IEquatable<T>)endID)
            {
                // Found end
                return new(path, startV, endV, this);
            }
            
            foreach (var adjP in currentV.adjacent)
            {
                // Edge tuple
                var edge = GetEdge(currentID, adjP.Key);

                float newScore = gScores[currentID] + edge.Item3;
                float currScore = (gScores.ContainsKey(adjP.Key)) ? gScores[adjP.Key] : float.PositiveInfinity;
                if (newScore < currScore)
                {
                    // Better path
                    Vertex<T> adjV = Vertices[adjP.Key];
                    gScores[adjP.Key] = newScore;
                    unvisited.Update(newScore + adjV.heuristic(), adjV);
                    path[adjV] = currentV;
                }
            }
        }

        return new(path, startV, endV, this);
    }

    /// <summary>
    /// Performs an A* search of the graph. Assumes graph is fully connected.
    /// </summary>
    /// <param name="startID">What coordinate to start at?</param>
    /// <param name="endID">What coordinate to end at?</param>
    /// <param name="cost">The cost of the traversal.</param>
    /// <returns>A mapping of <Vertex, Vertex>, where the second vertex is
    /// the vertex that precedes the first vertex, or empty if a path does not exist.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If startID and/or endID don't exists within the graph.</exception>
    public Path<T> AStarSearch(T startID, T endID,
        out float cost)
    {
        return AStarSearch(startID, endID, out cost, out _);
    }

    /// <summary>
    /// Performs an A* search of the graph. Assumes graph is fully connected.
    /// </summary>
    /// <param name="startID">What coordinate to start at?</param>
    /// <param name="endID">What coordinate to end at?</param>
    /// <param name="cost">The cost of the traversal.</param>
    /// <param name="distance">The path length.</param>
    /// <returns>A mapping of <Vertex, Vertex>, where the second vertex is
    /// the vertex that precedes the first vertex, or empty if a path does not exist.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If startID and/or endID don't exists within the graph.</exception>
    public Path<T> AStarSearch(T startID, T endID,
        out float cost, out int distance)
    {
        var path = AStarSearch(startID, endID);
        cost = 0;
        distance = 0;

        Vertex<T> pathTraverse = GetVertex(endID);
        Vertex<T> startV = GetVertex(startID);
        do
        {
            cost += pathTraverse.heuristic();
            distance++;
            pathTraverse = path.Next(pathTraverse);

        } while (pathTraverse != startV);

        return path;
    }
}