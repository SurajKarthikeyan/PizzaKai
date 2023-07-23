using UnityEngine;

/// <summary>
/// Contains methods pertaining to Unity rectangles.
/// </summary>
public static class RectExt
{
    #region Modifications
    /// <summary>
    /// Translates the rectangle by amount. Modifies the rectangle in place.
    /// </summary>
    /// <param name="rect">Rectangle to translate.</param>
    /// <param name="amount">Amount to translate by.</param>
    /// <returns></returns>
    public static void Translate(this ref Rect rect, Vector2 amount)
    {
        Vector2 center = rect.center + amount;
        rect.center = center;
    }
    #endregion

    #region Conversions
    /// <summary>
    /// Converts this rect to a bounds. Follows the logic from
    /// <see cref="VectorExt.ToVector3(Vector2, float)"/>, so the x and z
    /// components of the new bounds will be the x and y components of rect.
    /// </summary>
    /// <param name="rect">The rect used to create the bounds.</param>
    /// <param name="y">The optional y component that will be used as the y
    /// component of the minimum corner of the new bounds.</param>
    /// <param name="height">The optional height of the new bounds.</param>
    /// <returns></returns>
    public static Bounds ToRect(this Rect rect, float y = 0, float height = 0)
    {
        return new Bounds(rect.min.ToVector3(y), rect.size.ToVector3(height));
    }
    #endregion
}