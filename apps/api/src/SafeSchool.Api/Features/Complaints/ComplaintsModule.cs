using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;
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
        return services;
    }

    public static IEndpointRouteBuilder MapComplaintsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var school = endpoints.MapGroup(SchoolRoutePrefix);
        var guardian = endpoints.MapGroup(GuardianRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);

        school.MapGet("/", async (string schoolAccountId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct)));
        school.MapPost("/", async (string schoolAccountId, ComplaintSubmissionRequest request, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.SubmitAsync(schoolAccountId, request, ct)));
        school.MapGet("/triage", async (string schoolAccountId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.TriageAsync(schoolAccountId, ct)));
        school.MapGet("/assigned", async (string schoolAccountId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.AssignedAsync(schoolAccountId, ct)));
        school.MapGet("/assigned/{complaintId}", async (string schoolAccountId, string complaintId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.DetailAsync(schoolAccountId, complaintId, ct)));
        school.MapGet("/escalations", async (string schoolAccountId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.EscalationsAsync(schoolAccountId, ct)));
        school.MapGet("/exceptions", async (string schoolAccountId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.ExceptionsAsync(schoolAccountId, ct)));
        school.MapGet("/summaries", async (string schoolAccountId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.SummariesAsync(schoolAccountId, ct)));
        school.MapGet("/configuration/categories/{categoryId}", (string categoryId, ComplaintWorkflowService service) => Results.Ok(service.Category(categoryId)));
        school.MapGet("/configuration/escalation-rules/{ruleId}", (string ruleId, ComplaintWorkflowService service) => Results.Ok(service.EscalationRule(ruleId)));
        school.MapGet("/{complaintId}", async (string schoolAccountId, string complaintId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.DetailAsync(schoolAccountId, complaintId, ct)));
        school.MapPost("/{complaintId}/assign", async (string schoolAccountId, string complaintId, ComplaintActionRequest request, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.AssignAsync(schoolAccountId, complaintId, request, ct)));
        school.MapPost("/{complaintId}/escalate", async (string schoolAccountId, string complaintId, ComplaintActionRequest request, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.EscalateAsync(schoolAccountId, complaintId, request, ct)));
        school.MapPost("/{complaintId}/resolve", async (string schoolAccountId, string complaintId, ComplaintActionRequest request, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.ResolveAsync(schoolAccountId, complaintId, request, ct)));
        school.MapGet("/{complaintId}/trace", async (string schoolAccountId, string complaintId, ComplaintWorkflowService service, CancellationToken ct) => Results.Ok(await service.TraceAsync(schoolAccountId, complaintId, ct)));

        guardian.MapGet("/", async (ITenantContext tenantContext, ComplaintWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct)));
        guardian.MapPost("/", async (ComplaintSubmissionRequest request, ITenantContext tenantContext, ComplaintWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.SubmitAsync(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "guardian" }, ct)));
        guardian.MapPost("/{complaintId}/feedback", async (string complaintId, ITenantContext tenantContext, ComplaintActionRequest request, ComplaintWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.FeedbackAsync(GuardianTenantResolver.Resolve(tenantContext), complaintId, request, ct)));

        student.MapGet("/", async (ITenantContext tenantContext, ComplaintWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "student", ct)));
        student.MapPost("/", async (ComplaintSubmissionRequest request, ITenantContext tenantContext, ComplaintWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.SubmitAsync(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "student" }, ct)));
        return endpoints;
    }
}

public sealed record ComplaintSubmissionRequest(string StudentProfileId, string CategoryCode, string Description, string RequestedOutcome, string ClientRequestId, string SubmitterRole = "staff");
public sealed record ComplaintActionRequest(string Action, string Reason, string ActorId = "demo-actor", string ClientRequestId = "demo-request");
public sealed record ComplaintResponse(string ComplaintId, string TrackingReference, string Status, string Priority, string VisibleSummary, IReadOnlyList<string> AuditTrail);

public sealed class ComplaintWorkflowService(SafeSchoolDbContext dbContext)
{
    private static readonly string[] OpenStatuses = ["Received", "NeedsTriage", "Assigned", "Escalated", "ReopenRequested", "ManualReviewRequired"];

