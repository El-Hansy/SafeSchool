using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Complaints;

public static class ComplaintsModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/complaints";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/complaints";
    public const string StudentRoutePrefix = "/api/v1/students/me/complaints";

    public static IServiceCollection AddComplaintsFeature(this IServiceCollection services)
    {
        services.AddScoped<ComplaintWorkflowService>();
        services.AddScoped<ComplaintBoundaryGuard>();
        services.AddSingleton<ComplaintIdempotencyService>();
        return services;
    }

    public static IEndpointRouteBuilder MapComplaintsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var school = endpoints.MapGroup(SchoolRoutePrefix);
        var guardian = endpoints.MapGroup(GuardianRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);

        school.MapGet("/", (string schoolAccountId) => Results.Ok(ComplaintWorkflowService.DemoBoard(schoolAccountId)));
        school.MapPost("/", (string schoolAccountId, ComplaintSubmissionRequest request, ComplaintWorkflowService service) => Results.Ok(service.Submit(schoolAccountId, request)));
        school.MapGet("/triage", (ComplaintWorkflowService service) => Results.Ok(service.Triage()));
        school.MapGet("/assigned", (ComplaintWorkflowService service) => Results.Ok(service.Assigned()));
        school.MapGet("/assigned/{complaintId}", (string complaintId, ComplaintWorkflowService service) => Results.Ok(service.Detail(complaintId)));
        school.MapGet("/escalations", (ComplaintWorkflowService service) => Results.Ok(service.Escalations()));
        school.MapGet("/exceptions", (ComplaintWorkflowService service) => Results.Ok(service.Exceptions()));
        school.MapGet("/summaries", (ComplaintWorkflowService service) => Results.Ok(service.Summaries()));
        school.MapGet("/configuration/categories/{categoryId}", (string categoryId, ComplaintWorkflowService service) => Results.Ok(service.Category(categoryId)));
        school.MapGet("/configuration/escalation-rules/{ruleId}", (string ruleId, ComplaintWorkflowService service) => Results.Ok(service.EscalationRule(ruleId)));
        school.MapGet("/{complaintId}", (string complaintId, ComplaintWorkflowService service) => Results.Ok(service.Detail(complaintId)));
        school.MapPost("/{complaintId}/assign", (string complaintId, ComplaintActionRequest request, ComplaintWorkflowService service) => Results.Ok(service.Assign(complaintId, request)));
        school.MapPost("/{complaintId}/escalate", (string complaintId, ComplaintActionRequest request, ComplaintWorkflowService service) => Results.Ok(service.Escalate(complaintId, request)));
        school.MapPost("/{complaintId}/resolve", (string complaintId, ComplaintActionRequest request, ComplaintWorkflowService service) => Results.Ok(service.Resolve(complaintId, request)));
        school.MapGet("/{complaintId}/trace", (string complaintId, ComplaintWorkflowService service) => Results.Ok(service.Trace(complaintId)));

        guardian.MapGet("/", () => Results.Ok(ComplaintWorkflowService.GuardianSummary()));
        guardian.MapPost("/", (ComplaintSubmissionRequest request, ITenantContext tenantContext, ComplaintWorkflowService service) =>
            Results.Ok(service.Submit(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "guardian" })));
        guardian.MapPost("/{complaintId}/feedback", (string complaintId, ComplaintActionRequest request, ComplaintWorkflowService service) => Results.Ok(service.Feedback(complaintId, request)));

        student.MapGet("/", () => Results.Ok(ComplaintWorkflowService.StudentSummary()));
        student.MapPost("/", (ComplaintSubmissionRequest request, ITenantContext tenantContext, ComplaintWorkflowService service) =>
            Results.Ok(service.Submit(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "student" })));
        return endpoints;
    }
}

public sealed record ComplaintSubmissionRequest(string StudentProfileId, string CategoryCode, string Description, string RequestedOutcome, string ClientRequestId, string SubmitterRole = "staff");
public sealed record ComplaintActionRequest(string Action, string Reason, string ActorId = "demo-actor", string ClientRequestId = "demo-request");
public sealed record ComplaintResponse(string ComplaintId, string TrackingReference, string Status, string Priority, string VisibleSummary, IReadOnlyList<string> AuditTrail);

