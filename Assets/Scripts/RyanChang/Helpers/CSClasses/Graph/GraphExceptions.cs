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

public class VertexNotInAdjacentException : PathfindingException
{
    public VertexNotInAdjacentException(object vertex, object adjacentID)
        : base($"Vertex {adjacentID} is not adjacent to {vertex}.")
    {
    }
}

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


public class DisjointGraphException : PathfindingException
{
    public DisjointGraphException(string message) : base(message)
    {
    }

    public DisjointGraphException(string message, Exception innerException) : base(message, innerException)
    {
    }
}