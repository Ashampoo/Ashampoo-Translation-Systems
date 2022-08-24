using System.Globalization;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Utility class for guessing the language from a file path.
/// </summary>
public static class LanguageParser
{
    private static readonly Regex Regex = new(@".*((?<language>[a-z]{2,3})[-_](?<country>[a-zA-Z]{2,3}))");
    
    /// <summary>
    /// Try to parse a language from a file path.
    /// </summary>
    /// <param name="filePath">
    /// The file path to parse.
    /// </param>
    /// <returns>
    /// The language parsed from the file path, or null if no language could be parsed.
    /// </returns>
    public static string? TryParseLanguageId(string filePath)
    {
        var filePathComponents = filePath.Split(Path.DirectorySeparatorChar);

        for (var i = filePathComponents.Length - 1; i >= 0; i--)
        {
            var match = Regex.Match(filePathComponents[i]);

            if (!match.Success) continue;

            var targetLanguage = match.Groups["language"].Value;
            var targetCountry = match.Groups["country"].Value;
            var code = $"{targetLanguage}-{targetCountry.ToUpper()}";

            var valid = IsValidLanguageCode(code);

            if (valid) return code;
        }

        return null;
    }
    
    /// <summary>
    /// Check if a language code is valid.
    /// </summary>
    /// <param name="code"></param>
    /// <returns>
    /// True if the language code is valid, false otherwise.
    /// </returns>
    public static bool IsValidLanguageCode(string code)
    {
        var valid = CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture =>
            culture.Name.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        return valid;
    }
}