using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Contains methods pertaining to Vector3 and Vector2.
/// </summary>
public static class VectorExt
{
    #region Conversion
    /// <summary>
    /// Converts from a Vector2, v2, to a Vector3, v3, such that
    /// v3.x == v2.x and v3.z == v2.y and v3.y == 0
    /// </summary>
    /// <param name="v">The Vector2 to convert.</param>
    /// <param name="y">The y component of the new Vector3</param>
    /// <returns>The converted Vector3.</returns>
    public static Vector3 ToVector3(this Vector2 v, float y = 0)
    {
        return new(v.x, y, v.y);
    }

    /// <summary>
    /// Converts from a Vector3Int to a Vector3.
    /// </summary>
    /// <param name="v">The Vector3Int to convert.</param>
    /// <returns>The converted Vector3.</returns>
    public static Vector3 ToVector3(this Vector3Int v)
    {
        return new(v.x, v.y, v.z);
    }

    /// <summary>
    /// Converts from a Vector3, v3, to a Vector2, v2, such that
    /// v2.x == v3.x and v2.y == v3.z
    /// </summary>
    /// <param name="v">The Vector3 to convert.</param>
    /// <returns>The converted Vector2.</returns>
    public static Vector2 ToVector2(this Vector3 v)
    {
        return new(v.x, v.z);
    }

    /// <summary>
    /// Converts from a Vector2Int to a Vector2.
    /// </summary>
    /// <param name="v">The Vector2Int to convert.</param>
    /// <returns>The converted Vector2.</returns>
    public static Vector2 ToVector2(this Vector2Int v)
    {
        return new(v.x, v.y);
    }

