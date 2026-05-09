namespace SafeSchool.Api.Features.Wallet.Common.Trace;

public sealed record WalletTraceReference(string ReferenceType, string ReferenceId, string Label, DateTimeOffset EvidenceTime);
