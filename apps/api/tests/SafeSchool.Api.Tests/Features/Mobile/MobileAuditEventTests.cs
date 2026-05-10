using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileAuditEventTests
{
    [Fact]
    public void Audit_service_records_denied_access_with_reason()
    {
        var audit = new MobileAuditService();
        var evt = audit.Record("school-demo", "user-1", "device-1", "mobile.denied_access", MobileRoleCodes.Student, "mobile.admin", "blocked", "ROLE_ACCESS_DENIED");
        evt.ReasonCode.Should().Be("ROLE_ACCESS_DENIED");
        audit.Events("school-demo").Should().Contain(e => e.Result == "blocked");
    }
}
