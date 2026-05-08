using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.Transport.Common;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Tests.Features.Transport.Fixtures;

public sealed class TransportTestFixture
{
    public SafeSchoolDbContext CreateDbContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<SafeSchoolDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString("N"))
            .Options;
        return new SafeSchoolDbContext(options);
    }

    public TenantContext Tenant(string schoolAccountId = "school-1", string actorReference = "staff:transport") => new() { TenantId = schoolAccountId, ActorReference = actorReference };
    public IConfiguration EnabledConfiguration() => new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["FeatureSettings:DefaultAvailability"] = "Enabled" }).Build();
    public IConfiguration DisabledConfiguration() => new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["FeatureSettings:DefaultAvailability"] = "Disabled" }).Build();
    public TransportPermissionGuard Guard(SafeSchoolDbContext dbContext, ITenantContext tenantContext, IConfiguration? configuration = null) => new(new FeatureGateService(configuration ?? EnabledConfiguration()), tenantContext, new AccessDecisionWriter(dbContext, tenantContext));
}
