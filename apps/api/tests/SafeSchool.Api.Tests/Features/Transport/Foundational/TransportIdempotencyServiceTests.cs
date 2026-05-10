using FluentAssertions;
using SafeSchool.Api.Features.Transport.Common.Idempotency;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Transport.Foundational;

public sealed class TransportIdempotencyServiceTests
{
    [Fact]
    public void RegisterScan_DetectsRetryAndConflict()
    {
        var service = new TransportIdempotencyService();
        var first = service.RegisterScan("school-1", "scan-1", "hash-a", "result-1");
        var retry = service.RegisterScan("school-1", "scan-1", "hash-a", "result-1");
        var conflict = service.RegisterScan("school-1", "scan-1", "hash-b", "result-2");

        first.IsRetry.Should().BeFalse();
        retry.IsRetry.Should().BeTrue();
        conflict.IsConflict.Should().BeTrue();
    }
}
