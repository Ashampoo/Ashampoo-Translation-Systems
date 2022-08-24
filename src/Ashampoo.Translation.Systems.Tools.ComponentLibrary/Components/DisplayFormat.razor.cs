using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Tools.ComponentLibrary.Services;
using Microsoft.AspNetCore.Components;

namespace Ashampoo.Translation.Systems.Tools.ComponentLibrary.Components;

public partial class DisplayFormat : ComponentBase
{
    [Parameter] public IFormat Format { get; set; } = default!;

    [Inject] private IFormatService FormatService { get; init; } = default!;

    private string sourceLanguage = default!;
    private string targetLanguage = default!;

    private string searchString = "";
    private bool filterForEmptyTranslations;
    private ITranslationUnit selectedTranslation = default!;
    private (string, string) backupTranslations = ("", "");

    /// <summary>
    /// Get the source and target languages from the <see cref="IFormat" /> instance in <paramref name="parameters" />.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.TryGetValue<IFormat>(nameof(Format), out var format))
        {
            if (format is not null)
            {
                if (format.Header.SourceLanguage is null && format.LanguageCount != FormatLanguageCount.OnlyTarget)
                    throw new ArgumentNullException(nameof(format.Header.SourceLanguage));

                sourceLanguage = format.Header.SourceLanguage ?? "";
                targetLanguage = format.Header.TargetLanguage;
            }
        }

        await base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Save the original translations before editing.
    /// </summary>
    /// <param name="element"></param>
    private void BackupUnit(object element)
    {
        if (element is not ITranslationUnit unit) throw new ArgumentException("Expected ITranslationUnit.");

        var source = (unit.TryGet(sourceLanguage) as ITranslationString)?.Value ?? "";
        var target = (unit[targetLanguage] as ITranslationString)?.Value ?? "";

        backupTranslations = (source, target);
    }

    /// <summary>
    /// Reset the current changes.
    /// </summary>
    /// <param name="element"></param>
    private void ResetTranslations(object element)
    {
        if (element is ITranslationUnit translationUnit)
        {
            if (!string.IsNullOrWhiteSpace(sourceLanguage))
                translationUnit.AsTranslationString(sourceLanguage).Value = backupTranslations.Item1;

            translationUnit.AsTranslationString(targetLanguage).Value = backupTranslations.Item2;
        }
        else
            throw new ArgumentException("Expected ITranslationUnit.");
    }

    /// <summary>
    /// Change the languages of translations.
    /// </summary>
    /// <param name="oldLanguage"></param>
    /// <returns></returns>
    private async Task SwitchLanguage(string oldLanguage)
    {
        await FormatService.SwitchLanguageAsync(Format, oldLanguage);

        sourceLanguage = Format.Header.SourceLanguage ?? "";
        targetLanguage = Format.Header.TargetLanguage;
    }

    /// <summary>
    /// Required filter function from MudBlazor
    /// </summary>
    /// <param name="unit"></param>
    /// <returns><see langword="true"/> if the <see cref="ITranslationUnit.Id"/> or one of the translations contains the search string;
    /// otherwise <see langword="false"/>
    /// </returns>
    private bool FilterFunc1(ITranslationUnit unit) => FilterFunc(unit, searchString);

    /// <summary>
    /// Filter function for table.
    /// </summary>
    /// <param name="unit">
    /// The translation unit to filter.
    /// </param>
    /// <param name="search">
    /// The search string.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="ITranslationUnit.Id"/> or one of the translations contains <paramref name="search" /> ;
    /// otherwise <see langword="false"/>.
    /// </returns>
    private bool FilterFunc(ITranslationUnit unit, string search)
    {
        if (filterForEmptyTranslations)
            return string.IsNullOrWhiteSpace((unit.TryGet(targetLanguage) as ITranslationString)?.Value);

        if (unit.Id.Contains(search)) return true;
        if ((unit.TryGet(sourceLanguage) as ITranslationString)?.Value.Contains(search) ?? false) return true;
        return (unit.TryGet(targetLanguage) as ITranslationString)?.Value.Contains(search) ?? false;
    }
}

public static class TranslationExtensions
{
    public static ITranslationString AsTranslationString(this ITranslationUnit unit, string language)
    {
        var translation = unit[language];
        return (ITranslationString)translation;
    }
}