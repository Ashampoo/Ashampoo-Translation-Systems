using Ashampoo.Translations.Formats.Abstractions.Translation;
using Ashampoo.Translations.Formats.Abstractions.TranslationFilter;
using Microsoft.Toolkit.Diagnostics;

namespace Ashampoo.Translations.Formats.Abstractions;

/// <summary>
/// Provides extension methods for the <see cref="IFormat"/> interface.
/// </summary>
public static class FormatExtensions
{
    /// <summary>
    /// Validates if the generic is implemented by the format and returns it or throws an exception otherwise.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="format"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedFormatException"></exception>
    public static T GetRequired<T>(this IFormat format)
    {
        if (format is T result) return result;
        throw new UnsupportedFormatException(format,
            $"Interface '{typeof(T).FullName}' not supported by format '{format.GetType().FullName}'");
    }

    /// <summary>
    /// Imports translations from another format into the calling one
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatToImport"></param>
    /// <returns></returns>
    public static IList<ITranslation> ImportFrom(this IFormat format, ITranslationUnits formatToImport)
    {
        List<ITranslation> imported = new();
        foreach (var translationUnit in formatToImport)
        {
            foreach (var translation in translationUnit)
            {
                var id = translationUnit.Id;
                var language = translation.Language;
                var value = (translation as ITranslationString)?.Value;
                if (value is null) throw new Exception("Expected translation string");

                if (format[id]?.TryGet(language) is not ITranslationString translationString) continue;
                if (translationString.Value.Equals(value)) continue;

                translationString.Value = value;
                imported.Add(translation);
            }
        }

        return imported;
    }

    /// <summary>
    /// Convert current instance of IFormat into the IFormat specified by the type.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="type"></param>
    /// <param name="formatFactory"></param>
    /// <param name="options"></param>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <returns>A new instance of <see cref="IFormat"/>.</returns>
    public static IFormat ConvertTo(this IFormat format, Type type, IFormatFactory formatFactory,
        AssignOptions options)
    {
        return ConvertToAsync(format, type, formatFactory, options).Result;
    }

    /// <summary>
    /// Convert current instance of IFormat into the IFormat specified by the type.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="type"></param>
    /// <param name="formatFactory"></param>
    /// <param name="options"></param>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
    public static async Task<IFormat> ConvertToAsync(this IFormat format, Type type, IFormatFactory formatFactory,
        AssignOptions options)
    {
        var formatBuilder = formatFactory.GetFormatProvider(type).GetFormatBuilder();
        var formatToConvertTo = formatBuilder switch
        {
            IFormatBuilderWithTarget targetBuilder => await BuildWithTarget(format, targetBuilder, options),
            IFormatBuilderWithSourceAndTarget targetAndSourceBuilder => await BuildWithSourceAndTarget(format,
                targetAndSourceBuilder, options),
            _ => throw new NotImplementedException()
        };

        return formatToConvertTo;
    }

    /// <summary>
    /// Convert current instance of IFormat into the IFormat specified by the type.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatFactory"></param>
    /// <param name="options"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <returns>A new instance of IFormat.</returns>
    public static T ConvertTo<T>(this IFormat format, IFormatFactory formatFactory,
        AssignOptions options) where T : IFormat
    {
        var converted = ConvertToAsync(format, typeof(T), formatFactory, options).Result;
        if (converted is not T result) throw new ArgumentException("Expected format of type " + typeof(T).FullName);

        return result;
    }

    /// <summary>
    /// Convert current instance of IFormat into the IFormat specified by the type.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatFactory"></param>
    /// <param name="options"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
    public static async Task<T> ConvertToAsync<T>(this IFormat format, IFormatFactory formatFactory,
        AssignOptions options) where T : IFormat
    {
        var converted = await ConvertToAsync(format, typeof(T), formatFactory, options);
        if (converted is not T result) throw new ArgumentException("Expected format of type " + typeof(T).FullName);

        return result;
    }

