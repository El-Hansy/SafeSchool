namespace SafeSchool.Api.Features.Wallet.Reconciliation;

public sealed class WalletRetentionService
{
    public string ApplyRetention(string tenantId, int detailedRetentionDays, bool hasActiveHold) => hasActiveHold ? "preserved-by-active-hold" : $"reduced-after-{detailedRetentionDays}-days";
}
