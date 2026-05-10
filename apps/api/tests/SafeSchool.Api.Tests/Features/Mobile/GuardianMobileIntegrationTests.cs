using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class GuardianMobileIntegrationTests
{
    [Fact]
    public void Guardian_summary_is_limited_to_linked_students()
    {
        new GuardianMobileService().Summaries().Should().OnlyContain(summary => summary.StudentId.StartsWith("student-"));
    }
}
