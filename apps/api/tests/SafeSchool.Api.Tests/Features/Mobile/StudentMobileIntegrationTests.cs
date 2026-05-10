using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class StudentMobileIntegrationTests
{
    [Fact]
    public void Student_summary_uses_self_scope()
    {
        new StudentMobileService().Summary().StudentId.Should().Be("student-self");
    }
}
