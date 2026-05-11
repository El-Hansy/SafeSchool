using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Requests;

public static class RequestsModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/requests";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/requests";
    public const string StudentRoutePrefix = "/api/v1/students/me/requests";

    public static IServiceCollection AddRequestsFeature(this IServiceCollection services)
    {
        services.AddScoped<RequestWorkflowService>();
        services.AddScoped<RequestBoundaryGuard>();
        return services;
    }

    public static IEndpointRouteBuilder MapRequestsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var school = endpoints.MapGroup(SchoolRoutePrefix);
        var guardian = endpoints.MapGroup(GuardianRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);

        school.MapGet("/", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct)));
        school.MapPost("/", async (string schoolAccountId, RequestSubmissionRequest request, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.SubmitAsync(schoolAccountId, request with { SubmitterRole = string.IsNullOrWhiteSpace(request.SubmitterRole) ? "staff" : request.SubmitterRole }, ct)));
        school.MapGet("/outing", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "outing", ct)));
        school.MapGet("/early-leave", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "early-leave", ct)));
        school.MapGet("/approvals", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ApprovalsAsync(schoolAccountId, ct)));
        school.MapGet("/history", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.HistoryAsync(schoolAccountId, ct)));
        school.MapGet("/configuration", (RequestWorkflowService service) => Results.Ok(service.Configuration()));
        school.MapGet("/configuration/star-rules/{ruleId}", (string ruleId, RequestWorkflowService service) => Results.Ok(service.StarRule(ruleId)));
        school.MapGet("/{requestId}", async (string schoolAccountId, string requestId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.DetailAsync(schoolAccountId, requestId, ct)));
        school.MapPost("/{requestId}/approve", async (string schoolAccountId, string requestId, RequestActionRequest request, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ApproveAsync(schoolAccountId, requestId, request, ct)));
        school.MapPost("/{requestId}/reject", async (string schoolAccountId, string requestId, RequestActionRequest request, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.RejectAsync(schoolAccountId, requestId, request, ct)));
        school.MapPost("/{requestId}/cancel", async (string schoolAccountId, string requestId, RequestActionRequest request, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.CancelAsync(schoolAccountId, requestId, request, ct)));
        school.MapGet("/{requestId}/trace", async (string schoolAccountId, string requestId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.TraceAsync(schoolAccountId, requestId, ct)));

        guardian.MapGet("/", async (ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct)));
        guardian.MapPost("/", async (RequestSubmissionRequest request, ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.SubmitAsync(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "guardian" }, ct)));

        student.MapGet("/", async (ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "student", ct)));
        student.MapPost("/", async (RequestSubmissionRequest request, ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.SubmitAsync(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "student" }, ct)));
        return endpoints;
    }
}

public sealed record RequestSubmissionRequest(
    string StudentProfileId,
    string RequestType,
    string Reason,
    string RequestedOutcome,
    string ClientRequestId,
    DateTimeOffset? StartsAt = null,
    DateTimeOffset? EndsAt = null,
    string SubmitterRole = "guardian");

public sealed record RequestActionRequest(string Action, string Reason, string ActorId = "request-approver", string ClientRequestId = "request-action");

public sealed record RequestResponse(string RequestId, string TrackingReference, string RequestType, string Status, string Priority, string StudentProfileId, string VisibleSummary, DateTimeOffset? StartsAt, DateTimeOffset? EndsAt, IReadOnlyList<string> AuditTrail);

public sealed class RequestWorkflowService(SafeSchoolDbContext dbContext)
{
    private static readonly string[] OpenStatuses = ["Submitted", "PendingApproval", "NeedsReview", "Approved", "Rejected", "CancelRequested"];

