using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Infrastructure.Persistence;

namespace SafeSchool.Api.Features.Wallet.Common.Idempotency;

public sealed class WalletIdempotencyRecord : TenantOwnedEntity
{
    public string IdempotencyKind { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
    public string SourceReference { get; set; } = string.Empty;
    public string OutcomeReference { get; set; } = string.Empty;
    public string RequestHash { get; set; } = string.Empty;
}

public sealed class WalletIdempotencyService(SafeSchoolDbContext dbContext)
{
    public async Task<OperationResult<WalletIdempotencyRecord>> ReserveAsync(
        string tenantId,
        string kind,
        string key,
        string sourceReference,
        string requestHash = "",
        CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.WalletIdempotencyRecords.SingleOrDefaultAsync(x =>
            x.TenantId == tenantId && x.IdempotencyKind == kind && x.IdempotencyKey == key, cancellationToken);

        if (existing is not null)
        {
            if (!string.IsNullOrWhiteSpace(existing.RequestHash) && !string.Equals(existing.RequestHash, requestHash, StringComparison.Ordinal))
            {
                return OperationResult<WalletIdempotencyRecord>.Failure(new ValidationError("idempotency_conflict", "The idempotency key was reused with different request content.", nameof(key)));
            }

            return OperationResult<WalletIdempotencyRecord>.Success(existing);
        }

        var record = new WalletIdempotencyRecord
        {
            TenantId = tenantId,
            IdempotencyKind = kind,
            IdempotencyKey = key,
            SourceReference = sourceReference,
            RequestHash = requestHash
        };
        dbContext.WalletIdempotencyRecords.Add(record);
        await dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult<WalletIdempotencyRecord>.Success(record);
    }
}
