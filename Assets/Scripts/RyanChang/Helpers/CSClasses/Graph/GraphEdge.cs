using System;

public struct GraphEdge<T> : IEquatable<GraphEdge<T>> where T : IEquatable<T>
{
    #region Constants
    public static readonly GraphEdge<T> InvalidEdge = new(null, null, float.NaN);
    #endregion

    #region Variables
    public float weight;

    public Vertex<T> from;

    public Vertex<T> to;
    #endregion

    #region Properties
    public readonly bool IsValid => float.IsFinite(weight);
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new graph edge.
    /// </summary>
    /// <param name="from">The vertex to start from.</param>
    /// <param name="to">The vertex to go to.</param>
    /// <param name="weight">The weight of the edge.</param>
    public GraphEdge(Vertex<T> from, Vertex<T> to, float weight) : this()
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    /// <summary>
    /// Creates a new graph edge, obtaining the weight from <paramref
    /// name="from"/> <see cref="Vertex{T}.Adjacent"/>.
    /// </summary>
    /// <inheritdoc cref="GraphEdge{T}(Vertex{T}, Vertex{T}, float)"/>
    public GraphEdge(Vertex<T> from, Vertex<T> to) :
        this(from, to, from.Adjacent[to.id])
    {
        
    }
    #endregion

    #region Object Functions
    public override readonly string ToString()
    {
        return $"[({from}, {to}), weight:{weight}]";
    }

    public override readonly int GetHashCode()
    {
        return from.GetHashCode() + to.GetHashCode();
    }

    public override readonly bool Equals(object obj)
    {
        return obj is GraphEdge<T> other && Equals(other);
    }

    public readonly bool Equals(GraphEdge<T> other)
    {
        return from.Equals(other.from) &&
            to.Equals(other.to);
    }
    #endregion
}