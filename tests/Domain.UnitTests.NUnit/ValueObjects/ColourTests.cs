using RssFeeder.Domain.Exceptions;
using RssFeeder.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace RssFeeder.Domain.UnitTests.ValueObjects;

public class ColourTests
{
    [Test, Category("Skip")]
    public void ShouldReturnCorrectColourCode()
    {
        var code = "#FFFFFF";

        var colour = Colour.From(code);

        colour.Code.Should().Be(code);
    }

    [Test, Category("Skip")]
    public void ToStringReturnsCode()
    {
        var colour = Colour.White;

        colour.ToString().Should().Be(colour.Code);
    }

    [Test, Category("Skip")]
    public void ShouldPerformImplicitConversionToColourCodeString()
    {
        string code = Colour.White;

        code.Should().Be("#FFFFFF");
    }

    [Test, Category("Skip")]
    public void ShouldPerformExplicitConversionGivenSupportedColourCode()
    {
        var colour = (Colour)"#FFFFFF";

        colour.Should().Be(Colour.White);
    }

    [Test, Category("Skip")]
    public void ShouldThrowUnsupportedColourExceptionGivenNotSupportedColourCode()
    {
        FluentActions.Invoking(() => Colour.From("##FF33CC"))
            .Should().Throw<UnsupportedColourException>();
    }
}
