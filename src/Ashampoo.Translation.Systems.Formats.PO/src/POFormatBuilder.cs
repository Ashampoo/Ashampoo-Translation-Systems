using Ashampoo.Translation.Systems.Formats.Abstractions;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of the <see cref="IFormatBuilderWithTarget"/> for the PO format.
/// </summary>
public class POFormatBuilder : IFormatBuilderWithTarget
{
    private string? targetLanguage;
    private readonly Dictionary<string, string> translations = new();
    private const string Divider = "/"; // TODO: move to interface?

    public void Add(string id, string target)
    {
        translations.Add(id, target);
    }

    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(targetLanguage, nameof(targetLanguage));

        //Create new PO format and add translations
        var poFormat = new POFormat
        {
            Header =
            {
                TargetLanguage = targetLanguage
            }
        };

        foreach (var translation in translations)
        {
            var translationUnit = new TranslationUnit(translation.Key);
            var index = translation.Key.LastIndexOf(Divider, StringComparison.Ordinal); 
            if (index > 0) // if divider exists, then a message context is used
            {
                var ctxt = translation.Key[..index];
                var msgId = translation.Key[(index + 1)..];
                translationUnit.Add(new MessageString(id: msgId, value: translation.Value, language: targetLanguage,
                    msgCtxt: ctxt));
            }
            else
                translationUnit.Add(new MessageString(translation.Key, translation.Value, targetLanguage));

            poFormat.Add(translationUnit);
        }

        return poFormat;
    }

    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
    }
}