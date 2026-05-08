namespace SafeSchool.Api.Features.Transport.Common.Trace;

public sealed record TransportTraceReference(string ReferenceType, string ReferenceId, string Label, DateTimeOffset? EvidenceAt = null);
