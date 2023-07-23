using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RNGExt
{
    private static System.Random RNGNum = new System.Random();

    #region Integer
    /// <summary>
    /// Returns an integer ranging from minValue, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from minValue to maxValue - 1.</returns>
    public static int RandomInt(int minVal, int maxVal)
    {
        return RNGNum.Next(minVal, maxVal);
    }

    /// <summary>
    /// Returns an integer ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from 0 to maxValue - 1.</returns>
    public static int RandomInt(int maxVal)
    {
        return RandomInt(0, maxVal);
    }

    /// <summary>
    /// Alias for <see cref="System.Random.Next"/>.
    /// </summary>
    /// <returns>An integer ranging from 0 to <see cref="Int32.MaxValue"/>.</returns>
    public static int RandomInt()
    {
        return RNGNum.Next();
    }
    #endregion

    #region Double
    /// <summary>
    /// Returns a double ranging from minValue, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>An integer ranging from minValue to maxValue - 1.</returns>
    public static double RandomDouble(double minVal, double maxVal)
    {
        return RNGNum.NextDouble() * (maxVal - minVal) + minVal;
    }

    /// <summary>
    /// Returns a double ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A double ranging from 0, inclusive, to maxValue, exclusive.</returns>
    public static double RandomDouble(double maxVal)
    {
        return RandomDouble(0, maxVal);
    }

    /// <summary>
    /// Returns a double ranging from 0, inclusive, to 1, exclusive.
    /// </summary>
    /// <returns>A double ranging from 0 to 1.</returns>
    public static double RandomDouble()
    {
        return RNGNum.NextDouble();
    }
    #endregion

    #region Float
    /// <summary>
    /// Returns a float ranging from minValue, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A float ranging from minValue, inclusive, to maxValue, exclusive.</returns>
    public static float RandomFloat(float minVal, float maxVal)
    {
        return (float)(RNGNum.NextDouble() * (maxVal - minVal) + minVal);
    }

    /// <summary>
    /// Returns a float ranging from 0, inclusive, to maxValue, exclusive.
    /// </summary>
    /// <returns>A float ranging from minValue, inclusive, to maxValue, exclusive.</returns>
    public static float RandomFloat(float maxVal)
    {
        return RandomFloat(0, maxVal);
    }

    /// <summary>
    /// Returns a float ranging from 0, inclusive, to 1, exclusive.
    /// </summary>
    /// <returns>A float ranging from 0 to 1.</returns>
    public static float RandomFloat()
    {
        return (float)RNGNum.NextDouble();
    }

    /// <summary>
    /// Returns true based on percent chance given.
    /// </summary>
    /// <param name="percentChance">A float [0 - 1]</param>
    /// <returns>True based on percent chance given.</returns>
    public static bool PercentChance(float percentChance)
    {
        return RandomFloat() < percentChance;
    }
    #endregion

    #region Vector2
    /// <summary>
    /// Returns a Vector2 with all components ranging from -val (inclusive) to
    /// val (exclusive).
    /// </summary>
    /// <param name="val">The bounds of the Vector2.</param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(float val)
    {
        return RandomVector2(-val, val);
    }

    /// <summary>
    /// Returns a Vector2 with both components ranging from min to max.
    /// </summary>
    /// <param name="min">
    /// Minimum value of the x and y components, inclusive.
    /// </param>
    /// <param name="max">
    /// Maximum value of the x and y components, exclusive.
    /// </param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(float min, float max)
    {
        return RandomVector2(min, min, max, max);
    }

    /// <summary>
    /// Returns a random Vector2 with components ranging between the respective
    /// components of min and max.
    /// </summary>
    /// <param name="min">
    /// The lower bound of the random Vector2, inclusive.
    /// </param>
    /// <param name="max">
    /// The upper bound of the random Vector2, exclusive.
    /// </param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(Vector2 min, Vector2 max)
    {
        return RandomVector2(min.x, min.y, max.x, max.y);
    }

    /// <summary>
    /// Returns a random Vector2 with
    /// the x component ranging from minX to maxX and
    /// the y component ranging from minY to maxY.
    /// </summary>
    /// <param name="minX">Minimum value of the x component, inclusive.</param>
    /// <param name="minY">Minimum value of the y component, inclusive.</param>
    /// <param name="maxX">Maximum value of the x component, exclusive.</param>
    /// <param name="maxY">Maximum value of the y component, exclusive.</param>
    /// <returns>A random Vector2.</returns>
    public static Vector2 RandomVector2(float minX, float minY, float maxX,
        float maxY)
    {
        return new Vector2(RandomFloat(minX, maxX), RandomFloat(minY, maxY));
    }

    /// <summary>
    /// Returns a random Vector2 with its respective components ranging from
    /// rect.min (inclusive) to rect.max (exclusive).
    /// </summary>
    /// <param name="rect">The bounds.</param>
    /// <returns></returns>
    public static Vector2 RandomVector2(Rect rect)
    {
        return RandomVector2(rect.min.x, rect.min.y, rect.max.x,
            rect.max.y);
    }

    /// <summary>
    /// Gets a point on the unit circle.
    /// </summary>
    /// <returns>A point on the unit circle.</returns>
    public static Vector2 OnUnitCircle()
    {
        return Quaternion.Euler(RandomFloat(0, 360), 0, 0) * Vector2.up;
    }
    #endregion

    #region Vector3
    /// <summary>
    /// Returns a Vector3 with all components ranging from -val (inclusive) to
    /// val (exclusive).
    /// </summary>
    /// <param name="val">The bounds of the Vector3.</param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(float val)
    {
        return RandomVector3(-val, val);
    }

    /// <summary>
    /// Returns a Vector3 with all components ranging between min and max.
    /// </summary>
    /// <param name="min">
    /// Minimum value of the x, y, and z components, inclusive.
    /// </param>
    /// <param name="max">
    /// Maximum value of the x, y, and z components, exclusive.
    /// </param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(float min, float max)
    {
        return RandomVector3(min, min, min, max, max, max);
    }

    /// <summary>
    /// Returns a random Vector3 with components ranging between the respective
    /// components of min and max.
    /// </summary>
    /// <param name="min">
    /// The lower bound of the random Vector3, inclusive.
    /// </param>
    /// <param name="max">
    /// The upper bound of the random Vector3, exclusive.
    /// </param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        return RandomVector3(min.x, min.y, min.z, max.x, max.y, max.z);
    }

    /// <summary>
    /// Returns a random Vector3 with
    /// the x component ranging from minX to maxX, 
    /// the y component ranging from minY to maxY, and
    /// the z component ranging from minZ to maxZ.
    /// </summary>
    /// <param name="minX">Minimum value of the x component, inclusive.</param>
    /// <param name="minY">Minimum value of the y component, inclusive.</param>
    /// <param name="minZ">Minimum value of the z component, inclusive.</param>
    /// <param name="maxX">Maximum value of the x component, exclusive.</param>
    /// <param name="maxY">Maximum value of the y component, exclusive.</param>
    /// <param name="maxZ">Maximum value of the z component, exclusive.</param>
    /// <returns>A random Vector3.</returns>
    public static Vector3 RandomVector3(float minX, float minY,
        float minZ, float maxX, float maxY, float maxZ)
    {
        return new Vector3(
            RandomFloat(minX, maxX),
            RandomFloat(minY, maxY),
            RandomFloat(minZ, maxZ)
            );
    }

    /// <summary>
    /// Returns a random Vector3 with its respective components ranging from
    /// bounds.min (inclusive) to bounds.max (exclusive).
    /// </summary>
    /// <param name="bounds">The bounds.</param>
    /// <returns></returns>
    public static Vector3 RandomVector3(Bounds bounds)
    {
        return RandomVector3(bounds.min.x, bounds.min.y, bounds.max.z,
            bounds.max.x, bounds.max.y, bounds.max.z);
    }
    #endregion

    #region IEnumerable & Related
    /// <summary>
    /// Returns a random value from the provided enumeration of values.
    /// </summary>
    /// <param name="defaultOK">If true, then return default value if values
    /// length is 0. Else throw an index error.</param>
    public static T GetRandomValue<T>(this IEnumerable<T> values,
        bool defaultOK = true)
    {
        int length = values.Count();

        if (length < 1 && defaultOK)
        {
            return default;
        }

        return values.ElementAt(RandomInt(length));
    }

    /// <summary>
    /// Returns a random value from the provided enumeration of values.
    /// Alias for <see cref="GetRandomValue"/>.
    /// </summary>
    /// <param name="defaultOK">If true, then return default value if values
    /// length is 0. Else throw an index error.</param>
    public static T RandomSelectOne<T>(this IEnumerable<T> values,
        bool defaultOK = true)
    {
        return values.GetRandomValue(defaultOK);
    }

    /// <summary>
    /// Selects multiple items from an IEnumerable. Can also select none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key">How to get to the percentage [0-1] from any obj.</param>
    /// <returns></returns>
    public static IEnumerable<T> RandomSelectMany<T>(this IEnumerable<T> objs,
        Func<T, float> key)
    {
        List<T> selected = new List<T>();

        var shuffled = objs.ToList();
        shuffled.Shuffle();

        foreach (T obj in shuffled)
        {
            if (PercentChance(key(obj)))
            {
                selected.Add(obj);
            }
        }

        return selected;
    }

    /// <summary>
    /// Randomly selects multiple items from an IEnumerable. Guaranteed to
    /// select at least one item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IEnumerable<T> RandomSelectAtLeastOne<T>(
        this IEnumerable<T> objs, Func<T, float> key)
    {
        var selected = objs.RandomSelectMany(key);

        if (selected.Count() < 1)
        {
            T[] arr = { objs.GetRandomValue() };
            selected = arr.AsEnumerable();
        }

        return selected;
    }

    /// <summary>
    /// Selects one or no items from an IEnumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key">
    /// How to get to the percentage [0-1] from any obj.
    /// </param>
    /// <returns></returns>
    public static T RandomSelectOneOrNone<T>(this IEnumerable<T> objs,
        Func<T, float> key)
    {
        IEnumerable<T> selected = objs.RandomSelectMany(key);

        if (selected.Count() < 1)
        {
            return objs.GetRandomValue();
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Selects one item from an IEnumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="key">How to get to the percentage [0-1] from any obj.</param>
    /// <returns></returns>
    public static T RandomSelectOne<T>(this IEnumerable<T> objs, Func<T, float> key)
    {
        IEnumerable<T> selected = objs.RandomSelectMany(key);

        if (selected.Count() < 1)
        {
            return objs.GetRandomValue();
        }
        else
        {
            return selected.GetRandomValue();
        }
    }


    /// <summary>
    /// Shuffles the list in place.
    /// Adapted from https://stackoverflow.com/a/1262619.
    /// </summary>
    /// <typeparam name="T">Any type.</typeparam>
    /// <param name="list">List to shuffle.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RandomInt(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Selects one item from a param list of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">List to select from.</param>
    /// <returns>A random item from items.</returns>
    public static T SelectOneFrom<T>(params T[] items)
    {
        return items.GetRandomValue<T>();
    }
    #endregion
}
