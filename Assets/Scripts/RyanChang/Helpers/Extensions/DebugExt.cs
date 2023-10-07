using System;
using UnityEngine;

/// <summary>
/// Contains methods pertaining to debug drawing, gizmos drawing, and other
/// methods that may be useful for debugging.
/// </summary>
public static class DebugExt
{
    #region Drawing
    #region Variables
    public static Color primaryColor;
    public static Color secondaryColor;
    public static Color tertiaryColor;
    public static Color quaternaryColor;

    public static float drawDuration;

    public enum DrawingMode
    {
        /// <summary>
        /// Draws using methods from Debug.
        /// </summary>
        Debug,
        /// <summary>
        /// Draws using methods from Gizmos. duration parameters are ignored.
        /// </summary>
        Gizmos
    }

    private static DrawingMode drawingMode;
    #endregion

    #region Setup
    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    /// <param name="tertiary">The second accent color to use.</param>
    /// <param name="quaternary">The third accent color to use.</param>
    /// <param name="duration">Time to show the drawn object.</param>
    public static void UseDebug(Color primary, Color secondary, Color tertiary,
        Color quaternary, float duration)
    {
        primaryColor = primary;
        secondaryColor = secondary;
        tertiaryColor = tertiary;
        quaternaryColor = quaternary;
        drawingMode = DrawingMode.Debug;
        drawDuration = duration;
    }

    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    /// <param name="tertiary">The second accent color to use.</param>
    /// <param name="quaternary">The third accent color to use.</param>
    public static void UseDebug(Color primary, Color secondary, Color tertiary,
        Color quaternary)
    {
        UseDebug(primary, secondary, tertiary, quaternary, Time.deltaTime);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    /// <param name="tertiary">The second accent color to use.</param>
    /// <param name="duration">Time to show the drawn object.</param>
    public static void UseDebug(Color primary, Color secondary, Color tertiary,
        float duration)
    {
        UseDebug(primary, secondary, tertiary, tertiary, duration);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    /// <param name="tertiary">The second accent color to use.</param>
    public static void UseDebug(Color primary, Color secondary, Color tertiary)
    {
        UseDebug(primary, secondary, tertiary, tertiary);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    /// <param name="duration">Time to show the drawn object.</param>
    public static void UseDebug(Color primary, Color secondary, float duration)
    {
        UseDebug(primary, secondary, secondary, duration);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    public static void UseDebug(Color primary, Color secondary)
    {
        UseDebug(primary, secondary, secondary);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="duration">Time to show the drawn object.</param>
    public static void UseDebug(Color primary, float duration)
    {
        UseDebug(primary, primary, duration);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Debug methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    public static void UseDebug(Color primary)
    {
        UseDebug(primary, primary);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Gizmos methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    /// <param name="tertiary">The second accent color to use.</param>
    /// <param name="quaternary">The third accent color to use.</param>
    public static void UseGizmos(Color primary, Color secondary, Color tertiary,
        Color quaternary)
    {
        primaryColor = primary;
        secondaryColor = secondary;
        tertiaryColor = tertiary;
        quaternaryColor = quaternary;
        drawingMode = DrawingMode.Gizmos;
    }

    /// <summary>
    /// Sets up DebugExt to draw with Gizmos methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    /// <param name="tertiary">The second accent color to use.</param>
    public static void UseGizmos(Color primary, Color secondary, Color tertiary)
    {
        UseGizmos(primary, secondary, tertiary, tertiary);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Gizmos methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    /// <param name="secondary">The first accent color to use.</param>
    public static void UseGizmos(Color primary, Color secondary)
    {
        UseGizmos(primary, secondary, secondary);
    }

    /// <summary>
    /// Sets up DebugExt to draw with Gizmos methods.
    /// </summary>
    /// <param name="primary">The primary color used to draw.</param>
    public static void UseGizmos(Color primary)
    {
        UseGizmos(primary, primary);
    }
    #endregion

    #region Internal
    private static void DrawFace(Vector3[] corners)
    {
        // Draw outer box
        for (int i = 1; i < corners.Length; i++)
        {
            DrawLine(corners[i - 1], corners[i], primaryColor);
        }
        DrawLine(corners[0], corners[corners.Length - 1], primaryColor);

        // Draw cross
        DrawLine(corners[0], corners[2], secondaryColor);
        DrawLine(corners[1], corners[3], secondaryColor);
    }

    /// <summary>
    /// Draws a rectangular prism.
    /// </summary>
    /// <param name="corners"></param>
    private static void Draw8Face(Vector3[] corners)
    {
        // Draw outer box
        for (int i = 1; i < corners.Length; i++)
        {
            DrawLine(corners[i - 1], corners[i], primaryColor);
        }
        DrawLine(corners[0], corners[corners.Length - 1], primaryColor);

        // Other corners
        DrawLine(corners[1], corners[6], primaryColor);
        DrawLine(corners[2], corners[7], primaryColor);
        DrawLine(corners[3], corners[8], primaryColor);

        // Draw cross
        DrawLine(corners[0], corners[7], secondaryColor);
        DrawLine(corners[1], corners[8], secondaryColor);
        DrawLine(corners[2], corners[9], secondaryColor);
        DrawLine(corners[3], corners[6], secondaryColor);
    }

    private static void DrawLine(Vector3 from, Vector3 to, Color color)
    {
        switch (drawingMode)
        {
            case DrawingMode.Debug:
                Debug.DrawLine(from, to, color, drawDuration);
                break;
            case DrawingMode.Gizmos:
                Gizmos.color = color;
                Gizmos.DrawLine(from, to);
                break;
        }
    }
    #endregion

    #region Cross Square
    /// <summary>
    /// Draws a crossed square.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="rotation">Rotation of the drawn item.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossSquare(Vector3 position, Quaternion rotation,
        float size)
    {
        float half = size / 2;

        // Define corners
        Vector3[] corners = {
            // First face
            new Vector3(half, half, 0),
            new Vector3(-half, half, 0),
            new Vector3(-half, -half, 0),
            new Vector3(half, -half, 0),
            new Vector3(half, half, 0),
        };

        // Apply rotation, then transformation.
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = rotation * corners[i] + position;
        }

        DrawFace(corners);
    }

    /// <summary>
    /// Draws a crossed square.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossSquare(Vector3 position, float size)
    {
        DrawCrossSquare(position, Quaternion.identity, size);
    }
    #endregion

    #region Cross Cube
    /// <summary>
    /// Draws a crossed cube.
    /// </summary>
    /// <param name="position">Center of the cross, at world position.</param>
    /// <param name="rotation">Rotation of the drawn item.</param>
    /// <param name="size">Length of the cube.</param>
    public static void DrawCrossCube(Vector3 position, Quaternion rotation,
        float size)
    {
        float half = size / 2;

        // Define corners
        Vector3[] corners = {
            // First face
            new Vector3(half, half, half),
            new Vector3(-half, half, half),
            new Vector3(-half, -half, half),
            new Vector3(half, -half, half),
            new Vector3(half, half, half),
            //Cross over to other face
            new Vector3(half, half, -half),
            new Vector3(-half, half, -half),
            new Vector3(-half, -half, -half),
            new Vector3(half, -half, -half),
            new Vector3(half, half, -half),
        };

        // Apply rotation, then transformation.
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = rotation * corners[i] + position;
        }

        Draw8Face(corners);
    }

    /// <summary>
    /// Draws a crossed cube.
    /// </summary>
    /// <param name="position">Center of the cross.</param>
    /// <param name="size">Length of the box.</param>
    public static void DrawCrossCube(Vector3 position, float size)
    {
        DrawCrossCube(position, Quaternion.identity, size);
    }
    #endregion

    #region Cross Rect
    /// <summary>
    /// Draws a crossed rectangle.
    /// </summary>
    /// <param name="min">Minimum corner of the rectangle.</param>
    /// <param name="max">Maximum corner of the rectangle.</param>
    /// <param name="rotation">Rotation of the drawn object.</param>
    public static void DrawCrossRect(Vector2 min, Vector2 max,
        Quaternion rotation)
    {
        // Define corners
        Vector3[] corners = {
            new Vector2(max.x, max.y).ToVector3(),
            new Vector2(min.x, max.y).ToVector3(),
            new Vector2(min.x, min.y).ToVector3(),
            new Vector2(max.x, min.y).ToVector3()
        };

        // Center rect at origin, rotate, then transform back to original
        // position.
        Vector3 center = min.Average(max).ToVector3();
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = rotation * (corners[i] - center) + center;
        }

        DrawFace(corners);
    }

    /// <summary>
    /// Draws a crossed rectangle.
    /// </summary>
    /// <param name="min">Minimum corner of the rectangle.</param>
    /// <param name="max">Maximum corner of the rectangle.</param>
    public static void DrawCrossRect(Vector2 min, Vector2 max)
    {
        DrawCrossRect(min, max, Quaternion.identity);
    }

    /// <summary>
    /// Draws a crossed rectangle.
    /// </summary>
    /// <param name="rect">Rectangle to draw.</param>
    /// <param name="rotation">Rotation of the drawn object.</param>
    public static void DrawCrossRect(Rect rect, Quaternion rotation)
    {
        Vector2 max = rect.max, min = rect.min;

        // Define corners
        Vector3[] corners = {
            new Vector2(max.x, max.y).ToVector3(),
            new Vector2(min.x, max.y).ToVector3(),
            new Vector2(min.x, min.y).ToVector3(),
            new Vector2(max.x, min.y).ToVector3()
        };

        // Center rect at origin, rotate, then transform back to original
        // position.
        Vector3 center = rect.center.ToVector3();
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = rotation * (corners[i] - center) + center;
        }

        DrawFace(corners);
    }

    /// <summary>
    /// Draws a crossed rectangle.
    /// </summary>
    /// <param name="rect">Rectangle to draw.</param>
    public static void DrawCrossRect(Rect rect)
    {
        DrawCrossRect(rect, Quaternion.identity);
    }
    #endregion

    #region Cross Bounds
    /// <summary>
    /// Draws a crossed bound.
    /// </summary>
    /// <param name="bounds">Bounds to draw.</param>
    /// <param name="rotation">Rotation of the drawn item.</param>
    public static void DrawCrossBounds(Bounds bounds, Quaternion rotation)
    {
        Vector3 max = bounds.max;
        Vector3 min = bounds.min;

        // Define corners
        Vector3[] corners = {
            // First face
            new Vector3(max.x, max.y, max.z),
            new Vector3(min.x, max.y, max.z),
            new Vector3(min.x, min.y, max.z),
            new Vector3(max.x, min.y, max.z),
            new Vector3(max.x, max.y, max.z),
            //Cross over to other face
            new Vector3(max.x, max.y, min.z),
            new Vector3(min.x, max.y, min.z),
            new Vector3(min.x, min.y, min.z),
            new Vector3(max.x, min.y, min.z),
            new Vector3(max.x, max.y, min.z),
        };

        // Center rect at origin, rotate, then transform back to original
        // position.
        Vector3 center = bounds.center;
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = rotation * (corners[i] - center) + center;
        }

        Draw8Face(corners);
    }

    /// <summary>
    /// Draws a crossed bound.
    /// </summary>
    /// <param name="bounds">Bounds to draw.</param>
    public static void DrawCrossBounds(Bounds bounds)
    {
        DrawCrossBounds(bounds, Quaternion.identity);
    }

    /// <inheritdoc cref="DrawCrossBounds(Bounds, Quaternion)"/>
    private static void DrawCrossBounds(BoundsInt bounds, Quaternion rotation)
    {
        DrawCrossBounds(bounds.ToBounds(), rotation);
    }


    /// <inheritdoc cref="DrawCrossBounds(Bounds)"/>
    public static void DrawCrossBounds(BoundsInt bounds)
    {
        DrawCrossBounds(bounds, Quaternion.identity);
    }
    #endregion
    #endregion
}