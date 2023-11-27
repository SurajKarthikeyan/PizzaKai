using System.Text.RegularExpressions;

/// <summary>
/// Contains methods that extend strings and Regex.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class StringExt
{
    #region Variables
    private static readonly Regex memberPrefixRx = new("^((m?_)|k| +)([A-Z])",
        RegexOptions.Compiled);

    private static readonly Regex capitalLetterRx = new("([A-Z])",
        RegexOptions.Compiled);
    #endregion


    #region Methods
    /// <summary>
    /// Replicates <see
    /// href="https://docs.unity3d.com/ScriptReference/ObjectNames.NicifyVariableName.html"/>.
    /// </summary>
    /// <param name="str">The input string to nicify.</param>
    /// <returns>A nicified string</returns>
    public static string Nicify(this string str)
    {
        // First replace the member, underscore, and k prefixes (and also whitespace).
        str = memberPrefixRx.Replace(str, "$3").Trim();

        // Make first character uppercase.
        str = str.FirstCharUppercase();

        // Add spaces.
        str = capitalLetterRx.Replace(str, " $1");
        return str.Trim();
    }

    /// <summary>
    /// Returns a string with the first letter capitalized.
    /// </summary>
    /// <param name="str">The string input.</param>
    /// <returns>A string with the first letter capitalized.</returns>
    public static string FirstCharUppercase(this string str)
    {
        if (str.Length > 0)
        {
            return char.ToUpper(str[0]) + str[1..];
        }

        return "";
    }
    #endregion
}