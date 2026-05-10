using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileLocalizationTests
{
    [Theory]
    [InlineData("ar", "rtl")]
    [InlineData("en", "ltr")]
    [InlineData("fr", "ltr")]
    public void Language_service_resolves_supported_direction(string input, string direction)
    {
        MobileLanguageService.DirectionFor(input).Should().Be(direction);
        new MobileLanguageService().Resolve("school-demo", "user-demo", input).LanguageCode.Should().Be(input == "ar" ? "ar" : "en");
    }
}
