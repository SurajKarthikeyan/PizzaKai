using UnityEngine;
using static VectorExt;

/// <summary>
/// Contains methods pertaining to Unity bounds.
/// </summary>
public static class BoundsExt
{
    #region Modifications
    /// <summary>
    /// Translates the bounds by amount. Modifies the bounds in place.
    /// </summary>
    /// <param name="bounds">Bounds to translate.</param>
    /// <param name="amount">Amount to translate by.</param>
    /// <returns></returns>
    public static void Translate(this ref Bounds bounds, Vector3 amount)
    {
        Vector3 center = bounds.center + amount;
        bounds.center = center;
    }
    #endregion

    #region Conversions
    /// <summary>
    /// Converts this bounds to a rect. Follows the logic from
    /// <see cref="VectorExt.ToVector2(Vector3)"/>.
    /// </summary>
    /// <param name="bounds">The bounds used to create the rect.</param>
    /// <returns></returns>
    public static Rect ToRect(this Bounds bounds, Axis axis = DEFAULT_AXIS)
    {
        return new Rect(bounds.min.ToVector2(axis), bounds.size.ToVector2(axis));
    }

    /// <summary>
    /// Converts the specified BoundsInt to a regular Bounds.
    /// </summary>
    /// <param name="bounds">The bounds to convert.</param>
    /// <returns></returns>
    public static Bounds ToBounds(this BoundsInt bounds)
    {
        return new Bounds(bounds.center, bounds.size);
    }

    /// <summary>
    /// Converts the specified BoundsInt to a regular Bounds.
    /// </summary>
    /// <param name="bounds">The bounds to convert.</param>
    /// <returns></returns>
    public static BoundsInt ToBoundsInt(this Bounds bounds)
    {
        return new BoundsInt(bounds.center.ToVector3Int(), bounds.size.ToVector3Int());
    }
    #endregion

    #region Random
    /// <summary>
    /// Returns a random point within <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">The rectangle to evaluate.</param>
    /// <returns></returns>
    public static Vector2 RandomPointIn(this Rect rect)
    {
        return RNGExt.RandomVector2(rect.min, rect.max);
    }

    /// <inheritdoc cref="RandomPointIn(Rect)"/>
    public static Vector2Int RandomPointIn(this RectInt rect)
    {
        return new(
            RNGExt.RandomInt(rect.xMin, rect.xMax),
            RNGExt.RandomInt(rect.yMin, rect.yMax)
        );
    }

    /// <summary>
    /// Returns a random point within <paramref name="bounds"/>.
    /// </summary>
    /// <param name="bounds">The bounds to evaluate.</param>
    /// <returns></returns>
    public static Vector3 RandomPointIn(this Bounds bounds)
    {
        return RNGExt.RandomVector3(bounds.min, bounds.max);
    }

    /// <inheritdoc cref="RandomPointIn(Rect)"/>
    public static Vector3Int RandomPointIn(this BoundsInt bounds)
    {
        return new(
            RNGExt.RandomInt(bounds.xMin, bounds.xMax),
            RNGExt.RandomInt(bounds.yMin, bounds.yMax),
            RNGExt.RandomInt(bounds.zMin, bounds.zMax)
        );
    }
    #endregion
}