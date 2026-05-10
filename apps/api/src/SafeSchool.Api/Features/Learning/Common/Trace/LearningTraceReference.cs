namespace SafeSchool.Api.Features.Learning.Common.Trace;

public sealed record LearningTraceReference(string TraceType, string ReferenceId, string TenantId, string Description = "");
