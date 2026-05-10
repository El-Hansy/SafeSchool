using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class GuardianMobileActionTests
{
    [Fact]
    public void Guardian_actions_route_through_source_domain_adapter()
    {
        var response = new GuardianStudentActionRouter(new SourceDomainActionAdapters(new MobileAuditService()))
            .Route("school-demo", MobileRoleCodes.Guardian, new MobileActionRequest(MobileRoleCodes.Guardian, "guardian", "student-amina", "action-1", "{}", DateTimeOffset.UtcNow));
        response.ActionResult.Should().Be("Accepted");
    }
}
