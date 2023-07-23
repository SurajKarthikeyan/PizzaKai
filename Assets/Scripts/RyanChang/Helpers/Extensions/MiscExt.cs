/// <summary>
/// Contains methods pertaining to miscellaneous things.
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
}
