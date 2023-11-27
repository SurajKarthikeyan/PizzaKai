
using System;
using UnityEngine;

/// <summary>
/// Contains methods pertaining to C# floats, doubles, and ints.
/// </summary>
public static class NumericalExt
{
    #region Comparison
    /// <summary>
    /// True if a differs from b by no more than margin
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public static bool Approx(this float a, float b, float margin)
    {
        return Mathf.Abs(a - b) <= margin;
    }

    /// <summary>
    /// Alias to Mathf.Approximate
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool Approx(this float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    /// <summary>
    /// Evaluates if <paramref name="values"/> to determine if all members are
    /// equal.
    /// </summary>
    /// <param name="values">The values to evaluate.</param>
    /// <returns>True if all members of <paramref name="values"/>, or if there
    /// are less than 2 values in <paramref name="values"/>. False
    /// otherwise.</returns>
    public static bool AllEqual<T>(params T[] values)
        where T : IEquatable<T>
    {
        if (values.Length < 2)
            return true;

        T temp = values[0];
        
        for (int i = 1; i < values.Length; i++)
        {
            if (!temp.Equals(values[i]))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Returns true if number is in between bounds A and B, inclusive
    /// </summary>
    /// <param name="number">The number to evaluate</param>
    /// <param name="boundsA">The lower bound</param>
    /// <param name="boundsB">The upper bound</param>
    /// <param name="fixRange">Swaps bounds A and B if B < A</param>
    /// <returns></returns>
    public static bool IsBetween(this float number, float boundsA,
        float boundsB, bool fixRange = true)
    {
        if (fixRange)
        {
            float temp = boundsA;

            boundsA = Mathf.Min(boundsA, boundsB);
            boundsB = Mathf.Max(boundsB, temp);
        }

        return boundsA <= number && number <= boundsB;
    }
    #endregion

    #region Sign
    /// <summary>
    /// Returns true if <paramref name="number"/> is greater than 0.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static bool IsPositive(this float number) => number > 0;

    /// <summary>
    /// Returns true if <paramref name="number"/> is less than 0.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static bool IsNegative(this float number) => number < 0;

    /// <summary>
    /// Returns sign of number.
    /// </summary>
    /// <param name="number">The sign of the number. Zero is considered
    /// positive.</param>
    /// <returns>-1 if <paramref name="number"/> is negative, otherwise
    /// 1.</returns>
    public static int Sign(this float number)
    {
        return number < 0 ? -1 : 1;
    }

    /// <inheritdoc cref="Sign(float)"/>
    public static int Sign<N>(this N number) where N : IComparable<int>
    {
        return number.CompareTo(0) < 0 ? -1 : 1;
    }

    /// <summary>
    /// Returns either zero if number is zero or the sign of number if it is
    /// not.
    /// </summary>
    /// <param name="number">The number to evaluate.</param>
    /// <returns>0 if <paramref name="number"/> is zero, the sign of <paramref
    /// name="number"/> otherwise.</returns>
    public static int ZeroOrSign(this float number)
    {
        return number == 0 ? 0 : number.Sign();
    }

    /// <inheritdoc cref="ZeroOrSign(float)"/>
    public static int ZeroOrSign<N>(this N number) where N : IComparable<int>
    {
        return number.CompareTo(0) == 0 ? 0 : number.Sign();
    }
    #endregion

    #region Deltas
    /// <summary>
    /// Returns value such that the change of value is towards target and is no
    /// greater than margin;
    /// </summary>
    /// <param name="value">The value to change.</param>
    /// <param name="target">The number to change towards.</param>
    /// <param name="margin">The maximal change.</param>
    /// <returns></returns>
    public static float GetMinimumChange(this float value, float target,
        float margin)
    {
        return value + Mathf.Sign(target)
            * Mathf.Min(Mathf.Abs(target), Mathf.Abs(margin));
    }
    #endregion

    #region Rounding
    #region Enum
    public enum RoundMode
    {
        /// <summary>
        /// Rounds to the nearest int.
        /// </summary>
        NearestInt,
        /// <summary>
        /// Rounds to the nearest int greater than the value.
        /// </summary>
        Ceiling,
        /// <summary>
        /// Rounds to the nearest int lesser than the value.
        /// </summary>
        Floor,
        /// <summary>
        /// If value is positive, round to the nearest int greater than value.
        /// Else, round to the nearest int lesser than value.
        /// </summary>
        IncreaseAbs,
        /// <summary>
        /// If value is positive, round to the nearest int lesser than value.
        /// Else, round to the nearest int greater than value.
        /// </summary>
        DecreaseAbs
    }
    #endregion

    /// <summary>
    /// Rounds the float using the provided rounding method.
    /// </summary>
    /// <param name="number">The float to round.</param>
    /// <param name="mode">How to round <paramref name="number"/>.</param>
    /// <returns></returns>
    public static int Round(this float number, RoundMode mode = RoundMode.NearestInt)
    {
        switch (mode)
        {
            case RoundMode.NearestInt:
                return Mathf.RoundToInt(number);
            case RoundMode.Ceiling:
                return Mathf.CeilToInt(number);
            case RoundMode.Floor:
                return Mathf.FloorToInt(number);
            case RoundMode.IncreaseAbs:
                if (number < 0)
                    return Mathf.FloorToInt(number);
                else if (number > 0)
                    return Mathf.CeilToInt(number);
                else
                    return 0;
            case RoundMode.DecreaseAbs:
            default:
                if (number > 0)
                    return Mathf.FloorToInt(number);
                else if (number < 0)
                    return Mathf.CeilToInt(number);
                else
                    return 0;
        }
    }
    #endregion

    #region Misc Operations
    /// <summary>
    /// Returns the square of value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float Squared(this float value)
    {
        return value * value;
    }

    /// <summary>
    /// Returns the square of value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Squared(this int value)
    {
        return value * value;
    }
    #endregion
}