    /// <summary>
    /// Converts from a Vector3 to a Vector3Int. Rounds all components.
    /// </summary>
    /// <param name="v">The Vector3 to convert.</param>
    /// <returns>The converted Vector3Int.</returns>
    public static Vector3Int ToVector3Int(this Vector3 v)
    {
        return new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y),
            Mathf.RoundToInt(v.z));
    }

    /// <summary>
    /// Converts from a Vector2 to a Vector2Int. Rounds all components.
    /// </summary>
    /// <param name="v">The Vector2 to convert.</param>
    /// <returns>The converted Vector2Int.</returns>
    public static Vector2Int ToVector2Int(this Vector2 v)
    {
        return new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    /// <summary>
    /// Converts from polar coordinates
    /// </summary>
    /// <param name="r">Radius</param>
    /// <param name="theta">Angle, in radians</param>
    /// <returns></returns>
    public static Vector2 FromPolar(float r, float theta)
    {
        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);

        return new Vector2(x, y);
    }
    #endregion

    #region Comparison
    /// <summary>
    /// Returns true if a differs from b by at most margin. Otherwise, returns
    /// false.
    /// </summary>
    /// <param name="a">First vector.</param>
    /// <param name="b">Second vector.</param>
    /// <param name="margin">How much can a differ from b (component wise).</param>
    /// <returns></returns>
    public static bool Approx(this Vector3 a, Vector3 b, float margin)
    {
        return a.x.Approx(b.x, margin) && a.y.Approx(b.y, margin)
            && a.z.Approx(b.z, margin);
    }

    /// <summary>
    /// Returns true if a is approximately b. Otherwise, returns false.
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>True if a is approximately b, false otherwise</returns>
    public static bool Approx(this Vector3 a, Vector3 b)
    {
        return a.x.Approx(b.x) && a.y.Approx(b.y) && a.z.Approx(b.z);
    }

    /// <summary>
    /// Returns true if a is approximately zero. Otherwise, returns false.
    /// </summary>
    /// <param name="a">Vector</param>
    /// <returns>True if a is approximately zero, false otherwise.</returns>
    public static bool ApproxZero(this Vector3 a)
    {
        return a.Approx(Vector3.zero);
    }

    /// <summary>
    /// Returns true if a differs from b by at most margin. Otherwise, returns
    /// false.
    /// </summary>
    /// <param name="a">First vector.</param>
    /// <param name="b">Second vector.</param>
    /// <param name="margin">How much can a differ from b (component wise).</param>
    /// <returns></returns>
    public static bool Approx(this Vector2 a, Vector2 b, float margin)
    {
        return a.x.Approx(b.x, margin) && a.y.Approx(b.y, margin);
    }

    /// <summary>
    /// Returns true if a is approximately b. Otherwise, returns false.
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">Second vector</param>
    /// <returns>True if a is approximately b, false otherwise</returns>
    public static bool Approx(this Vector2 a, Vector2 b)
    {
        return a.x.Approx(b.x) && a.y.Approx(b.y);
    }

    /// <summary>
    /// Returns true if a is approximately zero. Otherwise, returns false.
    /// </summary>
    /// <param name="a">Vector</param>
    /// <returns>True if a is approximately zero, false otherwise.</returns>
    public static bool ApproxZero(this Vector2 a)
    {
        return a.Approx(Vector2.zero);
    }
    #endregion

    #region Component-Wise
    #region Min
    /// <summary>
    /// Returns the minimum component in v.
    /// </summary>
    /// <param name="v">The Vector2 to evaluate.</param>
    /// <returns></returns>
    public static float MinComponent(this Vector2 v)
    {
        return Mathf.Min(v.x, v.y);
    }

    /// <summary>
    /// Returns the minimum component in v.
    /// </summary>
    /// <param name="v">The Vector3 to evaluate.</param>
    /// <returns></returns>
    public static float MinComponent(this Vector3 v)
    {
        return Mathf.Min(v.x, v.y, v.z);
    }

    /// <summary>
    /// Returns the minimum component in v.
    /// </summary>
    /// <param name="v">The Vector2Int to evaluate.</param>
    /// <returns></returns>
    public static float MinComponent(this Vector2Int v)
    {
        return Mathf.Min(v.x, v.y);
    }

    /// <summary>
    /// Returns the minimum component in v.
    /// </summary>
    /// <param name="v">The Vector3Int to evaluate.</param>
    /// <returns></returns>
    public static float MinComponent(this Vector3Int v)
    {
        return Mathf.Min(v.x, v.y, v.z);
    }
    
    /// <summary>
    /// Returns a vector that is the minimum component of all other vectors.
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static Vector2 WithMinComponents(params Vector2[] vertices)
    {
        return new(
            vertices.Min(v => v.x),
            vertices.Min(v => v.y)
        );
    }

    /// <inheritdoc cref="WithMinComponents(Vector2[])"/>
    public static Vector2Int WithMinComponents(params Vector2Int[] vertices)
    {
        return new(
            vertices.Min(v => v.x),
            vertices.Min(v => v.y)
        );
    }

    /// <inheritdoc cref="WithMinComponents(Vector2[])"/>
    public static Vector3 WithMinComponents(params Vector3[] vertices)
    {
        return new(
            vertices.Min(v => v.x),
            vertices.Min(v => v.y),
            vertices.Min(v => v.z)
        );
    }

    /// <inheritdoc cref="WithMinComponents(Vector3[])"/>
    public static Vector3Int WithMinComponents(params Vector3Int[] vertices)
    {
        return new(
            vertices.Min(v => v.x),
            vertices.Min(v => v.y),
            vertices.Min(v => v.z)
        );
    }
    #endregion

    #region Max
    /// <summary>
    /// Returns the maximum component in v.
    /// </summary>
    /// <param name="v">The Vector2 to evaluate.</param>
    /// <returns></returns>
    public static float MaxComponent(this Vector2 v)
    {
        return Mathf.Max(v.x, v.y);
    }

    /// <summary>
    /// Returns the maximum component in v.
    /// </summary>
    /// <param name="v">The Vector3 to evaluate.</param>
    /// <returns></returns>
    public static float MaxComponent(this Vector3 v)
    {
        return Mathf.Max(v.x, v.y, v.z);
    }

    /// <summary>
    /// Returns the maximum component in v.
    /// </summary>
    /// <param name="v">The Vector2Int to evaluate.</param>
    /// <returns></returns>
    public static float MaxComponent(this Vector2Int v)
    {
        return Mathf.Max(v.x, v.y);
    }

    /// <summary>
    /// Returns the maximum component in v.
    /// </summary>
    /// <param name="v">The Vector3Int to evaluate.</param>
    /// <returns></returns>
    public static float MaxComponent(this Vector3Int v)
    {
        return Mathf.Max(v.x, v.y, v.z);
    }
    
    /// <summary>
    /// Returns a vector that is the maximum component of all other vectors.
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static Vector2 WithMaxComponents(params Vector2[] vertices)
    {
        return new(
            vertices.Max(v => v.x),
            vertices.Max(v => v.y)
        );
    }

    /// <inheritdoc cref="WithMaxComponents(Vector2[])"/>
    public static Vector2Int WithMaxComponents(params Vector2Int[] vertices)
    {
        return new(
            vertices.Max(v => v.x),
            vertices.Max(v => v.y)
        );
    }

    /// <inheritdoc cref="WithMaxComponents(Vector2[])"/>
    public static Vector3 WithMaxComponents(params Vector3[] vertices)
    {
        return new(
            vertices.Max(v => v.x),
            vertices.Max(v => v.y),
            vertices.Max(v => v.z)
        );
    }

    /// <inheritdoc cref="WithMaxComponents(Vector3[])"/>
    public static Vector3Int WithMaxComponents(params Vector3Int[] vertices)
    {
        return new(
            vertices.Max(v => v.x),
            vertices.Max(v => v.y),
            vertices.Max(v => v.z)
        );
    }
    #endregion

    #region Average
    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components in v1 and v2. This is the center of v1 and v2.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns></returns>
    public static Vector2 Average(this Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y) / 2f;
    }

    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components specified in vectors.
    /// </summary>
    /// <param name="vectors">List of vectors.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">
    /// If passed an empty list of params.
    /// </exception>
    public static Vector2 Average(params Vector2[] vectors)
    {
        if (vectors.Length > 0)
        {
            Vector2 sum = new();

            foreach (var v in vectors) sum += v;

            return sum / vectors.Length;
        }
        else
        {
            throw new System.ArgumentException("Cannot provide empty params");
        }
    }

    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components in v1 and v2. This is the center of v1 and v2.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns></returns>
    public static Vector2Int Average(this Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.x + v2.x, v1.y + v2.y) / 2;
    }

    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components specified in vectors.
    /// </summary>
    /// <param name="vectors">List of vectors.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">
    /// If passed an empty list of params.
    /// </exception>
    public static Vector2Int Average(params Vector2Int[] vectors)
    {
        if (vectors.Length > 0)
        {
            Vector2Int sum = new();

            foreach (var v in vectors) sum += v;

            return sum / vectors.Length;
        }
        else
        {
            throw new System.ArgumentException("Cannot provide empty params");
        }
    }

    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components in v1 and v2. This is the center of v1 and v2.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns></returns>
    public static Vector3 Average(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z) / 2f;
    }

    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components specified in vectors.
    /// </summary>
    /// <param name="vectors">List of vectors.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">
    /// If passed an empty list of params.
    /// </exception>
    public static Vector3 Average(params Vector3[] vectors)
    {
        if (vectors.Length > 0)
        {
            Vector3 sum = new();

            foreach (var v in vectors) sum += v;

            return sum / vectors.Length;
        }
        else
        {
            throw new System.ArgumentException("Cannot provide empty params");
        }
    }

    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components in v1 and v2. This is the center of v1 and v2.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns></returns>
    public static Vector3Int Average(this Vector3Int v1, Vector3Int v2)
    {
        return new Vector3Int(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z) / 2;
    }

    /// <summary>
    /// Returns a vector whose components are the arithmetic average of all of
    /// the individual components specified in vectors.
    /// </summary>
    /// <param name="vectors">List of vectors.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">
    /// If passed an empty list of params.
    /// </exception>
    public static Vector3Int Average(params Vector3Int[] vectors)
    {
        if (vectors.Length > 0)
        {
            Vector3Int sum = new();

            foreach (var v in vectors) sum += v;

            return sum / vectors.Length;
        }
        else
        {
            throw new System.ArgumentException("Cannot provide empty params");
        }
    }
    #endregion

    #region Sum
    /// <summary>
    /// Returns the sum of all components in the vector.
    /// </summary>
    /// <param name="v">The vector to evaluate.</param>
    /// <returns>The sum of all components.</returns>
    public static float SumComponents(this Vector2 v)
    {
        return v.x + v.y;
    }

    /// <summary>
    /// Returns the sum of all components in the vector.
    /// </summary>
    /// <param name="v">The vector to evaluate.</param>
    /// <returns>The sum of all components.</returns>
    public static float SumComponents(this Vector3 v)
    {
        return v.x + v.y + v.z;
    }

    /// <summary>
    /// Returns the sum of all components in the vector.
    /// </summary>
    /// <param name="v">The vector to evaluate.</param>
    /// <returns>The sum of all components.</returns>
    public static int SumComponents(this Vector2Int v)
    {
        return v.x + v.y;
    }

    /// <summary>
    /// Returns the sum of all components in the vector.
    /// </summary>
    /// <param name="v">The vector to evaluate.</param>
    /// <returns>The sum of all components.</returns>
    public static int SumComponents(this Vector3Int v)
    {
        return v.x + v.y + v.z;
    }
    #endregion
    #endregion

    #region Operations
    #region Abs
    /// <summary>
    /// Returns the vector with all components positive.
    /// </summary>
    /// <param name="v">The Vector2 to evaluate.</param>
    /// <returns></returns>
    public static Vector2 Abs(this Vector2 v)
    {
        return new(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    /// <summary>
    /// Returns the vector with all components positive.
    /// </summary>
    /// <param name="v">The Vector3 to evaluate.</param>
    /// <returns></returns>
    public static Vector3 Abs(this Vector3 v)
    {
        return new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    /// <summary>
    /// Returns the vector with all components positive.
    /// </summary>
    /// <param name="v">The Vector2Int to evaluate.</param>
    /// <returns></returns>
    public static Vector2Int Abs(this Vector2Int v)
    {
        return new(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    /// <summary>
    /// Returns the vector with all components positive.
    /// </summary>
    /// <param name="v">The Vector3Int to evaluate.</param>
    /// <returns></returns>
    public static Vector3Int Abs(this Vector3Int v)
    {
        return new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }
    #endregion

    #region Trig
    /// <summary>
    /// Rotates a vector by theta degrees
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="theta">In degrees</param>
    /// <returns></returns>
    public static Vector2 RotateVector2(this Vector2 vec, float theta)
    {
        float sin = Mathf.Sin(theta * Mathf.Deg2Rad);
        float cos = Mathf.Cos(theta * Mathf.Deg2Rad);

        float oldX = vec.x, oldY = vec.y;

        return new Vector2(cos * oldX - sin * oldY, sin * oldX + cos * oldY);
    }
    #endregion


    #region Distance
    /// <summary>
    /// Gets the taxicab distance between two vectors.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The taxicab distance.</returns>
    public static float TaxicabDistance(this Vector2 v1, Vector2 v2)
    {
        return (v1 - v2).Abs().SumComponents();
    }

    /// <summary>
    /// Gets the taxicab distance between two vectors.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The taxicab distance.</returns>
    public static int TaxicabDistance(this Vector2Int v1, Vector2Int v2)
    {
        return (v1 - v2).Abs().SumComponents();
    }

    /// <summary>
    /// Gets the taxicab distance between two vectors.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The taxicab distance.</returns>
    public static float TaxicabDistance(this Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).Abs().SumComponents();
    }

    /// <summary>
    /// Gets the taxicab distance between two vectors.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The taxicab distance.</returns>
    public static int TaxicabDistance(this Vector3Int v1, Vector3Int v2)
    {
        return (v1 - v2).Abs().SumComponents();
    }
    #endregion

    #region Kernel
    /// <summary>
    /// Returns a list of vectors that contains every vector within an area
    /// of (2 r + 1) by (2 r + 1) of the vector at.
    /// </summary>
    /// <param name="at">Center of the kernel.</param>
    /// <param name="r">Kernel radius.</param>
    /// <returns></returns>
    public static List<Vector2Int> SquareKernel(this Vector2Int at, int r)
    {
        List<Vector2Int> list = new();

        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                list.Add(at + new Vector2Int(x, y));
            }
        }

        return list;
    }

    /// <summary>
    /// Returns a list of vectors that contains every vector within an volume
    /// of (2 r + 1) by (2 r + 1) by (2 r + 1) of the vector at.
    /// </summary>
    /// <param name="at">Center of the kernel.</param>
    /// <param name="r">Kernel radius.</param>
    /// <returns></returns>
    public static List<Vector3Int> CubeKernel(this Vector3Int at, int r)
    {
        List<Vector3Int> list = new();

        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                for (int z = -r; z <= r; z++)
                {
                    list.Add(at + new Vector3Int(x, y, z));
                }
            }
        }

        return list;
    }
    #endregion
    #endregion
}