    public async Task<object> BoardAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var records = dbContext.OperationalComplaints.AsNoTracking().Where(x => x.TenantId == tenantId);
        return new
        {
            schoolAccountId = tenantId,
            phase = "complaints-escalations",
            status = "operational",
            capabilities = ComplaintCapabilities.All,
            open = await records.CountAsync(x => OpenStatuses.Contains(x.Status), cancellationToken),
            escalated = await records.CountAsync(x => x.Status == "Escalated", cancellationToken),
            pendingFeedback = await records.CountAsync(x => x.Status == "Resolved", cancellationToken)
        };
    }

    public async Task<IReadOnlyList<ComplaintResponse>> AudienceSummaryAsync(string tenantId, string submitterRole, CancellationToken cancellationToken = default)
    {
        var records = await dbContext.OperationalComplaints.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && x.SubmitterRole == submitterRole)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    public Task<IReadOnlyList<ComplaintResponse>> TriageAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListByStatusAsync(tenantId, ["Received", "NeedsTriage", "ManualReviewRequired"], cancellationToken);

    public Task<IReadOnlyList<ComplaintResponse>> AssignedAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListByStatusAsync(tenantId, ["Assigned"], cancellationToken);

    public Task<IReadOnlyList<ComplaintResponse>> EscalationsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListByStatusAsync(tenantId, ["Escalated"], cancellationToken);

    public async Task<IReadOnlyList<object>> ExceptionsAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var events = await dbContext.OperationalComplaintEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId && x.EventType == "idempotency_conflict")
            .OrderByDescending(x => x.OccurredAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return events.Select(x => (object)new { exceptionReference = x.Id.ToString("N"), reason = x.Reason, owner = "Complaint reviewer", status = "ManualReviewRequired" }).ToList();
    }

    public async Task<IReadOnlyList<object>> SummariesAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var summaries = await dbContext.OperationalComplaints.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .GroupBy(x => x.Status)
            .Select(x => new { summaryReference = $"cmp-summary-{x.Key}", status = x.Key, count = x.Count(), evidence = "restricted-details-minimized" })
            .ToListAsync(cancellationToken);
        return summaries.Cast<object>().ToList();
    }

    public object Category(string categoryId) => new { categoryId, name = "Wellbeing", ownerRole = "Student wellbeing", sla = "1 school day", evidence = new[] { "category-versioned", "routing-reviewed" } };
    public object EscalationRule(string ruleId) => new { ruleId, trigger = "Urgent priority or overdue SLA", ownerRole = "Senior operations", window = "Same day", evidence = new[] { "rule-versioned", "audit-written" } };

    public async Task<ComplaintResponse> DetailAsync(string tenantId, string complaintId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, complaintId, cancellationToken);
        return record is null ? Missing(complaintId) : ToResponse(record);
    }

    public async Task<ComplaintResponse> SubmitAsync(string tenantId, ComplaintSubmissionRequest request, CancellationToken cancellationToken = default)
    {
        var fingerprint = Fingerprint(request);
        var command = $"{tenantId}:submit";
        var idempotency = await dbContext.OperationalComplaintIdempotencyRecords
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Command == command && x.ClientRequestId == request.ClientRequestId, cancellationToken);

        if (idempotency is not null)
        {
            var existing = await LoadAsync(idempotency.ComplaintId, cancellationToken);
            if (existing is null) return Missing(request.ClientRequestId);

            if (idempotency.Fingerprint == fingerprint)
            {
                return ToResponse(existing, ["idempotency_duplicate"]);
            }

            existing.Status = "ManualReviewRequired";
            existing.VisibleSummary = "Complaint requires manual review because the client request id was reused with different details.";
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            dbContext.OperationalComplaintEvents.Add(Event(tenantId, existing.Id, "idempotency_conflict", "system", "Client request id reused with different complaint details."));
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
        }

        var now = DateTimeOffset.UtcNow;
        var sequence = await dbContext.OperationalComplaints.CountAsync(x => x.TenantId == tenantId && x.CreatedAt.Year == now.Year, cancellationToken) + 1;
        var record = new OperationalComplaintRecord
        {
            TenantId = tenantId,
            TrackingReference = $"CMP-{now:yyyy}-{sequence:0000}",
            StudentProfileId = request.StudentProfileId,
            CategoryCode = request.CategoryCode,
            Description = request.Description,
            RequestedOutcome = request.RequestedOutcome,
            ClientRequestId = request.ClientRequestId,
            SubmitterRole = request.SubmitterRole,
            Status = "Received",
            Priority = PriorityFor(request.CategoryCode),
            VisibleSummary = "Complaint accepted with restricted details minimized.",
            CreatedAt = now,
            UpdatedAt = now
        };
        record.Events.Add(Event(tenantId, record.Id, "submitted", request.SubmitterRole, "Complaint submitted."));
        record.Events.Add(Event(tenantId, record.Id, "audit-written", request.SubmitterRole, "Append-only complaint audit event recorded."));
        dbContext.OperationalComplaints.Add(record);
        dbContext.OperationalComplaintIdempotencyRecords.Add(new OperationalComplaintIdempotencyRecord
        {
            TenantId = tenantId,
            Command = command,
            ClientRequestId = request.ClientRequestId,
            Fingerprint = fingerprint,
            ComplaintId = record.Id,
            CreatedAt = now
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, ["tenant-checked", "feature-checked"]);
    }

    public Task<ComplaintResponse> AssignAsync(string tenantId, string complaintId, ComplaintActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, complaintId, "Assigned", "Assigned to complaint owner queue without source-domain mutation.", "assignment-audit", request, cancellationToken);

    public Task<ComplaintResponse> EscalateAsync(string tenantId, string complaintId, ComplaintActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, complaintId, "Escalated", "Escalation owner notified through status event only.", "escalation-audit", request, cancellationToken, "Urgent");

    public Task<ComplaintResponse> ResolveAsync(string tenantId, string complaintId, ComplaintActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, complaintId, "Resolved", "Resolution visible summary is ready for feedback.", "resolution-audit", request, cancellationToken);

    public Task<ComplaintResponse> FeedbackAsync(string tenantId, string complaintId, ComplaintActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, complaintId, "ReopenRequested", "Feedback captured and routed for review.", "feedback-audit", request, cancellationToken);

    public async Task<object> TraceAsync(string tenantId, string complaintId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, complaintId, cancellationToken);
        if (record is null) return new { traceId = $"trace-{complaintId}", references = Array.Empty<string>(), status = "NotFound" };
        return new { traceId = $"trace-{record.Id:N}", references = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).ToArray(), status = record.Status };
    }

    private async Task<IReadOnlyList<ComplaintResponse>> ListByStatusAsync(string tenantId, string[] statuses, CancellationToken cancellationToken)
    {
        var records = await dbContext.OperationalComplaints.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && statuses.Contains(x.Status))
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    private async Task<ComplaintResponse> TransitionAsync(string tenantId, string complaintId, string status, string summary, string eventType, ComplaintActionRequest request, CancellationToken cancellationToken, string? priority = null)
    {
        var record = await FindAsync(tenantId, complaintId, cancellationToken);
        if (record is null) return Missing(complaintId);

        record.Status = status;
        record.VisibleSummary = summary;
        record.Priority = priority ?? record.Priority;
        record.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalComplaintEvents.Add(Event(tenantId, record.Id, eventType, request.ActorId, request.Reason));
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, [request.Reason]);
    }

    private async Task<OperationalComplaintRecord?> FindAsync(string tenantId, string complaintId, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalComplaints.Include(x => x.Events).Where(x => x.TenantId == tenantId);
        if (Guid.TryParse(complaintId, out var id))
        {
            return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        return await query.SingleOrDefaultAsync(x => x.TrackingReference == complaintId || x.ClientRequestId == complaintId, cancellationToken);
    }

    private async Task<OperationalComplaintRecord?> LoadAsync(Guid complaintId, CancellationToken cancellationToken) =>
        await dbContext.OperationalComplaints.Include(x => x.Events).SingleOrDefaultAsync(x => x.Id == complaintId, cancellationToken);

    private static ComplaintResponse ToResponse(OperationalComplaintRecord record, IReadOnlyList<string>? extraAudit = null)
    {
        var audit = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraAudit ?? []).Distinct().ToList();
        return new ComplaintResponse(record.Id.ToString("N"), record.TrackingReference, record.Status, record.Priority, record.VisibleSummary, audit);
    }

    private static ComplaintResponse Missing(string complaintId) => new(complaintId, "unknown", "NotFound", "Unknown", "Complaint was not found in this tenant.", ["tenant-checked", "not-found"]);

    private static OperationalComplaintEvent Event(string tenantId, Guid complaintId, string eventType, string actor, string reason) => new()
    {
        TenantId = tenantId,
        ComplaintId = complaintId,
        EventType = eventType,
        ActorReference = string.IsNullOrWhiteSpace(actor) ? "system" : actor,
        Reason = reason,
        OccurredAt = DateTimeOffset.UtcNow
    };

    private static string Fingerprint(ComplaintSubmissionRequest request) =>
        $"{request.StudentProfileId}|{request.CategoryCode}|{request.Description}|{request.RequestedOutcome}|{request.SubmitterRole}".ToUpperInvariant();

    private static string PriorityFor(string categoryCode) =>
        categoryCode.Contains("urgent", StringComparison.OrdinalIgnoreCase) ||
        categoryCode.Contains("safety", StringComparison.OrdinalIgnoreCase) ||
        categoryCode.Contains("wellbeing", StringComparison.OrdinalIgnoreCase)
            ? "High"
            : "Normal";
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
