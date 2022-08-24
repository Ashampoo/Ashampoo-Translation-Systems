namespace Ashampoo.Translation.Systems.Formats.Abstractions;

public abstract record FormatOption(string Name, bool Required = false);

public record FormatStringOption(string Name, bool Required = false) : FormatOption(Name, Required)
{
    public string Value { get; set; } = "";
}

public record FormatFilterOption(string Name, bool Required = false) : FormatOption(Name, Required)
{
    public bool SetFilter { get; set; } = Required;
}

public record FormatOptions
{
    public FormatOption[] Options { get; init; } = default!;
    public bool IsCanceled { get; set; }
}
public delegate Task FormatOptionsCallback(FormatOptions options);