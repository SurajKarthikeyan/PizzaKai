using System;
/// <summary>
/// Contains methods pertaining to miscellaneous things, usually low level
/// things that don't fit anywhere else.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class MiscExt
{
    /// <summary>
    /// Swaps two values around. No performance benefits, just use if you are
    /// lazy.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a">To be assigned the value of b.</param>
    /// <param name="b">To be assigned the value of a.</param>
    public static void Swap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNumber(this object value)
    {
        return value is sbyte
            || value is byte
            || value is short
            || value is ushort
            || value is int
            || value is uint
            || value is long
            || value is ulong
            || value is float
            || value is double
            || value is decimal;
    }

    /// <summary>
    /// Detects if there is any difference between <paramref name="target"/> and
    /// <paramref name="newValue"/>. If there is, set the value of <paramref
    /// name="target"/> to <paramref name="newValue"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="newValue"></param>
    /// <returns>If <paramref name="target"/> and <paramref name="newValue"/>
    /// are different.</returns>
    public static bool DetectChange<T>(ref T target, T newValue) where T : IEquatable<T>
    {
        if (target.Equals(newValue))
            return false;

        target = newValue;
        return true;
    }
}
