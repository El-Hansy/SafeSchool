using SafeSchool.Api.Features.Learning.Common;

namespace SafeSchool.Api.Features.Learning.Domain;

public sealed class StarLedgerEntry : TenantOwnedEntity
{
    public string StudentProfileId { get; set; } = string.Empty;
    public StarLedgerDirection Direction { get; set; } = StarLedgerDirection.Credit;
    public StarLedgerState State { get; set; } = StarLedgerState.Posted;
    public int Amount { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public int RuleVersion { get; set; } = 1;
    public string Reason { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
}
