using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.AttendanceAccess.Anomalies;

public sealed class AnomalyDetectionService(SafeSchoolDbContext dbContext, AnomalyDetectionRuleSet ruleSet)
{
    public async Task<IReadOnlyList<AnomalyResponse>> RunAsync(string tenantId, RunAnomalyDetectionRequest request, CancellationToken cancellationToken = default)
    {
        var scans = await dbContext.GateScanEvents.Where(x => x.TenantId == tenantId).ToListAsync(cancellationToken);
        foreach (var scan in scans)
        {
            foreach (var classification in ruleSet.Classify(scan))
            {
                var existing = await dbContext.AttendanceAnomalies.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.EvidenceReference == scan.Id.ToString() && x.AnomalyType == classification.Type, cancellationToken);
                if (existing is not null)
                {
                    existing.Severity = classification.Severity;
                    continue;
                }

                dbContext.AttendanceAnomalies.Add(new AttendanceAnomaly
                {
                    TenantId = tenantId,
                    AnomalyType = classification.Type,
                    Severity = classification.Severity,
                    Status = Common.AnomalyStatus.New,
                    StudentProfileId = scan.StudentProfileId,
                    EvidenceReference = scan.Id.ToString(),
                    ResolutionReason = classification.Reason
                });
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.AttendanceAnomalies.Where(x => x.TenantId == tenantId).Select(x => x.ToResponse()).ToListAsync(cancellationToken);
    }
}

