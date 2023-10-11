using System;

/// <summary>
/// Extension class for <see cref="Enum"/>.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class EnumExt
{
    #region Enums
    /// <summary>
    /// Evaluates <paramref name="value"/> and determines whether or not all
    /// bits in <paramref name="flags"/> is also set in <paramref
    /// name="value"/>.
    /// </summary>
    /// <typeparam name="TEnum">Any enum type.</typeparam>
    /// <param name="value">The value to test.</param>
    /// <param name="flags">The flags to test against.</param>
    /// <returns>True if all bits in <paramref name="flags"/> are also set in
    /// <paramref name="value"/>, false otherwise.</returns>
    public static bool AllFlagsSet<TEnum>(this TEnum value, TEnum flags)
        where TEnum : Enum
    {
        var valueLong = Convert.ToInt64(value);
        var flagsLong = Convert.ToInt64(flags);

        return (valueLong & flagsLong) == flagsLong;
    }

    /// <summary>
    /// Evaluates <paramref name="value"/> and determines whether or not at
    /// least one bit in <paramref name="flags"/> is also set in its
    /// corresponding location in <paramref name="value"/>.
    /// </summary>
    /// <returns>True if at least one bit in <paramref name="flags"/> is also
    /// set in its corresponding location in <paramref name="value"/>, false
    /// otherwise.</returns>
    /// <inheritdoc cref="AllFlagsSet{TEnum}(TEnum, TEnum)"/>
    public static bool SomeFlagsSet<TEnum>(this TEnum value, TEnum flags)
    {
        var valueLong = Convert.ToInt64(value);
        var flagsLong = Convert.ToInt64(flags);

        return (valueLong & flagsLong) != 0;
    }
    #endregion
}