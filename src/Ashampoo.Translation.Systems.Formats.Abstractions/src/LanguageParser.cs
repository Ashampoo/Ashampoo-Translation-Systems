using System.Globalization;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Utility class for guessing the language from a file path.
/// </summary>
public static class LanguageParser
{
    private static readonly Regex LanguageCountryRegex = 
        new(@".*((?<language>[a-z]{2,3})[-_](?<country>[a-zA-Z]{2,3}))");

    private static readonly Regex LanguageScriptTagCountryRegex =
        new(@".*(?<language>[a-z]{2,3})[-_](?<scripttag>[a-zA-Z]{4})[-_](?<country>[a-zA-Z]{2,3})");

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
            var filePathComponent = filePathComponents[i];
            var code = TryParseLanguageCountry(filePathComponent);
            code ??= TryParseLanguageScriptTagCountry(filePathComponent);

            if (code is not null) return code;
        }

        return null;
    }

    private static string? TryParseLanguageCountry(string filePath)
    {
        var match = LanguageCountryRegex.Match(filePath);

        if (!match.Success) return null;

        var targetLanguage = match.Groups["language"].Value;
        var targetCountry = match.Groups["country"].Value;
        var code = $"{targetLanguage}-{targetCountry.ToUpper()}";

        var valid = IsValidLanguageCode(code);

        return valid ? code : null;
    }

    private static string? TryParseLanguageScriptTagCountry(string filePath)
    {
        var match = LanguageScriptTagCountryRegex.Match(filePath);

        if (!match.Success) return null;

        var targetLanguage = match.Groups["language"].Value;
        var targetScriptTag = match.Groups["scripttag"].Value;
        var targetCountry = match.Groups["country"].Value;
        var code = $"{targetLanguage}-{targetScriptTag}-{targetCountry.ToUpper()}";

        var valid = IsValidLanguageCode(code);

        return valid ? code : null;
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