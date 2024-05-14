using System;
using System.Linq;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.PO.Tests._TestFiles_;

public sealed class FormatBuilderTests
{
    [Fact]
    public void BuildEmptyFormatTest()
    {
        var builder = new POFormatBuilder();
        builder.SetTargetLanguage(new Language("de-DE"));
        var format = builder.Build();
        format.TranslationUnits.Should().BeEmpty();
    }

    [Fact]
    public void Build_WithoutTargetLanguage_ThrowsException()
    {
        var builder = new POFormatBuilder();
        var action = () => builder.Build();
        action.Should().Throw<ArgumentNullException>()
            .WithMessage(
                "Parameter \"_targetLanguage\" (string) must not be null or whitespace, was null. (Parameter '_targetLanguage')");
    }

    public void Build_WithMsgContext_Success()
    {
        var builder = new POFormatBuilder();
        builder.Add(
            "{\\\"cxt\\\": \\\"survey_title_html\\\", \\\"id\\\": 518150496, \\\"checksum\\\": \"\n\"3594429271}||User Experience Survey for Our Website",
            "Umfrage zur Nutzererfahrung unserer Website");
        var format = builder.Build();
        var unit = format.TranslationUnits.First();
        var translation = unit.Translations.First();
        translation.Should().BeAssignableTo<MessageString>();

        var message = (MessageString)translation;
        message.MsgCtxt.Should()
            .Be("{\\\"cxt\\\": \\\"survey_title_html\\\", \\\"id\\\": 518150496, \\\"checksum\\\": \"\n\"3594429271}");
        message.MsgId.Should().Be("User Experience Survey for Our Website");
        message.Value.Should().Be("Umfrage zur Nutzererfahrung unserer Website");
        message.Id.Should()
            .Be(
                "{\\\"cxt\\\": \\\"survey_title_html\\\", \\\"id\\\": 518150496, \\\"checksum\\\": \"\n\"3594429271}||User Experience Survey for Our Website");
    }
}