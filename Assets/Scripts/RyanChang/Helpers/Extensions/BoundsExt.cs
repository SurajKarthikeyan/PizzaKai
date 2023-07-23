using UnityEngine;

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
    /// <see cref="VectorExt.ToVector2(Vector3)"/>, so the x and y components of
    /// the new rect will be the x and z components of bounds.
    /// </summary>
    /// <param name="bounds">The bounds used to create the rect.</param>
    /// <returns></returns>
    public static Rect ToRect(this Bounds bounds)
    {
        return new Rect(bounds.min.ToVector2(), bounds.size.ToVector2());
    }
    #endregion
}