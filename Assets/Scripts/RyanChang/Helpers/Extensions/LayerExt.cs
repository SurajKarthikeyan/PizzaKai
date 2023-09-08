using UnityEngine;

/// <summary>
/// Contains methods that pertain to Unity layers.
/// </summary>
public static class LayerExt
{
    /// <summary>
    /// Returns true if layer is in mask.
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Returns true if the layer is in mask.
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool ContainsLayer(this int mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}