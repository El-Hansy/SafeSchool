using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Requests;

public static class RequestsModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/requests";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/requests";
    public const string GuardianStudentRoutePrefix = "/api/v1/guardians/me/students/{studentProfileId}/requests";
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
        var guardianStudent = endpoints.MapGroup(GuardianStudentRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);

        school.MapGet("/", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.Read);
        school.MapPost("/", async (string schoolAccountId, RequestSubmissionRequest request, RequestWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.SubmitAsync(schoolAccountId, request with { SubmitterRole = string.IsNullOrWhiteSpace(request.SubmitterRole) ? "staff" : request.SubmitterRole }, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Create);
        school.MapGet("/outing", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "outing", ct))).RequireCapability(RequestCapabilities.Outing, RequestPermissions.Read);
        school.MapGet("/early-leave", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "early-leave", ct))).RequireCapability(RequestCapabilities.EarlyLeave, RequestPermissions.Read);
        school.MapGet("/early-leave/{requestId}/release-eligibility", async (string schoolAccountId, string requestId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ReleaseEligibilityAsync(schoolAccountId, requestId, ct))).RequireCapability(RequestCapabilities.EarlyLeave, RequestPermissions.ReleaseRead);
        school.MapGet("/approvals", async (string schoolAccountId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.ApprovalsAsync(schoolAccountId, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Decide);
        school.MapGet("/history", async (string schoolAccountId, string? studentProfileId, string? requestType, string? status, string? submitterRole, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.HistoryAsync(schoolAccountId, new RequestHistoryFilter(studentProfileId, requestType, status, submitterRole), ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.HistoryRead);
        school.MapGet("/status-events", async (string schoolAccountId, string? studentProfileId, string? requestType, string? status, string? sourceEventType, bool? notificationEligible, bool? reviewRequired, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.StatusEventsAsync(schoolAccountId, new RequestStatusEventFilter(studentProfileId, requestType, status, sourceEventType, notificationEligible, reviewRequired), ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.HistoryRead);
        school.MapGet("/review-summaries", async (string schoolAccountId, string? studentProfileId, string? requestType, string? status, string? currentAssignee, string? exceptionState, string? starOutcome, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.ReviewSummariesAsync(schoolAccountId, new RequestReviewSummaryFilter(studentProfileId, requestType, status, currentAssignee, exceptionState, starOutcome), ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.AuditRead);
        school.MapGet("/configuration", (RequestWorkflowService service) => Results.Ok(service.Configuration())).RequireCapability(RequestCapabilities.Configuration, RequestPermissions.Configure);
        school.MapGet("/configuration/star-rules/{ruleId}", (string ruleId, RequestWorkflowService service) => Results.Ok(service.StarRule(ruleId))).RequireCapability(RequestCapabilities.StarRules, RequestPermissions.Configure);
        school.MapGet("/{requestId}", async (string schoolAccountId, string requestId, RequestWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.DetailAsync(schoolAccountId, requestId, ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.Read);
        school.MapPost("/{requestId}/approve", async (string schoolAccountId, string requestId, RequestActionRequest request, RequestWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.ApproveAsync(schoolAccountId, requestId, request, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Decide);
        school.MapPost("/{requestId}/reject", async (string schoolAccountId, string requestId, RequestActionRequest request, RequestWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.RejectAsync(schoolAccountId, requestId, request, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Decide);
        school.MapPost("/{requestId}/cancel", async (string schoolAccountId, string requestId, RequestActionRequest request, RequestWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.CancelAsync(schoolAccountId, requestId, request, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Decide);
        school.MapPost("/{requestId}/withdraw", async (string schoolAccountId, string requestId, RequestActionRequest request, RequestWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.WithdrawAsync(schoolAccountId, requestId, request, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Withdraw);
        school.MapGet("/{requestId}/trace", async (string schoolAccountId, string requestId, RequestWorkflowService service, CancellationToken ct) => Results.Ok(await service.TraceAsync(schoolAccountId, requestId, ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.AuditRead);

        guardian.MapGet("/", async (ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.GuardianRead);
        guardian.MapPost("/", async (RequestSubmissionRequest request, ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            ToEndpointResult(await service.SubmitAsync(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "guardian" }, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Create);
        guardianStudent.MapGet("/", async (string studentProfileId, ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct, studentProfileId))).RequireCapability(RequestCapabilities.History, RequestPermissions.GuardianRead);
        guardianStudent.MapPost("/", async (string studentProfileId, RequestSubmissionRequest request, ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            ToEndpointResult(await service.SubmitAsync(GuardianTenantResolver.Resolve(tenantContext), request with { StudentProfileId = studentProfileId, SubmitterRole = "guardian" }, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Create);

        student.MapGet("/", async (ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "student", ct))).RequireCapability(RequestCapabilities.History, RequestPermissions.StudentRead);
        student.MapPost("/", async (RequestSubmissionRequest request, ITenantContext tenantContext, RequestWorkflowService service, CancellationToken ct) =>
            ToEndpointResult(await service.SubmitAsync(GuardianTenantResolver.Resolve(tenantContext), request with { SubmitterRole = "student" }, ct))).RequireCapability(RequestCapabilities.Approval, RequestPermissions.Create);
        return endpoints;
    }

    private static IResult ToEndpointResult(RequestResponse response) => response.Status switch
    {
        "ValidationFailed" => Results.BadRequest(response),
        "DuplicateBlocked" => Results.Conflict(response),
        "FinalStateRejected" => Results.Conflict(response),
        "NotFound" => Results.NotFound(response),
        _ => Results.Ok(response)
    };
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
public sealed record RequestHistoryFilter(string? StudentProfileId = null, string? RequestType = null, string? Status = null, string? SubmitterRole = null);
public sealed record RequestStatusEventFilter(string? StudentProfileId = null, string? RequestType = null, string? Status = null, string? SourceEventType = null, bool? NotificationEligible = null, bool? ReviewRequired = null);
public sealed record RequestReviewSummaryFilter(string? StudentProfileId = null, string? RequestType = null, string? Status = null, string? CurrentAssignee = null, string? ExceptionState = null, string? StarOutcome = null);

public sealed class RequestWorkflowService(SafeSchoolDbContext dbContext)
{
    private static readonly string[] OpenStatuses = ["Submitted", "PendingApproval", "NeedsReview", "Approved", "Rejected", "CancelRequested"];
    private static readonly string[] ActiveStatuses = ["Submitted", "PendingApproval", "NeedsReview", "Approved", "CancelRequested"];
    private static readonly string[] FinalStatuses = ["Approved", "Rejected", "Cancelled", "Withdrawn"];
    private static readonly string[] AllowedRequestTypes = ["outing", "early-leave", "permission"];

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
            approvedToday = await records.CountAsync(x => x.Status == "Approved" && x.UpdatedAt.Date == DateTimeOffset.UtcNow.Date, cancellationToken),
            statusEvents = await dbContext.OperationalRequestStatusEvents.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
            reviewSummaries = await dbContext.OperationalRequestReviewSummaries.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken)
        };
    }

    public Task<IReadOnlyList<RequestResponse>> ApprovalsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, ["Submitted", "PendingApproval", "NeedsReview"], new RequestHistoryFilter(), cancellationToken);

    public Task<IReadOnlyList<RequestResponse>> HistoryAsync(string tenantId, CancellationToken cancellationToken = default) =>
        HistoryAsync(tenantId, new RequestHistoryFilter(), cancellationToken);

    public Task<IReadOnlyList<RequestResponse>> HistoryAsync(string tenantId, RequestHistoryFilter filter, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, OpenStatuses, filter, cancellationToken);

    public Task<IReadOnlyList<RequestResponse>> ByTypeAsync(string tenantId, string requestType, CancellationToken cancellationToken = default) =>
        ListByTypeAsync(tenantId, requestType, cancellationToken);

    public Task<IReadOnlyList<OperationalRequestStatusEvent>> StatusEventsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        StatusEventsAsync(tenantId, new RequestStatusEventFilter(), cancellationToken);

    public async Task<IReadOnlyList<OperationalRequestStatusEvent>> StatusEventsAsync(string tenantId, RequestStatusEventFilter filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.OperationalRequestStatusEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId);
        if (!string.IsNullOrWhiteSpace(filter.StudentProfileId)) query = query.Where(x => x.StudentProfileId == filter.StudentProfileId);
        if (!string.IsNullOrWhiteSpace(filter.RequestType)) query = query.Where(x => x.RequestType == NormalizeType(filter.RequestType));
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.SourceEventType)) query = query.Where(x => x.SourceEventType == filter.SourceEventType);
        if (filter.NotificationEligible is not null) query = query.Where(x => x.NotificationEligible == filter.NotificationEligible);
        if (filter.ReviewRequired is not null) query = query.Where(x => x.ReviewRequired == filter.ReviewRequired);

        return await query
            .OrderByDescending(x => x.OccurredAt)
            .Take(100)
            .ToListAsync(cancellationToken);
    }

    public Task<IReadOnlyList<OperationalRequestReviewSummary>> ReviewSummariesAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ReviewSummariesAsync(tenantId, new RequestReviewSummaryFilter(), cancellationToken);

    public async Task<IReadOnlyList<OperationalRequestReviewSummary>> ReviewSummariesAsync(string tenantId, RequestReviewSummaryFilter filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.OperationalRequestReviewSummaries.AsNoTracking()
            .Where(x => x.TenantId == tenantId);
        if (!string.IsNullOrWhiteSpace(filter.StudentProfileId)) query = query.Where(x => x.StudentProfileId == filter.StudentProfileId);
        if (!string.IsNullOrWhiteSpace(filter.RequestType)) query = query.Where(x => x.RequestType == NormalizeType(filter.RequestType));
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.CurrentAssignee)) query = query.Where(x => x.CurrentAssignee == filter.CurrentAssignee);
        if (!string.IsNullOrWhiteSpace(filter.ExceptionState)) query = query.Where(x => x.ExceptionState == filter.ExceptionState);
        if (!string.IsNullOrWhiteSpace(filter.StarOutcome)) query = query.Where(x => x.StarOutcome == filter.StarOutcome);

        return await query
            .OrderByDescending(x => x.UpdatedAt)
            .Take(100)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RequestResponse>> AudienceSummaryAsync(string tenantId, string submitterRole, CancellationToken cancellationToken = default, string? studentProfileId = null)
    {
        var query = dbContext.OperationalRequests.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && x.SubmitterRole == submitterRole);
        if (!string.IsNullOrWhiteSpace(studentProfileId))
        {
            query = query.Where(x => x.StudentProfileId == studentProfileId);
        }

        var records = await query
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
        var validation = ValidateSubmission(request);
        if (validation is not null) return validation;

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
            await RecordLifecycleEvidenceAsync(tenantId, existing, "idempotency_conflict", false, true, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
        }

        var normalizedType = NormalizeType(request.RequestType);
        var exactDuplicate = await dbContext.OperationalRequests
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId
                && x.StudentProfileId == request.StudentProfileId
                && x.RequestType == normalizedType
                && ActiveStatuses.Contains(x.Status)
                && x.StartsAt == request.StartsAt
                && x.EndsAt == request.EndsAt)
            .OrderByDescending(x => x.UpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (exactDuplicate is not null)
        {
            dbContext.OperationalRequestEvents.Add(Event(tenantId, exactDuplicate.Id, "duplicate_blocked", request.SubmitterRole, "Exact active duplicate request blocked."));
            await RecordLifecycleEvidenceAsync(tenantId, exactDuplicate, "duplicate_blocked", false, true, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(exactDuplicate, ["duplicate_blocked"]) with
            {
                Status = "DuplicateBlocked",
                VisibleSummary = "Exact active duplicate request was blocked and original request evidence was preserved."
            };
        }

        var overlappingRequest = await FindOverlappingActiveRequestAsync(tenantId, request, normalizedType, cancellationToken);
        var now = DateTimeOffset.UtcNow;
        var sequence = await dbContext.OperationalRequests.CountAsync(x => x.TenantId == tenantId && x.CreatedAt.Year == now.Year, cancellationToken) + 1;
        var record = new OperationalRequestRecord
        {
            TenantId = tenantId,
            TrackingReference = $"REQ-{now:yyyy}-{sequence:0000}",
            StudentProfileId = request.StudentProfileId,
            RequestType = normalizedType,
            Reason = request.Reason,
            RequestedOutcome = request.RequestedOutcome,
            ClientRequestId = request.ClientRequestId,
            SubmitterRole = request.SubmitterRole,
            Status = overlappingRequest is null ? "PendingApproval" : "NeedsReview",
            Priority = PriorityFor(request.RequestType),
            VisibleSummary = overlappingRequest is null
                ? "Request accepted and routed to approval workflow."
                : $"Request routed to manual review because it overlaps {overlappingRequest.TrackingReference}.",
            StartsAt = request.StartsAt,
            EndsAt = request.EndsAt,
            CreatedAt = now,
            UpdatedAt = now
        };
        record.Events.Add(Event(tenantId, record.Id, "submitted", request.SubmitterRole, "Request submitted."));
        record.Events.Add(Event(tenantId, record.Id, "approval-routing", "system", "Approval route selected without mutating attendance, transport, wallet, learning, or medical outcomes."));
        if (overlappingRequest is not null)
        {
            record.Events.Add(Event(tenantId, record.Id, "overlap_manual_review", "system", $"Overlaps active request {overlappingRequest.TrackingReference}."));
        }

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
        await RecordLifecycleEvidenceAsync(tenantId, record, record.Status == "NeedsReview" ? "overlap_manual_review" : "submitted", true, record.Status == "NeedsReview", cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, ["tenant-checked", "feature-checked"]);
    }

    public Task<RequestResponse> ApproveAsync(string tenantId, string requestId, RequestActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, requestId, "Approved", "Approved request is ready for source-domain follow-up by authorized staff.", "approved", request, cancellationToken);

    public Task<RequestResponse> RejectAsync(string tenantId, string requestId, RequestActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, requestId, "Rejected", "Request rejected with visible reason and preserved evidence.", "rejected", request, cancellationToken);

    public Task<RequestResponse> CancelAsync(string tenantId, string requestId, RequestActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, requestId, "Cancelled", "Request cancelled without changing source-domain outcomes.", "cancelled", request, cancellationToken);

    public Task<RequestResponse> WithdrawAsync(string tenantId, string requestId, RequestActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, requestId, "Withdrawn", "Request withdrawn by requester while preserving approval history.", "withdrawn", request, cancellationToken);

    public async Task<object> ReleaseEligibilityAsync(string tenantId, string requestId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, requestId, cancellationToken);
        if (record is null)
        {
            return new
            {
                requestId,
                status = "NotFound",
                eligible = false,
                evidence = new[] { "tenant-checked", "not-found" }
            };
        }

        var isEarlyLeave = record.RequestType == "early-leave";
        var eligible = isEarlyLeave && record.Status == "Approved";
        return new
        {
            requestId = record.Id.ToString("N"),
            record.TrackingReference,
            record.StudentProfileId,
            status = eligible ? "Eligible" : "NotEligible",
            eligible,
            reason = eligible
                ? "Approved early leave is available as read-only release evidence."
                : "Release eligibility requires an approved early leave request.",
            evidence = new[] { "tenant-checked", "read-only-request-evidence", "no-attendance-event", "no-gate-event", "no-scan-event" }
        };
    }

    public async Task<object> TraceAsync(string tenantId, string requestId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, requestId, cancellationToken);
        if (record is null) return new { traceId = $"trace-{requestId}", references = Array.Empty<string>(), status = "NotFound" };
        return new { traceId = $"trace-{record.Id:N}", references = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).ToArray(), status = record.Status };
    }

    private async Task<IReadOnlyList<RequestResponse>> ListAsync(string tenantId, string[] statuses, RequestHistoryFilter filter, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalRequests.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && statuses.Contains(x.Status));
        if (!string.IsNullOrWhiteSpace(filter.StudentProfileId)) query = query.Where(x => x.StudentProfileId == filter.StudentProfileId);
        if (!string.IsNullOrWhiteSpace(filter.RequestType)) query = query.Where(x => x.RequestType == NormalizeType(filter.RequestType));
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.SubmitterRole)) query = query.Where(x => x.SubmitterRole == filter.SubmitterRole);

        var records = await query
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
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            return ValidationFailed("Decision reason is required.");
        }
        if (string.IsNullOrWhiteSpace(request.ClientRequestId))
        {
            return ValidationFailed("Client request id is required for idempotency.");
        }

        var record = await FindAsync(tenantId, requestId, cancellationToken);
        if (record is null) return Missing(requestId);

        var command = $"{tenantId}:request-action:{record.Id:N}";
        var fingerprint = $"{eventType}|{status}|{request.ActorId}|{request.Reason}".ToUpperInvariant();
        var idempotency = await dbContext.OperationalRequestIdempotencyRecords
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Command == command && x.ClientRequestId == request.ClientRequestId, cancellationToken);
        if (idempotency is not null)
        {
            if (idempotency.Fingerprint == fingerprint) return ToResponse(record, ["idempotency_duplicate"]);

            dbContext.OperationalRequestEvents.Add(Event(tenantId, record.Id, "idempotency_conflict", request.ActorId, "Action client request id reused with different decision details."));
            await RecordLifecycleEvidenceAsync(tenantId, record, "idempotency_conflict", false, true, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(record, ["idempotency_conflict"]);
        }

        if (FinalStatuses.Contains(record.Status))
        {
            dbContext.OperationalRequestEvents.Add(Event(tenantId, record.Id, "final_state_decision_rejected", request.ActorId, $"Attempted {eventType} after final status {record.Status}."));
            await RecordLifecycleEvidenceAsync(tenantId, record, "final_state_decision_rejected", false, true, cancellationToken);
            dbContext.OperationalRequestIdempotencyRecords.Add(new OperationalRequestIdempotencyRecord
            {
                TenantId = tenantId,
                Command = command,
                ClientRequestId = request.ClientRequestId,
                Fingerprint = fingerprint,
                RequestId = record.Id,
                CreatedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(record, ["final_state_decision_rejected"]) with
            {
                Status = "FinalStateRejected",
                VisibleSummary = $"Request is already final as {record.Status}; duplicate final outcome was rejected."
            };
        }

        record.Status = status;
        record.VisibleSummary = summary;
        record.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalRequestEvents.Add(Event(tenantId, record.Id, eventType, request.ActorId, request.Reason));
        await RecordLifecycleEvidenceAsync(tenantId, record, eventType, true, status == "NeedsReview", cancellationToken);
        dbContext.OperationalRequestIdempotencyRecords.Add(new OperationalRequestIdempotencyRecord
        {
            TenantId = tenantId,
            Command = command,
            ClientRequestId = request.ClientRequestId,
            Fingerprint = fingerprint,
            RequestId = record.Id,
            CreatedAt = DateTimeOffset.UtcNow
        });
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

    private async Task RecordLifecycleEvidenceAsync(string tenantId, OperationalRequestRecord record, string eventType, bool notificationEligible, bool reviewRequired, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        dbContext.OperationalRequestStatusEvents.Add(new OperationalRequestStatusEvent
        {
            TenantId = tenantId,
            RequestId = record.Id,
            TrackingReference = record.TrackingReference,
            StudentProfileId = record.StudentProfileId,
            RequestType = record.RequestType,
            Status = record.Status,
            SourceEventType = eventType,
            NotificationEligible = notificationEligible,
            ReviewRequired = reviewRequired || record.Status == "NeedsReview",
            OccurredAt = now,
            AvailableForNotificationsAt = now.AddMinutes(2)
        });

        var summary = await dbContext.OperationalRequestReviewSummaries
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.RequestId == record.Id, cancellationToken);
        if (summary is null)
        {
            summary = new OperationalRequestReviewSummary
            {
                TenantId = tenantId,
                RequestId = record.Id,
                CreatedAt = now
            };
            dbContext.OperationalRequestReviewSummaries.Add(summary);
        }

        summary.TrackingReference = record.TrackingReference;
        summary.StudentProfileId = record.StudentProfileId;
        summary.RequestType = record.RequestType;
        summary.Status = record.Status;
        summary.CurrentAssignee = record.Status is "PendingApproval" or "NeedsReview" ? "workflow-reviewer" : "none";
        summary.ExceptionState = summary.CurrentAssignee == "workflow-reviewer" && eventType != "submitted" ? eventType : "none";
        summary.StarOutcome = record.RequestType == "outing" ? "not-evaluated" : "not-required";
        summary.LastEventType = eventType;
        summary.UpdatedAt = now;
    }

    private async Task<OperationalRequestRecord?> FindOverlappingActiveRequestAsync(string tenantId, RequestSubmissionRequest request, string normalizedType, CancellationToken cancellationToken)
    {
        if (request.StartsAt is null || request.EndsAt is null) return null;
        var startsAt = request.StartsAt.Value;
        var endsAt = request.EndsAt.Value;

        return await dbContext.OperationalRequests.AsNoTracking()
            .Where(x => x.TenantId == tenantId
                && x.StudentProfileId == request.StudentProfileId
                && x.RequestType == normalizedType
                && ActiveStatuses.Contains(x.Status)
                && x.StartsAt != null
                && x.EndsAt != null
                && x.StartsAt < endsAt
                && startsAt < x.EndsAt
                && (x.StartsAt != request.StartsAt || x.EndsAt != request.EndsAt))
            .OrderByDescending(x => x.UpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static RequestResponse ToResponse(OperationalRequestRecord record, IReadOnlyList<string>? extraAudit = null)
    {
        var audit = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraAudit ?? []).Distinct().ToList();
        return new RequestResponse(record.Id.ToString("N"), record.TrackingReference, record.RequestType, record.Status, record.Priority, record.StudentProfileId, record.VisibleSummary, record.StartsAt, record.EndsAt, audit);
    }

    private static RequestResponse Missing(string requestId) => new(requestId, "unknown", "unknown", "NotFound", "Unknown", "", "Request was not found in this tenant.", null, null, ["tenant-checked", "not-found"]);
    private static RequestResponse ValidationFailed(string message) => new("validation", "validation", "unknown", "ValidationFailed", "Unknown", "", message, null, null, ["validation_failed"]);

    private static RequestResponse? ValidateSubmission(RequestSubmissionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.StudentProfileId)) return ValidationFailed("Student profile id is required.");
        if (string.IsNullOrWhiteSpace(request.RequestType)) return ValidationFailed("Request type is required.");
        if (!AllowedRequestTypes.Contains(NormalizeType(request.RequestType))) return ValidationFailed("Request type is not enabled for operational requests.");
        if (string.IsNullOrWhiteSpace(request.Reason)) return ValidationFailed("Request reason is required.");
        if (string.IsNullOrWhiteSpace(request.RequestedOutcome)) return ValidationFailed("Requested outcome is required.");
        if (string.IsNullOrWhiteSpace(request.ClientRequestId)) return ValidationFailed("Client request id is required for idempotency.");
        if (request.StartsAt is not null && request.EndsAt is not null && request.StartsAt >= request.EndsAt) return ValidationFailed("Request start time must be before end time.");
        if (NormalizeType(request.RequestType) == "early-leave" && request.StartsAt is null) return ValidationFailed("Early leave requests require a release date and time.");
        return null;
    }

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

public static class RequestPermissions
{
    public const string Create = "requests.requests.create";
    public const string Read = "requests.requests.read";
    public const string GuardianRead = "requests.guardian_history.read";
    public const string StudentRead = "requests.student_history.read";
    public const string Withdraw = "requests.requests.withdraw";
    public const string Decide = "requests.workflow.decide";
    public const string ReleaseRead = "requests.release.read";
    public const string HistoryRead = "requests.history.read";
    public const string Configure = "requests.configuration.manage";
    public const string AuditRead = "requests.audit.read";
}
