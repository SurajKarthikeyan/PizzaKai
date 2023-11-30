using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a "flat" subsection of a graph that can be iterated like a
/// singlely linked list. See <see cref="Graph{T}"/>.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2022)
/// </summary>
/// <typeparam name="T"></typeparam>
public class Path<T> : IEnumerable<Vertex<T>>,
    IEnumerable<GraphEdge<T>>, ITraceable<T> where T : IEquatable<T>
{
    #region Variables
    private readonly Dictionary<Vertex<T>, Vertex<T>> path;
    #endregion

    #region Properties
    /// <summary>
    /// The start of the iteration.
    /// </summary>
    public Vertex<T> Start { get; private set; }
    /// <summary>
    /// The end of the iteration.
    /// </summary>
    public Vertex<T> End { get; private set; }
    public Graph<T> Graph { get; private set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new path.
    /// </summary>
    public Path(Vertex<T> start, Vertex<T> end,
        Dictionary<Vertex<T>, Vertex<T>> invertedPath,
        Dictionary<T, float> totalCosts, Graph<T> graph)
    {
        if (start == end)
            throw new PathfindingException($"start and end are the same: {start}");

        path = new();
        Graph = graph;
        Start = start;
        End = end;

        Vertex<T> next = end;

        while (next != start)
        {
            var prev = next;
            try
            {
                next = invertedPath[next];
            }
            catch (KeyNotFoundException e)
            {
                throw new PathfindingException(
                    $"Cannot find vertex ({next}) of inverted path.",
                    e
                );
            }

            if (path.TryGetValue(next, out var other))
            {
                if (totalCosts[prev.id] < totalCosts[other.id])
                {
                    path[next] = prev;
                }
                else
                {
                    path[next] = other;
                }
            }
            else
            {
                path[next] = prev;
            }
        }
    } 
    #endregion

    /// <summary>
    /// Index by <typeparamref name="T"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The vertex associated with the key.</returns>
    public Vertex<T> this[T key] => Graph.GetVertex(key);

    /// <summary>
    /// Traverse to the next item in the path.
    /// </summary>
    /// <param name="begin">The Vertex or Vertex ID at which to begin
    /// iteration.</param>
    /// <returns></returns>
    public T Next(T begin)
    {
        try
        {
            return this[begin].id;
        }
        catch (KeyNotFoundException e)
        {
            throw new PathfindingException(
                $"Vertex of ID {begin} is not in path.",
                e
            );
        }
    }

    /// <inheritdoc cref="Next(T)"/>
    public Vertex<T> Next(Vertex<T> begin)
    {
        try
        {
            return path[begin];
        }
        catch (KeyNotFoundException e)
        {
            var trailingEdge = path
                .FirstOrDefault(edge => edge.Value == begin);
            throw new PathfindingException(
                $"Vertex {begin} (trailing edge [{trailingEdge.Key}, " +
                $"{trailingEdge.Value}]) is not in path.",
                e
            );
        }
    }

    /// <summary>
    /// Traverse to the next N items on the path. Stops traversal if we reach
    /// the end of the path.
    /// </summary>
    /// <param name="steps">How many items to traverse?</param>
    /// <param name="stepsTaken">How many items were actually traversed before
    /// the iteration ended?</param>
    /// <inheritdoc cref="Next(T)"/>
    public T NextN(T begin, int steps, out int stepsTaken)
    {
        return NextN(Graph.GetVertex(begin), steps, out stepsTaken).id;
    }

    /// <inheritdoc cref="NextN(T, int, out int)"/>
    public Vertex<T> NextN(Vertex<T> begin, int steps, out int stepsTaken)
    {
        var next = begin;
        stepsTaken = 0;

        for (int i = 0; i < steps; i++)
        {
            next = Next(next);

            if (next == End)
                break;

            stepsTaken++;
        }

        return next;
    }

    /// <summary>
    /// Traverse the path until either we run out of cost or we reach the end.
    /// </summary>
    /// <param name="maxCost">The maximum cost to concur.</param>
    /// <param name="costUsed">The amount of cost used for the
    /// traversal.</param>
    /// <inheritdoc cref="NextN(T, int, out int)"/>
    public T NextCost(T begin, float maxCost,
        out float costUsed, out float stepsTaken)
    {
        return NextCost(Graph.GetVertex(begin), maxCost,
            out costUsed, out stepsTaken).id;
    }

    /// <summary>
    /// Traverse the path until either we run out of cost or we reach the end.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="maxCost"></param>
    /// <returns></returns>
    public Vertex<T> NextCost(Vertex<T> begin, float maxCost,
        out float costUsed, out float stepsTaken)
    {
        var next = begin;
        var peekNext = Next(next);
        costUsed = 0;
        stepsTaken = 0;

        while (next != End && costUsed + peekNext.heuristic <= maxCost)
        {
            next = peekNext;
            peekNext = Next(peekNext);
            costUsed += next.heuristic;
            stepsTaken++;
        }

        return next;
    }

    /// <summary>
    /// Removes all vertices between <paramref name="left"/> and <paramref
    /// name="right"/>. The weights of the removed edges and heuristics of the
    /// removed vertices are preserved as the weight of the new edge between the
    /// two vertices.
    /// </summary>
    public void Intrasect(Vertex<T> left, Vertex<T> right)
    {
        // Don't need to run this if right is literally the next value.
        if (Next(left) == right)
            return;

        // Step 1: Get total cost of edges.
        float totalCost = GetEdges(left, right)
            .Sum(e => e.weight);

        // Step 2: Remove intermediate vertices and get their heuristics.
        var next = Next(left);
        do
        {
            var prev = next;
            totalCost += prev.heuristic;
            next = Next(next);
            path.Remove(prev);
        }
        while (next != right);

        // Step 3: Bridge left and right.
        path[left] = right;

        // Step 4: Set cost of edge.
        left.Adjacent[right.id] = totalCost;
    }

    #region Get Length
    /// <summary>
    /// Returns the length of the path from <paramref name="from"/> to <paramref
    /// name="to"/>.
    /// </summary>
    /// <param name="from">The vertex to start iteration at.</param>
    /// <param name="to">The vertex to stop iteration at.</param>
    /// <returns></returns>
    public int GetLength(Vertex<T> from, Vertex<T> to)
    {
        if (from == null)
            throw new ArgumentNullException("Vertex from is null");
        else if (to == null)
            throw new ArgumentNullException("Vertex to is null");

        int cnt = 1;

        while (from != to)
        {
            cnt++;
            from = Next(from);
        }

        return cnt;
    }

    /// <summary>
    /// Returns the length of the path from <paramref name="from"/> to <see
    /// cref="End"/>.
    /// </summary>
    /// <inheritdoc cref="GetLength(Vertex{T}, Vertex{T})"/>
    public int GetLength(Vertex<T> from) => GetLength(from, End);

    /// <summary>
    /// Returns the length of the path from <see cref="Start"/> to <see
    /// cref="End"/>.
    /// </summary>
    /// <inheritdoc cref="GetLength(Vertex{T}, Vertex{T})"/>
    public int GetLength() => GetLength(Start, End);

    // <summary>
    /// Returns the length of the path from <paramref name="from"/> to <paramref
    /// name="to"/>.
    /// </summary>
    /// <param name="from">The vertex to start iteration at.</param>
    /// <param name="to">The vertex to stop iteration at.</param>
    /// <returns></returns>
    public int GetLength(T from, T to)
    {
        if (!Graph.TryGetVertex(from, out var fromV))
        {
            throw new ArgumentOutOfRangeException(
                nameof(from),
                $"{from} does not exist in the graph"
            );
        }

        if (!Graph.TryGetVertex(to, out var toV))
        {
            throw new ArgumentOutOfRangeException(
                nameof(to),
                $"{to} does not exist in the graph"
            );
        }

        return GetLength(fromV, toV);
    }

    /// <inheritdoc cref="GetLength(Vertex{T})"/>
    public int GetLength(T from) => GetLength(from, End.Value);
    #endregion

    #region Get Vertices
    /// <summary>
    /// Returns all the vertices along this path from <paramref name="from"/> to
    /// <paramref name="to"/>.
    /// </summary>
    /// <param name="from">The vertex to start the iteration from. This will be
    /// included in the enumerable.</param>
    /// <param name="to">The vertex to end the iteration at. This will be
    /// included in the enumerable.</param>
    /// <returns></returns>
    public IEnumerable<Vertex<T>> GetVertices(Vertex<T> from, Vertex<T> to)
    {
        var next = from;

        while (next != to)
        {
            yield return next;
            next = Next(next);
        }

        yield return to;
    }

    /// <summary>
    /// Returns all the vertices along this path from <paramref name="from"/> to
    /// <see cref="End"/>.
    /// </summary>
    /// <inheritdoc cref="GetVertices(Vertex{T}, Vertex{T})"/>
    public IEnumerable<Vertex<T>> GetVertices(Vertex<T> from)
    {
        return GetVertices(from, End);
    }

    /// <summary>
    /// Returns all the vertices along this path from <see cref="Start"/> to
    /// <see cref="End"/>.
    /// </summary>
    /// <inheritdoc cref="GetVertices(Vertex{T})"/>
    public IEnumerable<Vertex<T>> GetVertices()
    {
        return GetVertices(Start);
    }
    #endregion

    #region Get Edges
    /// <summary>
    /// Iterates through the edges of the path.
    /// </summary>
    /// <param name="from">The vertex to start the iteration from.</param>
    /// <param name="to">The vertex to end the iteration at.</param>
    /// <returns></returns>
    public IEnumerable<GraphEdge<T>> GetEdges(Vertex<T> from, Vertex<T> to)
    {
        GraphEdge<T> edge = new(from, Next(from));

        while (edge.to != to)
        {
            yield return edge;
            from = Next(from);
            edge = new(from, Next(from));
        }

        yield return edge;
    }

    /// <summary>
    /// Iterates through the edges of the path, until we reach the end.
    /// </summary>
    /// <inheritdoc cref="GetEdges(Vertex{T}, Vertex{T})"/>
    public IEnumerable<GraphEdge<T>> GetEdges(Vertex<T> from) =>
        GetEdges(from, End);

    /// <summary>
    /// Iterates through all edges of the path.
    /// </summary>
    /// <inheritdoc cref="GetEdges(Vertex{T}, Vertex{T})"/>
    public IEnumerable<GraphEdge<T>> GetEdges() =>
        GetEdges(Start, End);
    #endregion

    /// <summary>
    /// Iterates through the path, beginning at <paramref name="begin"/>, until
    /// we find a vertex with an id of <paramref name="stopAt"/>,
    /// </summary>
    /// <param name="begin">Where to start the iteration?</param>
    /// <param name="stopAt">What ID to stop at?</param>
    /// <returns></returns>
    public Vertex<T> Seek(Vertex<T> begin, T stopAt)
    {
        foreach (var vertex in GetVertices(begin))
        {
            if (vertex.id.Equals(stopAt))
                return vertex;
        }

        return null;
    }

    /// <summary>
    /// Gets the max cost of any vertex of the path.
    /// </summary>
    /// <returns></returns>
    public float MaxSingleCost()
    {
        float max = 0;

        Vertex<T> next = Start;

        while (next != End)
        {
            next = Next(next);
            max = Math.Max(max, next.heuristic);
        }

        return max;
    }

    #region Enumeration
    public IEnumerable<GraphEdge<T>> GetTraces()
    {
        var next = Start;

        while (next != End && Next(next) != End)
        {
            var prev = next;
            next = Next(next);

            yield return Graph.GetEdge(prev, next);
        }
    }

    public IEnumerator<Vertex<T>> GetEnumerator()
    {
        return GetVertices(Start, End).GetEnumerator();
    }


    IEnumerator<GraphEdge<T>> IEnumerable<GraphEdge<T>>.GetEnumerator()
    {
        return GetTraces().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    } 
    #endregion
}