using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Anomalies;

public sealed class WalletAnomalyDetectionService(SafeSchoolDbContext dbContext)
{
    public WalletAnomaly Detect(string tenantId, string sourceType, string sourceReference, SafeSchool.Api.Features.Wallet.Common.WalletAnomalyType anomalyType)
    {
        var anomaly = new WalletAnomaly { TenantId = tenantId, RelatedSourceType = sourceType, RelatedSourceReference = sourceReference, AnomalyType = anomalyType };
        dbContext.WalletAnomalies.Add(anomaly);
        return anomaly;
    }
}