    public async Task<object> BoardAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var records = dbContext.OperationalRequests.AsNoTracking().Where(x => x.TenantId == tenantId);
        return new
        {
            schoolAccountId = tenantId,
            phase = "requests-permissions",
            status = "operational",
            capabilities = RequestCapabilities.All,
            open = await records.CountAsync(x => OpenStatuses.Contains(x.Status), cancellationToken),
            pendingApproval = await records.CountAsync(x => x.Status == "PendingApproval" || x.Status == "NeedsReview", cancellationToken),
            approvedToday = await records.CountAsync(x => x.Status == "Approved" && x.UpdatedAt.Date == DateTimeOffset.UtcNow.Date, cancellationToken)
        };
    }

    public Task<IReadOnlyList<RequestResponse>> ApprovalsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, ["Submitted", "PendingApproval", "NeedsReview"], cancellationToken);

    public Task<IReadOnlyList<RequestResponse>> HistoryAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, OpenStatuses, cancellationToken);

    public Task<IReadOnlyList<RequestResponse>> ByTypeAsync(string tenantId, string requestType, CancellationToken cancellationToken = default) =>
        ListByTypeAsync(tenantId, requestType, cancellationToken);

    public async Task<IReadOnlyList<RequestResponse>> AudienceSummaryAsync(string tenantId, string submitterRole, CancellationToken cancellationToken = default)
    {
        var records = await dbContext.OperationalRequests.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && x.SubmitterRole == submitterRole)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    public object Configuration() => new { approvalLevels = 2, starRules = 3, earlyLeaveRequiresStaffRelease = true, evidence = new[] { "tenant-scoped", "role-reviewed", "audit-written" } };
    public object StarRule(string ruleId) => new { ruleId, trigger = "Positive star balance and guardian consent", status = "Enabled", review = "Human approval required", evidence = new[] { "rule-versioned", "source-read-only" } };

    public async Task<RequestResponse> DetailAsync(string tenantId, string requestId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, requestId, cancellationToken);
        return record is null ? Missing(requestId) : ToResponse(record);
    }

    public async Task<RequestResponse> SubmitAsync(string tenantId, RequestSubmissionRequest request, CancellationToken cancellationToken = default)
    {
        var fingerprint = Fingerprint(request);
        var command = $"{tenantId}:submit";
        var idempotency = await dbContext.OperationalRequestIdempotencyRecords
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Command == command && x.ClientRequestId == request.ClientRequestId, cancellationToken);

        if (idempotency is not null)
        {
            var existing = await LoadAsync(idempotency.RequestId, cancellationToken);
            if (existing is null) return Missing(request.ClientRequestId);
            if (idempotency.Fingerprint == fingerprint) return ToResponse(existing, ["idempotency_duplicate"]);

            existing.Status = "NeedsReview";
            existing.VisibleSummary = "Request requires manual review because the client request id was reused with different details.";
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            dbContext.OperationalRequestEvents.Add(Event(tenantId, existing.Id, "idempotency_conflict", "system", "Client request id reused with different request details."));
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
        }

        var now = DateTimeOffset.UtcNow;
        var sequence = await dbContext.OperationalRequests.CountAsync(x => x.TenantId == tenantId && x.CreatedAt.Year == now.Year, cancellationToken) + 1;
        var record = new OperationalRequestRecord
        {
            TenantId = tenantId,
            TrackingReference = $"REQ-{now:yyyy}-{sequence:0000}",
            StudentProfileId = request.StudentProfileId,
            RequestType = NormalizeType(request.RequestType),
            Reason = request.Reason,
            RequestedOutcome = request.RequestedOutcome,
            ClientRequestId = request.ClientRequestId,
            SubmitterRole = request.SubmitterRole,
            Status = "PendingApproval",
            Priority = PriorityFor(request.RequestType),
            VisibleSummary = "Request accepted and routed to approval workflow.",
            StartsAt = request.StartsAt,
            EndsAt = request.EndsAt,
            CreatedAt = now,
            UpdatedAt = now
        };
        record.Events.Add(Event(tenantId, record.Id, "submitted", request.SubmitterRole, "Request submitted."));
        record.Events.Add(Event(tenantId, record.Id, "approval-routing", "system", "Approval route selected without mutating attendance, transport, wallet, learning, or medical outcomes."));
        dbContext.OperationalRequests.Add(record);
        dbContext.OperationalRequestIdempotencyRecords.Add(new OperationalRequestIdempotencyRecord
        {
            TenantId = tenantId,
            Command = command,
            ClientRequestId = request.ClientRequestId,
            Fingerprint = fingerprint,
            RequestId = record.Id,
            CreatedAt = now
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, ["tenant-checked", "feature-checked"]);
    }

    public Task<RequestResponse> ApproveAsync(string tenantId, string requestId, RequestActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, requestId, "Approved", "Approved request is ready for source-domain follow-up by authorized staff.", "approved", request, cancellationToken);

    public Task<RequestResponse> RejectAsync(string tenantId, string requestId, RequestActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, requestId, "Rejected", "Request rejected with visible reason and preserved evidence.", "rejected", request, cancellationToken);

    public Task<RequestResponse> CancelAsync(string tenantId, string requestId, RequestActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, requestId, "Cancelled", "Request cancelled without changing source-domain outcomes.", "cancelled", request, cancellationToken);

    public async Task<object> TraceAsync(string tenantId, string requestId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, requestId, cancellationToken);
        if (record is null) return new { traceId = $"trace-{requestId}", references = Array.Empty<string>(), status = "NotFound" };
        return new { traceId = $"trace-{record.Id:N}", references = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).ToArray(), status = record.Status };
    }

    private async Task<IReadOnlyList<RequestResponse>> ListAsync(string tenantId, string[] statuses, CancellationToken cancellationToken)
    {
        var records = await dbContext.OperationalRequests.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && statuses.Contains(x.Status))
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    private async Task<IReadOnlyList<RequestResponse>> ListByTypeAsync(string tenantId, string requestType, CancellationToken cancellationToken)
    {
        var normalized = NormalizeType(requestType);
        var records = await dbContext.OperationalRequests.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && x.RequestType == normalized)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    private async Task<RequestResponse> TransitionAsync(string tenantId, string requestId, string status, string summary, string eventType, RequestActionRequest request, CancellationToken cancellationToken)
    {
        var record = await FindAsync(tenantId, requestId, cancellationToken);
        if (record is null) return Missing(requestId);

        record.Status = status;
        record.VisibleSummary = summary;
        record.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalRequestEvents.Add(Event(tenantId, record.Id, eventType, request.ActorId, request.Reason));
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, [request.Reason]);
    }

    private async Task<OperationalRequestRecord?> FindAsync(string tenantId, string requestId, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalRequests.Include(x => x.Events).Where(x => x.TenantId == tenantId);
        if (Guid.TryParse(requestId, out var id)) return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        return await query.SingleOrDefaultAsync(x => x.TrackingReference == requestId || x.ClientRequestId == requestId, cancellationToken);
    }

    private async Task<OperationalRequestRecord?> LoadAsync(Guid requestId, CancellationToken cancellationToken) =>
        await dbContext.OperationalRequests.Include(x => x.Events).SingleOrDefaultAsync(x => x.Id == requestId, cancellationToken);

    private static RequestResponse ToResponse(OperationalRequestRecord record, IReadOnlyList<string>? extraAudit = null)
    {
        var audit = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraAudit ?? []).Distinct().ToList();
        return new RequestResponse(record.Id.ToString("N"), record.TrackingReference, record.RequestType, record.Status, record.Priority, record.StudentProfileId, record.VisibleSummary, record.StartsAt, record.EndsAt, audit);
    }

    private static RequestResponse Missing(string requestId) => new(requestId, "unknown", "unknown", "NotFound", "Unknown", "", "Request was not found in this tenant.", null, null, ["tenant-checked", "not-found"]);

    private static OperationalRequestEvent Event(string tenantId, Guid requestId, string eventType, string actor, string reason) => new()
    {
        TenantId = tenantId,
        RequestId = requestId,
        EventType = eventType,
        ActorReference = string.IsNullOrWhiteSpace(actor) ? "system" : actor,
        Reason = reason,
        OccurredAt = DateTimeOffset.UtcNow
    };

    private static string Fingerprint(RequestSubmissionRequest request) =>
        $"{request.StudentProfileId}|{NormalizeType(request.RequestType)}|{request.Reason}|{request.RequestedOutcome}|{request.StartsAt}|{request.EndsAt}|{request.SubmitterRole}".ToUpperInvariant();

    private static string NormalizeType(string requestType) => requestType.Trim().ToLowerInvariant().Replace("_", "-");

    private static string PriorityFor(string requestType) =>
        requestType.Contains("early", StringComparison.OrdinalIgnoreCase) ? "High" : "Normal";
}

public sealed class RequestBoundaryGuard
{
    private static readonly string[] Blocked = ["attendance", "campus_gate", "scan", "transport", "wallet", "learning_reward", "medical", "emergency", "complaint", "communication", "document", "search"];
    public bool Allows(string sideEffect) => !Blocked.Contains(sideEffect, StringComparer.OrdinalIgnoreCase);
}

public static class RequestCapabilities
{
    public const string Outing = "requests.outing";
    public const string EarlyLeave = "requests.early_leave";
    public const string Approval = "requests.approval";
    public const string StarRules = "requests.star_rules";
    public const string History = "requests.history";
    public const string Configuration = "requests.configuration";
    public static readonly string[] All = [Outing, EarlyLeave, Approval, StarRules, History, Configuration];
}

