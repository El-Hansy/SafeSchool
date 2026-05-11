using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Communications;
using SafeSchool.Api.Infrastructure.Persistence;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Communications;

public sealed class CommunicationFoundationTests
{
    [Fact]
    public async Task Communications_create_recipient_snapshots_without_source_mutation()
    {
        await using var dbContext = CreateDbContext();
        var service = new CommunicationWorkflowService(dbContext);

        CommunicationCapabilities.All.Should().Contain(CommunicationCapabilities.NotificationCenter);
        new CommunicationBoundaryGuard().Allows("complaint_resolution").Should().BeFalse();

        var source = await service.AcceptSourceEventAsync("school-demo", new NotificationSourceEventRequest("complaints", "CMP-1", "status", "req-1"));
        source.Evidence.Should().Contain(["source-read-only", "notification-created"]);

        var duplicate = await service.AcceptSourceEventAsync("school-demo", new NotificationSourceEventRequest("complaints", "CMP-1", "status", "req-1"));
        duplicate.Reference.Should().Be(source.Reference);
        duplicate.Evidence.Should().Contain("idempotency_duplicate");

        var broadcast = await service.PublishBroadcastAsync("school-demo", new BroadcastRequest("Safety", "Update", "guardians", "b1"));
        broadcast.Status.Should().Be("Published");

        dbContext.OperationalCommunications.Should().HaveCount(2);
        dbContext.OperationalCommunicationEvents.Should().Contain(x => x.EventType == "source-read-only");
    }

    private static SafeSchoolDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }
}
