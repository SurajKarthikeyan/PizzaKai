
using UnityEngine;
/// <summary>
/// Contains methods pertaining to C# floats, doubles, and ints.
/// </summary>
public static class NumericalExt
{
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
    /// Returns true if number is in between bounds A and B, inclusive
    /// </summary>
    /// <param name="number">The number to evaluate</param>
    /// <param name="boundsA">The lower bound</param>
    /// <param name="boundsB">The upper bound</param>
    /// <param name="fixRange">Swaps bounds A and B if B < A</param>
    /// <returns></returns>
    public static bool IsBetween(this float number, float boundsA, float boundsB, bool fixRange = true)
    {
        if (fixRange)
        {
            float temp = boundsA;

            boundsA = Mathf.Min(boundsA, boundsB);
            boundsB = Mathf.Max(boundsB, temp);
        }

        return (boundsA <= number && number <= boundsB);
    }

    /// <summary>
    /// Returns sign of number.
    /// </summary>
    /// <param name="number">The sign of the number. Zero is considered positive.</param>
    /// <returns></returns>
    public static int Sign(this float number)
    {
        return number < 0 ? -1 : 1;
    }

    /// <summary>
    /// Returns either zero if number is zero or the sign of number if it is not.
    /// </summary>
    /// <param name="number">The number to evaluate.</param>
    /// <returns>Zero if number is zero, the sign of number otherwise.</returns>
    public static int ZeroOrSign(this float number)
    {
        return number == 0 ? 0 : number.Sign();
    }

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
}