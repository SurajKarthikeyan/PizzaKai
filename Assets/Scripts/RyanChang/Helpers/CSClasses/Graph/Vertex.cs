using System;
using System.Collections.Generic;

/// <summary>
/// Vertex used in graph. Weights are stored in the graph.
/// </summary>
/// <typeparam name="T">Any type. For use in graph, ensure it is IEquatable.</typeparam>
public class Vertex<T>
{
    /// <summary>
    /// This is unique for each vertex
    /// and serves as the id. 
    /// </summary>
    public T id;

    /// <summary>
    /// Dictionary of outgoing edges. <id, graph weight>.
    /// </summary>
    public Dictionary<T, float> adjacent = new Dictionary<T, float>();

    /// <summary>
    /// Used in the search algorithms. Key is the ID of the thing using the
    /// vertex, bool is if it has been visited or not.
    /// </summary>
    private Dictionary<Guid, bool> visited = new();

    /// <summary>
    /// Used in search algorithms. Key is the ID of the thing using the vertex.
    /// Value is the total cost of the iteration to get to this point.
    /// </summary>
    private Dictionary<Guid, float> aggregateCosts = new();

    /// <summary>
    /// Dictionary of tags this vertex has.
    /// </summary>
    public HashSet<string> tags = new HashSet<string>();

    /// <summary>
    /// Degree of this vertex (number of outgoing edges).
    /// </summary>
    public int Degree => adjacent.Count;

    /// <summary>
    /// Default heuristic (none) for a vertex.
    /// </summary>
    /// <returns>0</returns>
    private float NoHeuristic() { return 0; }

    public delegate float Heuristic();

    /// <summary>
    /// Heuristic for A* search. Smaller values means A* will most likely
    /// take that path.
    /// </summary>
    public Heuristic heuristic;

    /* Constructors */
    /// <summary>
    /// Creates a vertex.
    /// </summary>
    /// <param name="id">ID of this vertex.</param>
    public Vertex(T id)
    {
        this.id = id;
        heuristic = NoHeuristic;
    }

    /// <summary>
    /// Creates a vertex with a custom heuristic.
    /// </summary>
    /// <param name="id">ID of this vertex.</param>
    /// <param name="heuristic">The delegate that determines the heuristic.</param>
    public Vertex(T id, Heuristic heuristic)
    {
        this.id = id;
        this.heuristic = heuristic;
    }

    /* Methods */
    /// <summary>
    /// Uses the id to check if this vertex is visited or not.
    /// </summary>
    /// <param name="id">ID used for lookup.</param>
    /// <returns>True if vertex has been visited by the thing with ID, else false.</returns>
    public bool GetVisited(Guid id)
    {
        if (!visited.ContainsKey(id)) return false;

        return visited[id];
    }

    /// <summary>
    /// Uses the id to set this vertex's visited status.
    /// </summary>
    /// <param name="id">ID used for lookup.</param>
    /// <param name="visited">What to set visited to.</param>
    public void SetVisited(Guid id, bool visited)
    {
        this.visited[id] = visited;
    }

    /// <summary>
    /// Removes the id from the vertex's visited.
    /// </summary>
    /// <param name="id">ID used for lookup.</param>
    public void ResetVisited(Guid id)
    {
        visited.Remove(id);
    }

    /// <summary>
    /// Uses ID to get the aggregate cost of a vertex during traversal.
    /// </summary>
    /// <param name="id">ID used for lookup.</param>
    /// <returns></returns>
    public float GetAggregateCost(Guid id)
    {
        return aggregateCosts.ContainsKey(id) ? aggregateCosts[id] : 0;
    }

    /// <summary>
    /// Uses the id to set this vertex's aggregate cost.
    /// </summary>
    /// <param name="id">ID used for lookup.</param>
    /// <param name="visited">What to set visited to.</param>
    public void SetAggregateCost(Guid id, float cost)
    {
        aggregateCosts[id] = cost;
    }

    /// <summary>
    /// Removes the id from the vertex's aggregate costs.
    /// </summary>
    /// <param name="id">ID used for lookup.</param>
    public void ResetAggregateCost(Guid id)
    {
        aggregateCosts.Remove(id);
    }

    public override string ToString()
    {
        return $"Vertex: {id}";
    }
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}