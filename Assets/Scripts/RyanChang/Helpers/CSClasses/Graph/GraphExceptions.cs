using System;

public class PathfindingException : System.InvalidOperationException
{
    public PathfindingException(string message) : base(message)
    {

    }

    public PathfindingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Thrown when a vertex id cannot be found in the <see
/// cref="Vertex{T}.adjacent"/> dictionary.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class VertexNotInAdjacentException : PathfindingException
{
    public VertexNotInAdjacentException(object vertex, object adjacentID)
        : base($"Vertex {adjacentID} is not adjacent to {vertex}.")
    {
    }
}

/// <summary>
/// Thrown when the vertex cannot be found inside of a graph.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class VertexNotInGraphException : PathfindingException{
    public VertexNotInGraphException(object vertexID, object graph)
        : base($"Vertex {vertexID} cannot be found in graph {graph}.")
    {

    }

    public VertexNotInGraphException(object vertexID, object graph,
        Exception innerException)
        : base($"Vertex {vertexID} cannot be found in graph {graph}.",
            innerException)
    {

    }
}

/// <summary>
/// Thrown when an edge between two vertices does not exist within the graph.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class EdgeNotInGraphException : PathfindingException
{
    public EdgeNotInGraphException(object fromID, object toID, object graphValue)
        : base($"Edge from {fromID} to {toID} cannot be found in graph {graphValue}.")
    {

    }

    public EdgeNotInGraphException(object fromID, object toID, object graphValue,
        Exception innerException)
        : base($"Edge from {fromID} to {toID} cannot be found in graph {graphValue}.",
            innerException)
    {

    }
}

/// <summary>
/// Called when the graph detects that it is disjoint, that is, it would be
/// impossible to travel from vertex A to vertex B.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
public class DisjointGraphException : PathfindingException
{
    public DisjointGraphException(string message) : base(message)
    {
    }

    public DisjointGraphException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Called when the graph runs out of vertices to iterate over.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class RanOutOfVerticesException : PathfindingException
{
    public RanOutOfVerticesException(object stuck, object end) :
        base(
            "Ran out of vertices during iteration. " +
            $"Iteration stuck at {stuck}. Cannot get to {end}. " +
            "Is graph disjoint?"
        ) {}
}

/// <summary>
/// Throw if both start and end vertices are the same.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public class StartIsEndVertexException : PathfindingException
{
    public StartIsEndVertexException(object startAndEnd, string message) :
    base(
        $"{message} : Start and end vertices are both {startAndEnd}."
    ) {}
}