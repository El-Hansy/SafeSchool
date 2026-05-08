using FluentAssertions;
using SafeSchool.Api.Features.AttendanceAccess.Common.Idempotency;
using Xunit;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Foundational;

public sealed class IdempotencyServiceTests
{
    [Fact]
    public void Register_DetectsRetryAndConflict()
    {
        var service = new IdempotencyService();
        service.Register("school-1", "batch-1", "hash-a", "result-1").IsRetry.Should().BeFalse();
        service.Register("school-1", "batch-1", "hash-a", "result-1").IsRetry.Should().BeTrue();
        service.Register("school-1", "batch-1", "hash-b", "result-2").IsConflict.Should().BeTrue();
    }
}

