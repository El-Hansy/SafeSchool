using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.History;

public sealed record TransactionHistoryResponse(string TransactionId, Guid WalletId, string TransactionType, long AmountMinor, string CurrencyCode, string Status, string MerchantOrSource, DateTimeOffset EvidenceTime, bool StaffOnlyDetailSuppressed);
public sealed record ManualReviewRequest(string ReviewScope, string ScopeReference, ManualWalletReviewAction Action, string Reason, string ReviewerActor = "reviewer", string ClientRequestId = "");
public sealed record RefundOrReversalRequest(Guid WalletId, CorrectionType CorrectionType, string OriginalSourceType, string OriginalSourceReference, long AmountMinor, string CurrencyCode = "SAR", string Reason = "correction", string ClientRequestId = "", string ActorReference = "reviewer");
public sealed record CorrectionResponse(Guid CorrectionId, Guid WalletId, CorrectionType CorrectionType, CorrectionStatus Status, long AmountMinor, string Reason, Guid? ResultingLedgerEntryId);
public sealed record ManualReviewResponse(Guid ManualReviewId, string ReviewScope, string ScopeReference, ManualWalletReviewAction Action, ManualWalletReviewStatus Status, string Reason);
public sealed record AnomalyResponse(Guid AnomalyId, WalletAnomalyType Type, WalletAnomalySeverity Severity, WalletAnomalyStatus Status, string RelatedSourceReference, string ResolutionReason);
public sealed record TransactionTraceResponse(string TransactionId, IReadOnlyList<string> TraceReferences);
