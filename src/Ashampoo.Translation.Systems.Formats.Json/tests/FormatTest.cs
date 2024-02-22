using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.Json.Tests;

public class FormatTest : FormatTestBase<JsonFormat>
{
    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        format.Should().NotBeNull();
        format.TranslationUnits.Should().BeEmpty();
        format.Header.SourceLanguage.Should().BeNull();
        format.Header.TargetLanguage.Value.Should().BeEmpty();
        format.LanguageSupport.Should().Be(LanguageSupport.OnlyTarget);
    }


    [Fact]
    public void ReadFromFile()
    {
        IFormat format = CreateAndReadFromFile("en-us.json", new FormatReadOptions { TargetLanguage = new Language("en-US") });
        
        format.TranslationUnits.Should().HaveCount(301);
        format.Header.TargetLanguage.Should().Be(new Language("en-US"));
        format.Header.SourceLanguage.Should().BeNull();
        format.TranslationUnits.GetTranslationUnit("settings/save").Translations.GetTranslation(new Language("en-US")).Value.Should().Be("Save");
    }

    [Fact]
    public void ReadAndWrite()
    {
        IFormat format = CreateAndReadFromFile("de-DE.json", new FormatReadOptions { TargetLanguage = new Language("de-DE") });

        var ms = new MemoryStream();
        format.Write(ms);
        ms.Seek(0, SeekOrigin.Begin);

        // var fs = createFileInStream("de-DE.json");

        //FIXME: compare formats like in the other tests, and not the streams!
        //fs.MustBeEqualTo(ms);
    }
    
    [Fact]
    public async Task ReadAndWriteAsync()
    {
        var format = await CreateAndReadFromFileAsync("de-DE.json", new FormatReadOptions { TargetLanguage = new Language("de-DE") });

        var ms = new MemoryStream();
        await format.WriteAsync(ms);
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
        var options = new FormatReadOptions { TargetLanguage = new Language("de-DE") };
        var exception =
            await Assert.ThrowsAsync<JsonException>(async () => await CreateAndReadFromFileAsync(filename, options));
        exception.Message.Should().Be("Array element must be either an object, array or a string.");
    }

    [Fact]
    public async Task ReadAndWriteWithReorder()
    {
        var format = await CreateAndReadFromFileAsync("de-DE.json", new FormatReadOptions { TargetLanguage = new Language("de-DE") });

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
        format.Header.SourceLanguage.Should().BeNull();
        format.Header.TargetLanguage.Value.Should().BeEmpty();
        format.TranslationUnits.Should().BeEmpty();
    }

    [Fact]
    public async Task NoOptionsCallbackTest()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await CreateAndReadFromFileAsync("en-us.json", new FormatReadOptions()));
        exception.Message.Should().Be("Value cannot be null. (Parameter 'FormatOptionsCallback')");
        exception.ParamName.Should().Be("FormatOptionsCallback");
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

        format.Header.TargetLanguage.Should().Be(new Language("de-DE"));
        format.Header.SourceLanguage.Should().BeNull();
        format.TranslationUnits.Should().HaveCount(189);
    }
    
    [Fact]
    public void WriteFormatLeavesStreamOpen()
    {
        var format = CreateAndReadFromFile("de-DE.json", new FormatReadOptions { TargetLanguage = new Language("de-DE") });
        
        var memoryStream = new MemoryStream();
        format.Write(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }
    
    [Fact]
    public async Task WriteFormatLeavesStreamOpenAsync()
    {
        var format = await CreateAndReadFromFileAsync("de-DE.json", new FormatReadOptions { TargetLanguage = new Language("de-DE") });
        
        var memoryStream = new MemoryStream();
        await format.WriteAsync(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }
}