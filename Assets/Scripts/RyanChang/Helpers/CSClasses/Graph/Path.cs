using System;
using System.Collections.Generic;

public class Path<T> where T : IEquatable<T>
{
    private Dictionary<Vertex<T>, Vertex<T>> path;

    public Vertex<T> Start { get; private set; }
    public Vertex<T> End { get; private set; }
    public Graph<T> Graph { get; private set; }

    /// <summary>
    /// Creates a new path.
    /// </summary>
    /// <param name="path">The path.</param>
    public Path(Dictionary<Vertex<T>, Vertex<T>> path, Vertex<T> start,
        Vertex<T> end, Graph<T> graph)
    {
        this.path = path;
        this.Graph = graph;
        this.Start = start;
        this.End = end;

        if (start == end)
            throw new ArgumentException($"start and end are the same: {start}");
    }

    /// <summary>
    /// Traverse to the next item in the path.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public T Next(T item)
    {
        return path[Graph.GetVertex(item)].id;
    }

    /// <summary>
    /// Traverse to the next item in the path.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Vertex<T> Next(Vertex<T> item)
    {
        return path[item];
    }

    /// <summary>
    /// Traverse to the next N items on the path.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="steps"></param>
    /// <returns></returns>
    public T NextN(T begin, int steps)
    {
        return NextN(Graph.GetVertex(begin), steps).id;
    }

    /// <summary>
    /// Traverse to the next N items on the path.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="steps"></param>
    /// <returns></returns>
    public Vertex<T> NextN(Vertex<T> begin, int steps)
    {
        var next = begin;
        for (int i = 0; i < steps; i++)
        {
            next = path[next];

            if (next == End)
                break;
        }
        return next;
    }

    /// <summary>
    /// Traverse the path until either we run out of cost or we reach the end.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="maxCost"></param>
    /// <returns></returns>
    public T NextCost(T begin, float maxCost)
    {
        return NextCost(Graph.GetVertex(begin), maxCost).id;
    }

    /// <summary>
    /// Traverse the path until either we run out of cost or we reach the end.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="maxCost"></param>
    /// <returns></returns>
    public Vertex<T> NextCost(Vertex<T> begin, float maxCost)
    {
        var next = begin;
        var peekNext = Next(next);
        float cost = 0;

        while (next != Start && cost + peekNext.heuristic() <= maxCost)
        {
            next = peekNext;
            peekNext = Next(peekNext);
            cost += next.heuristic();
        }

        return next;
    }

    public int GetLength()
    {
        int len = 0;

        Vertex<T> next = End;

        while (next != Start)
        {
            next = Next(next);
            len++;
        }

        return len;
    }

    /// <summary>
    /// Gets the max cost of any vertex of the path.
    /// </summary>
    /// <returns></returns>
    public float MaxSingleCost()
    {
        float max = 0;

        Vertex<T> next = End;

        while (next != Start)
        {
            next = Next(next);
            max = Math.Max(max, next.heuristic());
        }

        return max;
    }
}