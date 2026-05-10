using FluentAssertions;
using SafeSchool.Api.Features.Mobile;
using Xunit;

namespace SafeSchool.Api.Tests.Features.Mobile;

public sealed class MobileOfflineQueueTests
{
    [Fact]
    public void Offline_sync_accepts_approved_sources_and_rejects_online_only_sources()
    {
        var service = new MobileOfflineActionSyncService(new OfflinePolicyAdapters(), new MobileAuditService());
        service.Sync(new OfflineActionRequest("school-demo", "device-1", "driver", MobileRoleCodes.TransportDriver, "transport", "scan", "client-1", DateTimeOffset.UtcNow, "{}")).SyncStatus.Should().Be("Accepted");
        service.Sync(new OfflineActionRequest("school-demo", "device-1", "admin", MobileRoleCodes.SchoolAdministrator, "administration", "setting", "client-2", DateTimeOffset.UtcNow, "{}")).SyncStatus.Should().Be("Rejected");
    }
}
