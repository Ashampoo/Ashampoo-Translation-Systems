using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.Json;

/// <summary>
/// Implementation of <see cref="IFormat"/> interface, for JSON files.
/// </summary>
public class JsonFormat : IFormat
{
    /// <inheritdoc />
    public IFormatHeader Header { get; init; } = new DefaultFormatHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.OnlyTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

    private const string Divider = "/";

    private static readonly Regex ArrayIdentifierRegex = new(@"\[\d+\]");

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        var configureSuccess = await ConfigureOptionsAsync(options); // Configure options
        if (!configureSuccess)
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage.Value, nameof(Header.TargetLanguage)); //Target language is required

        var root = await JsonSerializer.DeserializeAsync<JsonElement>(stream); // Deserialize JSON
        Parse(root); // Parse JSON to TranslationUnits
    }

    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions? options)
    {
        if (string.IsNullOrWhiteSpace(options?.TargetLanguage.Value))
        {
            ArgumentNullException.ThrowIfNull(options?.FormatOptionsCallback,
                nameof(options.FormatOptionsCallback)); // Format options callback is required

            FormatStringOption targetLanguageOption = new("Target language", true);
            FormatOptions formatOptions = new()
            {
                Options =
                [
                    targetLanguageOption
                ]
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // Invoke callback to get format options
            if (formatOptions.IsCanceled) return false; // If user cancelled, return false

            Header.TargetLanguage = Language.Parse(targetLanguageOption.Value);
        }
        else
        {
            Header.TargetLanguage = (Language)options.TargetLanguage!;
        }

        return true;
    }

    private void Parse(JsonElement element)
    {
        Parse("", element); //Parse root element
    }

    private void Parse(string id, JsonElement element)
    {
        //TODO: handle all value kinds
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                ParseObject(id, element);
                break;
            case JsonValueKind.Array:
                ParseArray(id, element);
                break;
        }
    }

    private void ParseObject(string id, JsonElement element)
    {
        foreach (var property in element.EnumerateObject())
        {
            var nextId =
                id.Length == 0
                    ? property.Name
                    : $"{id}{Divider}{property.Name}"; // Create the id, because it's an object, divider indicates a child element
            if (property.Value.ValueKind == JsonValueKind.String)
            {
                var translationString = new DefaultTranslationString // Create translation string
                (
                    property.Value.GetString() ?? throw new ArgumentNullException(nameof(property.Value)),
                    Header.TargetLanguage,
                    []
                );

                var translationUnit = new DefaultTranslationUnit(nextId) // Create translation unit
                {
                    Translations =
                    {
                        translationString
                    }
                };

                TranslationUnits.Add(translationUnit); // Add translation unit to list
            }
            else
                Parse(nextId, property.Value); // Parse next element
        }
    }

    private void ParseArray(string id, JsonElement element)
    {
        for (var i = 0; i < element.GetArrayLength(); i++)
        {
            var nextId =
                id.Length == 0
                    ? $"[{i}]"
                    : $"{id}{Divider}[{i}]"; // Create the id, because it's an array, divider indicates a child element, index indicates the array index
            switch (element[i].ValueKind)
            {
                case JsonValueKind.Object:
                    Parse(nextId, element[i]);
                    break;
                case JsonValueKind.String:
                {
                    var translationString = new DefaultTranslationString // Create translation string
                    (
                        element[i].GetString() ?? throw new ArgumentNullException(nameof(element)),
                        Header.TargetLanguage,
                        []
                    );

                    var translationUnit = new DefaultTranslationUnit(nextId)
                    {
                        Translations =
                        {
                            translationString
                        }
                    }; 
                    TranslationUnits.Add(translationUnit); // Add translation unit to list
                    break;
                }
                case JsonValueKind.Array:
                    ParseArray(nextId, element[i]);
                    break;
                default:
                    throw new JsonException("Array element must be either an object, array or a string.");
            }
        }
    }

    /// <inheritdoc />
    public void Write(Stream stream)
    {
        WriteAsync(stream).Wait();
    }

    /// <summary>
    /// Asynchronously writes the format to the specified stream.
    /// </summary>
    /// <param name="stream">
    /// The stream to write to.
    /// </param>
    public async Task WriteAsync(Stream stream)
    {
        var root = new JsonObject();
        CreateJsonObjects(root); // Create JSON objects from TranslationUnits

        var serializerOptions = new JsonSerializerOptions // Create JSON serializer options
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        await JsonSerializer.SerializeAsync(stream, root, serializerOptions);
        await stream.FlushAsync();
    }

    private void CreateJsonObjects(JsonObject obj)
    {
        foreach (var unit in TranslationUnits)
        {
            var id = unit.Id;
            var index = id.IndexOf(Divider,
                StringComparison
                    .Ordinal); // Check if id contains a divider, divider is used to indicate nested elements
            var front = index > 0 ? id[..index] : id;
            var trailing =
                index > 0 ? id[(index + Divider.Length)..] : ""; // Get trailing part of id, if id contains a divider

            CreateJsonObject(front, trailing, obj, unit); // Create JSON object for current TranslationUnit
        }
    }

    private void CreateJsonObject(string id, string trailing, JsonObject obj, ITranslationUnit unit)
    {
        if (trailing.Length == 0) // If trailing is empty, element has no nested elements
        {
            var value = unit.Translations.GetTranslation(Header.TargetLanguage).Value ??
                        throw new ArgumentNullException(nameof(ITranslation.Value));
            var jsonValue = JsonValue.Create(value); // create json element from value
            obj.Add(id, jsonValue);
            return;
        }

        var index = trailing.IndexOf(Divider, StringComparison.Ordinal); // get index of next divider
        var front = index > 0 ? trailing[..index] : trailing; // front part of trailing is the id for the next element
        var newTrailing = index switch // trailing part of trailing is the trailing part of the next element
        {
            > 0 => trailing[(index + Divider.Length)..],
            _ => string.Empty
        };

        if (ArrayIdentifierRegex.IsMatch(front)) // Check if next element is an array item
        {
            // Next element is an array item, so current element is an array

            foreach (var (key, value) in obj)
            {
                if (key != id) continue; // find the array element with the same id as the current element

                var array = value?.AsArray() ?? throw new Exception("Expected array."); // get the array
                CreateJsonArray(front, newTrailing, array, unit);
                return;
            }

            var newArray = new JsonArray(); // Array element doesn't exist, create new array
            CreateJsonArray(front, newTrailing, newArray, unit);
            obj.Add(id, newArray);
            return;
        }

        // Next element is an object element

        foreach (var pair in obj) // Check if object element with the same id exists
        {
            if (pair.Key != id) continue;

            var jsonObj = pair.Value?.AsObject() ?? throw new Exception("Unexpected object.");
            CreateJsonObject(front, newTrailing, jsonObj, unit);
            return;
        }

        // Object element doesn't exist, create new object

        var newJsonObj = new JsonObject();
        CreateJsonObject(front, newTrailing, newJsonObj, unit);
        obj.Add(id, newJsonObj);
    }

    private void CreateJsonArray(string id, string trailing, JsonArray array, ITranslationUnit unit)
    {
        while (true) // Avoid recursion
        {
            if (trailing.Length == 0) // Current element does not have nested elements
            {
                var value = unit.Translations.GetTranslation(Header.TargetLanguage).Value ??
                            throw new ArgumentNullException(nameof(ITranslation.Value));
                var jsonValue = JsonValue.Create(value);
                array.Add(jsonValue);
                return;
            }

            // Current element has nested elements

            var position = int.Parse(id.Trim('[', ']')); // Get array index from id
            var index = trailing.IndexOf(Divider, StringComparison.Ordinal); // Get index of next divider
            var front = index > 0
                ? trailing[..index]
                : trailing; // Front part of trailing is the id for the next element
            var newTrailing =
                index > 0
                    ? trailing[(index + Divider.Length)..]
                    : ""; // Trailing part of trailing is the trailing part of the next element

            var element = array.ElementAtOrDefault(position); // Get array element at position

            switch (element)
            {
                case JsonObject jsonObj: // Next element is an object element
                    CreateJsonObject(front, newTrailing, jsonObj, unit);
                    break;
                case JsonArray jsonArray: // Next element is an array element
                    id = front;
                    trailing = newTrailing;
                    array = jsonArray;
                    continue; // Parse next element
                case null: // Element doesn't exist, create new element
                {
                    var newJsonObj = new JsonObject(); // TODO: object oder array?
                    CreateJsonObject(front, newTrailing, newJsonObj, unit);
                    array.Insert(position, newJsonObj);
                    break;
                }
                default:
                    throw new Exception("Unexpected element.");
            }

            break;
        }
    }
}