using SafeSchool.Api.Features.AttendanceAccess.Common;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;

public sealed class FakeGuardianLinkEligibilityProvider : IGuardianLinkEligibilityProvider
{
    public bool IsEligible { get; set; } = true;
    public string Reason { get; set; } = "approved";

    public Task<GuardianLinkEligibility> CheckAsync(string tenantId, string guardianReference, string studentProfileId, CancellationToken cancellationToken = default) =>
        Task.FromResult(new GuardianLinkEligibility(tenantId, guardianReference, studentProfileId, IsEligible, Reason));
}