    private static async Task<IFormat> BuildWithTarget
    (
        IFormat format,
        IFormatBuilderWithTarget formatBuilder,
        AssignOptions options
    )
    {
        if (!await ConfigureOptions(false, options)) throw new InvalidOperationException();

        var targetLanguage = options.TargetLanguage;
        Guard.IsNotNullOrWhiteSpace(targetLanguage, nameof(targetLanguage));
        formatBuilder.SetTargetLanguage(targetLanguage);

        var filter = options.Filter ?? new DefaultTranslationFilter();

        foreach (var unit in format)
        {
            if (!filter.IsValid(unit)) continue;

            var id = unit.Id;
            var translationString = unit.TryGet(targetLanguage) as ITranslationString;

            formatBuilder.Add(id, translationString?.Value ?? string.Empty);
        }

        return formatBuilder.Build();
    }

    private static async Task<IFormat> BuildWithSourceAndTarget
    (
        IFormat format,
        IFormatBuilderWithSourceAndTarget formatBuilder,
        AssignOptions options
    )
    {
        if (!await ConfigureOptions(false, options)) throw new InvalidOperationException();
        var filter = options.Filter ?? new DefaultTranslationFilter();

        var targetLanguage = options.TargetLanguage;
        var sourceLanguage = options.SourceLanguage;

        Guard.IsNotNullOrWhiteSpace(targetLanguage, nameof(targetLanguage));
        Guard.IsNotNullOrWhiteSpace(sourceLanguage, nameof(sourceLanguage));

        formatBuilder.SetTargetLanguage(targetLanguage);
        formatBuilder.SetSourceLanguage(sourceLanguage);

        foreach (var unit in format)
        {
            if (!filter.IsValid(unit)) continue;

            var id = unit.Id;
            var targetTranslationString = unit.TryGet(targetLanguage) as ITranslationString;
            var sourceTranslationString = unit.TryGet(sourceLanguage) as ITranslationString;

            formatBuilder.Add(id, sourceTranslationString?.Value ?? string.Empty,
                targetTranslationString?.Value ?? string.Empty);
        }

        return formatBuilder.Build();
    }

    private static async Task<bool> ConfigureOptions(bool sourceAndTarget, AssignOptions? assignOptions)
    {
        var setTargetLanguage = string.IsNullOrWhiteSpace(assignOptions?.TargetLanguage);
        var setSourceLanguage = string.IsNullOrWhiteSpace(assignOptions?.SourceLanguage) && sourceAndTarget;
        var setFilter = assignOptions?.Filter is null;

        if (!setTargetLanguage && !setSourceLanguage && !setFilter) return true;

        if (assignOptions?.FormatOptionsCallback is null)
            throw new InvalidOperationException("Callback for Format options required.");

        FormatStringOption targetLanguageOption = new("Target language", true);
        FormatStringOption sourceLanguageOption = new("Source language", true);
        FormatFilterOption onlyUntranslated = new("Only untranslated");

        List<FormatOption> optionList = new();

        if (setSourceLanguage) optionList.Add(sourceLanguageOption);
        if (setTargetLanguage) optionList.Add(targetLanguageOption);
        if (setFilter) optionList.Add(onlyUntranslated);

        var formatOptions = new FormatOptions
        {
            Options = optionList.ToArray()
        };

        await assignOptions.FormatOptionsCallback.Invoke(formatOptions);
        if (formatOptions.IsCanceled) return false;

        if (setTargetLanguage) assignOptions.TargetLanguage = targetLanguageOption.Value;
        if (setSourceLanguage) assignOptions.SourceLanguage = sourceLanguageOption.Value;
        if (setFilter)
            assignOptions.Filter = onlyUntranslated.SetFilter
                ? new IsEmptyTranslationFilter(targetLanguageOption.Value)
                : assignOptions.Filter;

        return true;
    }
}