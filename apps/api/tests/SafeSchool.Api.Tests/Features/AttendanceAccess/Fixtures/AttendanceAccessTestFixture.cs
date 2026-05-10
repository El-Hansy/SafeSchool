using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Gates;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Tests.Features.AttendanceAccess.Fixtures;

public sealed class AttendanceAccessTestFixture
{
    public SafeSchoolDbContext CreateDbContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }

    public TenantContext Tenant(string schoolAccountId = "school-1", string actorReference = "staff:gate") =>
        new() { TenantId = schoolAccountId, ActorReference = actorReference };

    public IConfiguration EnabledConfiguration() =>
        new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["FeatureSettings:DefaultAvailability"] = "Enabled" }).Build();

    public IConfiguration DisabledConfiguration() =>
        new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["FeatureSettings:DefaultAvailability"] = "Disabled" }).Build();

    public AttendanceAccessPermissionGuard Guard(SafeSchoolDbContext dbContext, ITenantContext tenantContext, IConfiguration? configuration = null) =>
        new(new FeatureGateService(configuration ?? EnabledConfiguration()), tenantContext, new AccessDecisionWriter(dbContext, tenantContext));

    public async Task<(Gate Gate, ScanPoint ScanPoint)> SeedActiveGateAsync(SafeSchoolDbContext dbContext, string tenantId = "school-1")
    {
        var gate = new Gate { TenantId = tenantId, GateCode = "MAIN", DisplayName = "Main Gate", CampusReference = "main", Status = GateStatus.Active };
        var scanPoint = new ScanPoint { TenantId = tenantId, GateId = gate.Id, DeviceReference = "device-1", AssignedActorReference = "staff:gate", Status = ScanPointStatus.Active };
        dbContext.Gates.Add(gate);
        dbContext.ScanPoints.Add(scanPoint);
        await dbContext.SaveChangesAsync();
        return (gate, scanPoint);
    }
}

