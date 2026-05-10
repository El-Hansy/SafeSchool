using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileDomainModelTests
{
    [Fact]
    public void Mobile_domain_covers_all_phase12_roles()
    {
        MobileRoleCodes.All.Should().Contain([
            MobileRoleCodes.Guardian,
            MobileRoleCodes.Student,
            MobileRoleCodes.TransportDriver,
            MobileRoleCodes.GateAccess,
            MobileRoleCodes.CanteenCashier,
            MobileRoleCodes.Teacher,
            MobileRoleCodes.MedicalStaff,
            MobileRoleCodes.ComplaintHandler,
            MobileRoleCodes.CommunicationSender,
            MobileRoleCodes.DocumentAdministrator,
            MobileRoleCodes.SchoolAdministrator,
            MobileRoleCodes.PlatformSupport
        ]);
        MobileSeedCatalog.Workspaces.Should().HaveCount(12);
    }
}
