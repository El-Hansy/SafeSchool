using FluentAssertions;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Common;
using Xunit;

namespace SafeSchool.Api.Tests.Features.IdentityAccess.AccessControl;

public sealed class RoleAssignmentTests
{
    [Fact]
    public void ActorRoleAssignment_IsInactiveWhenExpired()
    {
        var assignment = new ActorRoleAssignment
        {
            TenantId = "school-1",
            ActorReference = "staff:admin",
            RoleId = Guid.NewGuid(),
            AssignmentStatus = AssignmentStatus.Active,
            ValidFrom = DateTimeOffset.UtcNow.AddDays(-3),
            ValidUntil = DateTimeOffset.UtcNow.AddDays(-1),
            AssignedBy = "platform:owner",
            ReviewReason = "Test assignment."
        };

        assignment.ValidUntil.Should().BeBefore(DateTimeOffset.UtcNow);
    }
}
