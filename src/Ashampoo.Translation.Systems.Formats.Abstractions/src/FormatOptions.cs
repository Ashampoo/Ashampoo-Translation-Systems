namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Abstract base record class for format options.
/// </summary>
/// <param name="Name">
/// The name of the option.
/// </param>
/// <param name="Required">
/// Whether the option is required.
/// </param>
public abstract record FormatOption(string Name, bool Required = false);

/// <summary>
/// Format option for a string value.
/// </summary>
/// <param name="Name">
/// <inheritdoc cref="FormatOption"/>
/// </param>
/// <param name="Required">
/// <inheritdoc cref="FormatOption"/>
/// </param>
public record FormatStringOption(string Name, bool Required = false) : FormatOption(Name, Required)
{
    /// <summary>
    /// The value of the option.
    /// </summary>
    public string Value { get; set; } = "";
}

/// <summary>
/// Format option for a char value.
/// </summary>
/// <param name="Name">
/// <inheritdoc cref="FormatOption"/>
/// </param>
/// <param name="Required">
/// <inheritdoc cref="FormatOption"/>
/// </param>
public record FormatCharacterOption(string Name, bool Required = false) : FormatOption(Name, Required)
{
    /// <summary>
    /// The value of the option.
    /// </summary>
    public char Value { get; set; } = ';';
}

/// <summary>
/// Configuration object for a <see cref="IFormat"/> containing a list of <see cref="FormatOption">FormatOptions</see>.
/// </summary>
public record FormatOptions
{
    /// <summary>
    /// The list of <see cref="FormatOption">FormatOptions</see> for the <see cref="IFormat"/>.
    /// </summary>
    public FormatOption[] Options { get; init; } = default!;
    /// <summary>
    /// A <see langword="bool"/> indicating whether the configuration was cancelled.
    /// </summary>
    public bool IsCanceled { get; set; }
}

/// <summary>
/// Callback to configure necessary options for a <see cref="IFormat"/>.
/// </summary>
public delegate Task FormatOptionsCallback(FormatOptions options);