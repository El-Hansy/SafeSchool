using SafeSchool.Api.Features.Wallet.Common;
using SafeSchool.Api.Features.Wallet.Ledger;

namespace SafeSchool.Api.Features.Wallet.Wallets;

public sealed record WalletCreateRequest(string StudentProfileId, string WalletCode, string CurrencyCode = "SAR", string CreatedBy = "system", string ClientRequestId = "");
public sealed record BulkActivateWalletsRequest(IReadOnlyList<string> StudentProfileIds, string CurrencyCode = "SAR", string CreatedBy = "system");
public sealed record WalletRestrictionRequest(string Reason, string ActorReference = "system");
public sealed record WalletRestoreRequest(string Reason = "restored", string ActorReference = "system");
public sealed record PostLedgerRequest(Guid WalletId, WalletLedgerEntryType EntryType, long AmountMinor, string CurrencyCode, string SourceType, string SourceReference, string IdempotencyKey, string CreatedBy = "system", string? Reason = null);
public sealed record WalletResponse(Guid WalletId, string TenantId, string StudentProfileId, string WalletCode, string CurrencyCode, long AvailableBalanceMinor, long PendingBalanceMinor, long HeldBalanceMinor, long SettledBalanceMinor, long PendingRecoveryMinor, WalletStatus WalletStatus, string RestrictionReason);
public sealed record LedgerEntryResponse(Guid LedgerEntryId, Guid WalletId, WalletLedgerEntryType EntryType, WalletLedgerEntryStatus EntryStatus, long AmountMinor, string CurrencyCode, long BalanceAvailableAfterMinor, string SourceType, string SourceReference, string IdempotencyKey, DateTimeOffset PostedAt, string ReviewReason);
public sealed record WalletTraceResponse(Guid WalletId, IReadOnlyList<string> TraceReferences, long AvailableBalanceMinor, long HeldBalanceMinor, long PendingRecoveryMinor);

public static class WalletLedgerMappingExtensions
{
    public static WalletResponse ToResponse(this StudentWallet wallet) => new(wallet.Id, wallet.TenantId, wallet.StudentProfileId, wallet.WalletCode, wallet.CurrencyCode, wallet.AvailableBalanceMinor, wallet.PendingBalanceMinor, wallet.HeldBalanceMinor, wallet.SettledBalanceMinor, wallet.PendingRecoveryMinor, wallet.WalletStatus, wallet.RestrictionReason);
    public static LedgerEntryResponse ToResponse(this WalletLedgerEntry entry) => new(entry.Id, entry.StudentWalletId, entry.EntryType, entry.EntryStatus, entry.AmountMinor, entry.CurrencyCode, entry.BalanceAvailableAfterMinor, entry.SourceType, entry.SourceReference, entry.IdempotencyKey, entry.PostedAt, entry.ReviewReason);
}