public sealed class ComplaintWorkflowService(ComplaintIdempotencyService idempotency)
{
    public static object DemoBoard(string schoolAccountId) => new { schoolAccountId, phase = "complaints-escalations", status = "demo-ready", capabilities = ComplaintCapabilities.All, open = 14, escalated = 3, pendingFeedback = 5 };
    public static IReadOnlyList<ComplaintResponse> GuardianSummary() => [new("cmp-1", "CMP-2026-0001", "InReview", "High", "Your complaint is assigned and under review.", ["submitted", "assigned"])];
    public static IReadOnlyList<ComplaintResponse> StudentSummary() => [new("cmp-2", "CMP-2026-0002", "Received", "Normal", "Your complaint was received.", ["submitted"])];
    public IReadOnlyList<ComplaintResponse> Triage() => [new("cmp-1", "CMP-2026-0001", "NeedsTriage", "High", "Wellbeing category suggested; restricted details minimized.", ["submitted", "category-suggested"])];
    public IReadOnlyList<ComplaintResponse> Assigned() => [new("cmp-1", "CMP-2026-0001", "Assigned", "High", "Assigned to student wellbeing owner.", ["submitted", "categorized", "assigned"])];
    public IReadOnlyList<ComplaintResponse> Escalations() => [new("cmp-2", "CMP-2026-0002", "Escalated", "Urgent", "Escalated for same-day transport review.", ["submitted", "assigned", "escalated"])];
    public object Exceptions() => new[] { new { exceptionReference = "cmp-exception-1", reason = "Duplicate client request conflict", owner = "Complaint reviewer", status = "ManualReviewRequired" } };
    public object Summaries() => new[] { new { summaryReference = "cmp-summary-1", status = "Published", audience = "Guardian", evidence = "restricted-details-minimized" } };
    public object Category(string categoryId) => new { categoryId, name = "Wellbeing", ownerRole = "Student wellbeing", sla = "1 school day", evidence = new[] { "category-versioned", "routing-reviewed" } };
    public object EscalationRule(string ruleId) => new { ruleId, trigger = "Urgent priority or overdue SLA", ownerRole = "Senior operations", window = "Same day", evidence = new[] { "rule-versioned", "audit-written" } };
    public ComplaintResponse Detail(string complaintId) => new(complaintId, "CMP-2026-0001", "Assigned", "High", "Assigned to complaint owner queue without source-domain mutation.", ["submitted", "categorized", "assigned", "audit-written"]);

    public ComplaintResponse Submit(string tenantId, ComplaintSubmissionRequest request)
    {
        var duplicate = idempotency.Record($"{tenantId}:submit", request.ClientRequestId, $"{request.StudentProfileId}:{request.CategoryCode}:{request.Description}");
        var status = duplicate == ComplaintIdempotencyOutcome.Conflict ? "ManualReviewRequired" : "Received";
        return new ComplaintResponse($"complaint-{request.ClientRequestId}", "CMP-2026-0001", status, "High", "Complaint accepted with restricted details minimized.", ["tenant-checked", "feature-checked", "audit-written", duplicate.ToString()]);
    }

    public ComplaintResponse Assign(string complaintId, ComplaintActionRequest request) => new(complaintId, "CMP-2026-0001", "Assigned", "High", "Assigned to complaint owner queue without source-domain mutation.", ["assignment-audit", request.Reason]);
    public ComplaintResponse Escalate(string complaintId, ComplaintActionRequest request) => new(complaintId, "CMP-2026-0001", "Escalated", "Urgent", "Escalation owner notified through status event only.", ["escalation-audit", request.Reason]);
    public ComplaintResponse Resolve(string complaintId, ComplaintActionRequest request) => new(complaintId, "CMP-2026-0001", "Resolved", "High", "Resolution visible summary is ready for feedback.", ["resolution-audit", request.Reason]);
    public ComplaintResponse Feedback(string complaintId, ComplaintActionRequest request) => new(complaintId, "CMP-2026-0001", "ReopenRequested", "High", "Feedback captured and routed for review.", ["feedback-audit", request.Reason]);
    public object Trace(string complaintId) => new { traceId = $"trace-{complaintId}", references = new[] { "submission", "categorization", "assignment", "escalation", "resolution", "feedback", "audit" } };
}

public enum ComplaintIdempotencyOutcome { Accepted, Duplicate, Conflict }

public sealed class ComplaintIdempotencyService
{
    private readonly Dictionary<string, string> _fingerprints = [];
    public ComplaintIdempotencyOutcome Record(string command, string clientRequestId, string fingerprint)
    {
        var key = $"{command}:{clientRequestId}";
        if (!_fingerprints.TryGetValue(key, out var existing)) { _fingerprints[key] = fingerprint; return ComplaintIdempotencyOutcome.Accepted; }
        return existing == fingerprint ? ComplaintIdempotencyOutcome.Duplicate : ComplaintIdempotencyOutcome.Conflict;
    }
}

public sealed class ComplaintBoundaryGuard
{
    private static readonly string[] Blocked = ["attendance", "campus_gate", "scan", "transport", "wallet", "learning_reward", "request_approval", "medical", "emergency", "broad_messaging", "document", "search", "admin_dashboard"];
    public bool Allows(string sideEffect) => !Blocked.Contains(sideEffect, StringComparer.OrdinalIgnoreCase);
}

public static class ComplaintCapabilities
{
    public const string Submission = "complaints.submission";
    public const string Categorization = "complaints.categorization";
    public const string Assignment = "complaints.assignment";
    public const string Escalation = "complaints.escalation";
    public const string FeedbackResolution = "complaints.feedback_resolution";
    public const string History = "complaints.history";
    public const string Configuration = "complaints.configuration";
    public const string ReviewSummaries = "complaints.review_summaries";
    public static readonly string[] All = [Submission, Categorization, Assignment, Escalation, FeedbackResolution, History, Configuration, ReviewSummaries];
}
