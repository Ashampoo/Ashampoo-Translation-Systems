using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.Json.Tests;

public class FormatTest : FormatTestBase<JsonFormat>
{
    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        Assert.NotNull(format);
        Assert.Empty(format.TranslationUnits);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(string.Empty, format.Header.TargetLanguage);
        Assert.Equal(LanguageSupport.OnlyTarget, format.LanguageSupport);
    }


    [Fact]
    public void ReadFromFile()
    {
        IFormat format = CreateAndReadFromFile("en-us.json", new FormatReadOptions { TargetLanguage = "en-US" });

        Assert.Equal(301, format.TranslationUnits.Count);
        Assert.Equal("en-US", format.Header.TargetLanguage);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal("Save", format.TranslationUnits.GetTranslationUnit("settings/save").Translations.GetTranslation("en-US").Value);
    }

    [Fact]
    public void ReadAndWrite()
    {
        IFormat format = CreateAndReadFromFile("de-DE.json", new FormatReadOptions { TargetLanguage = "de-DE" });

        var ms = new MemoryStream();
        format.Write(ms);
        ms.Seek(0, SeekOrigin.Begin);

        // var fs = createFileInStream("de-DE.json");

        //FIXME: compare formats like in the other tests, and not the streams!
        //fs.MustBeEqualTo(ms);
    }

    [Theory]
    [InlineData("json-value-kind-false-test.json")]
    [InlineData("json-value-kind-true-test.json")]
    [InlineData("json-value-kind-null-test.json")]
    [InlineData("json-value-kind-number-test.json")]
    public async Task InvalidValueKindsTest(string filename)
    {
        var options = new FormatReadOptions { TargetLanguage = "de-DE" };
        var exception =
            await Assert.ThrowsAsync<JsonException>(async () => await CreateAndReadFromFileAsync(filename, options));
        Assert.Equal("Array element must be either an object, array or a string.", exception.Message);
    }

    [Fact]
    public async Task ReadAndWriteWithReorder()
    {
        var format = await CreateAndReadFromFileAsync("de-DE.json", new FormatReadOptions { TargetLanguage = "de-DE" });

        var units = format.TranslationUnits.OrderBy(u => u.Id);
        format = new JsonFormat { Header = format.Header };
        foreach (var unit in units)
        {
            format.TranslationUnits.Add(unit);
        }

        var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
    }
    
    [Fact]
    public async Task OptionsCallbackCancelledTest()
    {
        Task OptionsCallback(FormatOptions options)
        {
            options.IsCanceled = true;
            return Task.CompletedTask;
        }

        var format = await CreateAndReadFromFileAsync("en-us.json",
            new FormatReadOptions { FormatOptionsCallback = OptionsCallback });
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(string.Empty, format.Header.TargetLanguage);
        Assert.Empty(format.TranslationUnits);
    }

    [Fact]
    public async Task NoOptionsCallbackTest()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await CreateAndReadFromFileAsync("en-us.json", new FormatReadOptions()));
        Assert.Equal("Value cannot be null. (Parameter 'FormatOptionsCallback')", exception.Message);
        Assert.Equal("FormatOptionsCallback", exception.ParamName);
    }

    [Fact]
    public async Task OptionsCallbackTest()
    {
        Task OptionsCallback(FormatOptions options)
        {
            (options.Options[0] as FormatStringOption)!.Value = "de-DE";
            return Task.CompletedTask;
        }

        var options = new FormatReadOptions { FormatOptionsCallback = OptionsCallback };
        var format = await CreateAndReadFromFileAsync("de-DE.json", options);

        Assert.Equal("de-DE", format.Header.TargetLanguage);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(189, format.TranslationUnits.Count);
    }
}