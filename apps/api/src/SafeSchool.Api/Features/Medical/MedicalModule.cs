using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Medical;

public static class MedicalModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/medical";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/medical";
    public const string GuardianStudentRoutePrefix = "/api/v1/guardians/me/students/{studentProfileId}/medical";
    public const string StudentRoutePrefix = "/api/v1/students/me/medical";

    public static IServiceCollection AddMedicalFeature(this IServiceCollection services)
    {
        services.AddScoped<MedicalWorkflowService>();
        services.AddScoped<MedicalBoundaryGuard>();
        return services;
    }

    public static IEndpointRouteBuilder MapMedicalEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var school = endpoints.MapGroup(SchoolRoutePrefix);
        var guardian = endpoints.MapGroup(GuardianRoutePrefix);
        var guardianStudent = endpoints.MapGroup(GuardianStudentRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);

        school.MapGet("/", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.RecordsRead);
        school.MapGet("/records", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "profile", ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.RecordsRead);
        school.MapPost("/records", async (string schoolAccountId, MedicalRecordRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.UpsertProfileAsync(schoolAccountId, request, ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.RecordsManage);
        school.MapGet("/records/{studentProfileId}", async (string schoolAccountId, string studentProfileId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.StudentRecordsAsync(schoolAccountId, studentProfileId, ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.RecordsRead);
        school.MapGet("/emergency", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "emergency-access", ct))).RequireCapability(MedicalCapabilities.EmergencyAccess, MedicalPermissions.EmergencyRead);
        school.MapPost("/emergency/access", async (string schoolAccountId, EmergencyAccessRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.OpenEmergencyAccessAsync(schoolAccountId, request, false, ct))).RequireCapability(MedicalCapabilities.EmergencyAccess, MedicalPermissions.EmergencyRead);
        school.MapPost("/emergency/break-glass", async (string schoolAccountId, EmergencyAccessRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.OpenEmergencyAccessAsync(schoolAccountId, request, true, ct))).RequireCapability(MedicalCapabilities.EmergencyAccess, MedicalPermissions.BreakGlass);
        school.MapPost("/emergency/{recordId}/reconfirm", async (string schoolAccountId, string recordId, MedicalActionRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.ReconfirmEmergencyAccessAsync(schoolAccountId, recordId, request, ct))).RequireCapability(MedicalCapabilities.EmergencyAccess, MedicalPermissions.EmergencyRead);
        school.MapPost("/emergency/{recordId}/close", async (string schoolAccountId, string recordId, MedicalActionRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.CloseEmergencyAccessAsync(schoolAccountId, recordId, request, ct))).RequireCapability(MedicalCapabilities.EmergencyAccess, MedicalPermissions.EmergencyRead);
        school.MapGet("/incidents", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "incident", ct))).RequireCapability(MedicalCapabilities.IncidentLogging, MedicalPermissions.IncidentsRead);
        school.MapPost("/incidents", async (string schoolAccountId, MedicalIncidentRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.LogIncidentAsync(schoolAccountId, request, ct))).RequireCapability(MedicalCapabilities.IncidentLogging, MedicalPermissions.IncidentsCreate);
        school.MapGet("/notifications", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "notification", ct))).RequireCapability(MedicalCapabilities.Notifications, MedicalPermissions.NotificationsRead);
        school.MapPost("/notifications", async (string schoolAccountId, MedicalNotificationRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.CreateNotificationAsync(schoolAccountId, request, ct))).RequireCapability(MedicalCapabilities.Notifications, MedicalPermissions.NotificationsCreate);
        school.MapGet("/history", async (string schoolAccountId, string? studentProfileId, string? recordType, string? status, string? severity, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.HistoryAsync(schoolAccountId, new MedicalHistoryFilter(studentProfileId, recordType, status, severity), ct))).RequireCapability(MedicalCapabilities.History, MedicalPermissions.HistoryRead);
        school.MapGet("/status-events", async (string schoolAccountId, string? studentProfileId, string? recordType, string? status, string? severity, string? sourceEventType, bool? notificationEligible, bool? reviewRequired, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.StatusEventsAsync(schoolAccountId, new MedicalStatusEventFilter(studentProfileId, recordType, status, severity, sourceEventType, notificationEligible, reviewRequired), ct))).RequireCapability(MedicalCapabilities.History, MedicalPermissions.HistoryRead);
        school.MapGet("/review-summaries", async (string schoolAccountId, string? studentProfileId, string? recordType, string? status, string? severity, string? reviewState, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.ReviewSummariesAsync(schoolAccountId, new MedicalReviewSummaryFilter(studentProfileId, recordType, status, severity, reviewState), ct))).RequireCapability(MedicalCapabilities.History, MedicalPermissions.AuditRead);
        school.MapGet("/configuration", (MedicalWorkflowService service) => Results.Ok(service.Configuration())).RequireCapability(MedicalCapabilities.Configuration, MedicalPermissions.ConfigurationManage);
        school.MapPost("/reviews/{recordId}/resolve", async (string schoolAccountId, string recordId, MedicalActionRequest request, MedicalWorkflowService service, CancellationToken ct) => ToEndpointResult(await service.ResolveReviewAsync(schoolAccountId, recordId, request, ct))).RequireCapability(MedicalCapabilities.History, MedicalPermissions.ReviewManage);
        school.MapGet("/trace/{recordId}", async (string schoolAccountId, string recordId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.TraceAsync(schoolAccountId, recordId, ct))).RequireCapability(MedicalCapabilities.History, MedicalPermissions.AuditRead);

        guardian.MapGet("/", async (ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.GuardianRead);
        guardian.MapPost("/updates", async (MedicalRecordRequest request, ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            ToEndpointResult(await service.GuardianUpdateAsync(GuardianTenantResolver.Resolve(tenantContext), request, ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.GuardianUpdate);
        guardianStudent.MapGet("/", async (string studentProfileId, ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct, studentProfileId))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.GuardianRead);
        guardianStudent.MapPost("/updates", async (string studentProfileId, MedicalRecordRequest request, ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            ToEndpointResult(await service.GuardianUpdateAsync(GuardianTenantResolver.Resolve(tenantContext), request with { StudentProfileId = studentProfileId }, ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.GuardianUpdate);

        student.MapGet("/", async (ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "student", ct))).RequireCapability(MedicalCapabilities.Records, MedicalPermissions.StudentRead);
        return endpoints;
    }

    private static IResult ToEndpointResult(MedicalResponse response) => response.Status switch
    {
        "ValidationFailed" => Results.BadRequest(response),
        "AccessDenied" => Results.Forbid(),
        "DuplicateBlocked" => Results.Conflict(response),
        "NotFound" => Results.NotFound(response),
        _ => Results.Ok(response)
    };
}

public sealed record MedicalRecordRequest(string StudentProfileId, string Summary, string RestrictedDetail, string ClientRequestId, string Severity = "Routine");
public sealed record EmergencyAccessRequest(string StudentProfileId, string Reason, string ActorId, string ClientRequestId, string ActorRole = "school-nurse", bool Confirmed = true);
public sealed record MedicalIncidentRequest(string StudentProfileId, string Severity, string Observation, string CareAction, string ClientRequestId);
public sealed record MedicalNotificationRequest(string StudentProfileId, string SourceReference, string Urgency, string Audience, string ClientRequestId);
public sealed record MedicalActionRequest(string Action, string Reason, string ActorId = "medical-reviewer", string ClientRequestId = "medical-action");
public sealed record MedicalResponse(string MedicalRecordId, string RecordReference, string StudentProfileId, string RecordType, string Status, string Severity, string VisibleSummary, DateTimeOffset? ExpiresAt, IReadOnlyList<string> AuditTrail);
public sealed record MedicalHistoryFilter(string? StudentProfileId = null, string? RecordType = null, string? Status = null, string? Severity = null);
public sealed record MedicalStatusEventFilter(string? StudentProfileId = null, string? RecordType = null, string? Status = null, string? Severity = null, string? SourceEventType = null, bool? NotificationEligible = null, bool? ReviewRequired = null);
public sealed record MedicalReviewSummaryFilter(string? StudentProfileId = null, string? RecordType = null, string? Status = null, string? Severity = null, string? ReviewState = null);

public sealed class MedicalWorkflowService(SafeSchoolDbContext dbContext)
{
    private static readonly string[] ActiveStatuses = ["Verified", "PendingMedicalReview", "Open", "Queued", "ManualReviewRequired"];
    private static readonly string[] EmergencyRoles = ["school-nurse", "emergency-authorized-staff", "clinic-staff"];

    public async Task<object> BoardAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var records = dbContext.OperationalMedicalRecords.AsNoTracking().Where(x => x.TenantId == tenantId);
        return new
        {
            schoolAccountId = tenantId,
            phase = "medical-emergency",
            status = "operational",
            capabilities = MedicalCapabilities.All,
            profiles = await records.CountAsync(x => x.RecordType == "profile", cancellationToken),
            openEmergencySessions = await records.CountAsync(x => x.RecordType == "emergency-access" && x.Status == "Open", cancellationToken),
            incidents = await records.CountAsync(x => x.RecordType == "incident", cancellationToken),
            notifications = await records.CountAsync(x => x.RecordType == "notification", cancellationToken),
            statusEvents = await dbContext.OperationalMedicalStatusEvents.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken),
            reviewSummaries = await dbContext.OperationalMedicalReviewSummaries.AsNoTracking().CountAsync(x => x.TenantId == tenantId, cancellationToken)
        };
    }

    public Task<IReadOnlyList<MedicalResponse>> ByTypeAsync(string tenantId, string recordType, CancellationToken cancellationToken = default) =>
        ListByTypeAsync(tenantId, recordType, cancellationToken);

    public Task<IReadOnlyList<OperationalMedicalStatusEvent>> StatusEventsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        StatusEventsAsync(tenantId, new MedicalStatusEventFilter(), cancellationToken);

    public async Task<IReadOnlyList<OperationalMedicalStatusEvent>> StatusEventsAsync(string tenantId, MedicalStatusEventFilter filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.OperationalMedicalStatusEvents.AsNoTracking()
            .Where(x => x.TenantId == tenantId);
        if (!string.IsNullOrWhiteSpace(filter.StudentProfileId)) query = query.Where(x => x.StudentProfileId == filter.StudentProfileId);
        if (!string.IsNullOrWhiteSpace(filter.RecordType)) query = query.Where(x => x.RecordType == filter.RecordType);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.Severity)) query = query.Where(x => x.Severity == filter.Severity);
        if (!string.IsNullOrWhiteSpace(filter.SourceEventType)) query = query.Where(x => x.SourceEventType == filter.SourceEventType);
        if (filter.NotificationEligible is not null) query = query.Where(x => x.NotificationEligible == filter.NotificationEligible);
        if (filter.ReviewRequired is not null) query = query.Where(x => x.ReviewRequired == filter.ReviewRequired);

        return await query
            .OrderByDescending(x => x.OccurredAt)
            .Take(100)
            .ToListAsync(cancellationToken);
    }

    public Task<IReadOnlyList<OperationalMedicalReviewSummary>> ReviewSummariesAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ReviewSummariesAsync(tenantId, new MedicalReviewSummaryFilter(), cancellationToken);

    public async Task<IReadOnlyList<OperationalMedicalReviewSummary>> ReviewSummariesAsync(string tenantId, MedicalReviewSummaryFilter filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.OperationalMedicalReviewSummaries.AsNoTracking()
            .Where(x => x.TenantId == tenantId);
        if (!string.IsNullOrWhiteSpace(filter.StudentProfileId)) query = query.Where(x => x.StudentProfileId == filter.StudentProfileId);
        if (!string.IsNullOrWhiteSpace(filter.RecordType)) query = query.Where(x => x.RecordType == filter.RecordType);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.Severity)) query = query.Where(x => x.Severity == filter.Severity);
        if (!string.IsNullOrWhiteSpace(filter.ReviewState)) query = query.Where(x => x.ReviewState == filter.ReviewState);

        return await query
            .OrderByDescending(x => x.UpdatedAt)
            .Take(100)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MedicalResponse>> StudentRecordsAsync(string tenantId, string studentProfileId, CancellationToken cancellationToken = default)
    {
        var records = await dbContext.OperationalMedicalRecords.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && x.StudentProfileId == studentProfileId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    public async Task<IReadOnlyList<MedicalResponse>> AudienceSummaryAsync(string tenantId, string audience, CancellationToken cancellationToken = default, string? studentProfileId = null)
    {
        var allowedTypes = audience == "guardian" ? new[] { "profile", "notification" } : ["profile"];
        var query = dbContext.OperationalMedicalRecords.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && allowedTypes.Contains(x.RecordType));
        if (!string.IsNullOrWhiteSpace(studentProfileId))
        {
            query = query.Where(x => x.StudentProfileId == studentProfileId);
        }

        var records = await query
            .OrderByDescending(x => x.UpdatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x, ["minimum-necessary-view"])).ToList();
    }

    public Task<IReadOnlyList<MedicalResponse>> HistoryAsync(string tenantId, CancellationToken cancellationToken = default) =>
        HistoryAsync(tenantId, new MedicalHistoryFilter(), cancellationToken);

    public Task<IReadOnlyList<MedicalResponse>> HistoryAsync(string tenantId, MedicalHistoryFilter filter, CancellationToken cancellationToken = default) =>
        ListAllAsync(tenantId, filter, cancellationToken);

    public object Configuration() => new { emergencyAccessMinutes = 30, offlineCacheHours = 24, breakGlassRoles = new[] { "school-nurse", "emergency-authorized-staff" }, evidence = new[] { "tenant-scoped", "privacy-reviewed", "audit-required" } };

    public async Task<MedicalResponse> UpsertProfileAsync(string tenantId, MedicalRecordRequest request, CancellationToken cancellationToken = default)
    {
        var validation = ValidateMedicalRecord(request);
        if (validation is not null) return validation;

        var existing = await dbContext.OperationalMedicalRecords
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId
                && x.StudentProfileId == request.StudentProfileId
                && x.RecordType == "profile"
                && ActiveStatuses.Contains(x.Status))
            .OrderByDescending(x => x.UpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is null)
        {
            return await CreateAsync(tenantId, "profile", request.ClientRequestId, request.StudentProfileId, "Verified", request.Severity, request.Summary, request.RestrictedDetail, ["medical-profile-updated", "guardian-visibility-reviewed"], null, cancellationToken);
        }

        existing.Status = "Verified";
        existing.Severity = request.Severity;
        existing.VisibleSummary = request.Summary;
        existing.RestrictedDetail = request.RestrictedDetail;
        existing.ClientRequestId = request.ClientRequestId;
        existing.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalMedicalEvents.Add(Event(tenantId, existing.Id, "medical-profile-upserted", "medical-operator", "Existing active profile updated with preserved history."));
        await RecordLifecycleEvidenceAsync(tenantId, existing, "medical-profile-upserted", false, false, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(existing, ["profile-upserted", "tenant-checked", "feature-checked"]);
    }

    public Task<MedicalResponse> GuardianUpdateAsync(string tenantId, MedicalRecordRequest request, CancellationToken cancellationToken = default)
    {
        var validation = ValidateMedicalRecord(request);
        return validation is not null
            ? Task.FromResult(validation)
            : CreateAsync(tenantId, "profile", request.ClientRequestId, request.StudentProfileId, "PendingMedicalReview", request.Severity, "Guardian medical update submitted for school medical review.", request.RestrictedDetail, ["guardian-update-submitted", "review-required"], null, cancellationToken);
    }

    public Task<MedicalResponse> OpenEmergencyAccessAsync(string tenantId, EmergencyAccessRequest request, bool breakGlass, CancellationToken cancellationToken = default)
    {
        var validation = ValidateEmergencyAccess(request, breakGlass);
        if (validation is not null) return Task.FromResult(validation);

        var events = breakGlass
            ? new[] { "break-glass-confirmed", "minimum-necessary-data-opened", "mandatory-review-created" }
            : ["emergency-access-opened", "minimum-necessary-data-opened"];
        return CreateAsync(tenantId, "emergency-access", request.ClientRequestId, request.StudentProfileId, "Open", "Urgent", $"Emergency access opened for {request.StudentProfileId}.", request.Reason, events, DateTimeOffset.UtcNow.AddMinutes(30), cancellationToken);
    }

    public Task<MedicalResponse> LogIncidentAsync(string tenantId, MedicalIncidentRequest request, CancellationToken cancellationToken = default)
    {
        var validation = ValidateIncident(request);
        if (validation is not null) return Task.FromResult(validation);

        var events = IsHighSeverity(request.Severity)
            ? new[] { "incident-logged", "care-action-recorded", "no-diagnosis-created", "notification-eligibility-exported" }
            : ["incident-logged", "care-action-recorded", "no-diagnosis-created"];
        return CreateAsync(tenantId, "incident", request.ClientRequestId, request.StudentProfileId, "Open", request.Severity, $"Medical incident logged: {request.CareAction}.", request.Observation, events, null, cancellationToken);
    }

    public Task<MedicalResponse> CreateNotificationAsync(string tenantId, MedicalNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var validation = ValidateNotification(request);
        if (validation is not null) return Task.FromResult(validation);

        var audience = string.IsNullOrWhiteSpace(request.Audience) && IsHighSeverity(request.Urgency)
            ? "approved guardians, emergency contacts, assigned nurse, school emergency coordinator"
            : request.Audience;
        return CreateAsync(tenantId, "notification", request.ClientRequestId, request.StudentProfileId, "Queued", request.Urgency, $"Medical notification queued for {audience}.", request.SourceReference, ["audience-minimized", "contact-attempt-required"], null, cancellationToken);
    }

    public Task<MedicalResponse> ReconfirmEmergencyAccessAsync(string tenantId, string recordId, MedicalActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, recordId, "Open", "Emergency access re-confirmed for another 30-minute window.", "emergency-access-reconfirmed", request, cancellationToken, DateTimeOffset.UtcNow.AddMinutes(30), expectedRecordType: "emergency-access");

    public Task<MedicalResponse> CloseEmergencyAccessAsync(string tenantId, string recordId, MedicalActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, recordId, "Closed", "Emergency access session closed with evidence preserved.", "emergency-access-closed", request, cancellationToken, expectedRecordType: "emergency-access");

    public Task<MedicalResponse> ResolveReviewAsync(string tenantId, string recordId, MedicalActionRequest request, CancellationToken cancellationToken = default) =>
        TransitionAsync(tenantId, recordId, "Reviewed", "Medical review resolved with original evidence preserved.", "review-resolved", request, cancellationToken);

    public async Task<object> TraceAsync(string tenantId, string recordId, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, recordId, cancellationToken);
        if (record is null) return new { traceId = $"trace-{recordId}", references = Array.Empty<string>(), status = "NotFound" };
        return new { traceId = $"trace-{record.RecordReference}", references = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).ToArray(), status = record.Status };
    }

    private async Task<MedicalResponse> CreateAsync(string tenantId, string recordType, string clientRequestId, string studentProfileId, string status, string severity, string visibleSummary, string restrictedDetail, string[] events, DateTimeOffset? expiresAt, CancellationToken cancellationToken)
    {
        var fingerprint = $"{recordType}|{studentProfileId}|{status}|{severity}|{visibleSummary}|{restrictedDetail}|{expiresAt}".ToUpperInvariant();
        var command = $"{tenantId}:{recordType}";
        var idempotency = await dbContext.OperationalMedicalIdempotencyRecords
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Command == command && x.ClientRequestId == clientRequestId, cancellationToken);
        if (idempotency is not null)
        {
            var existing = await LoadAsync(idempotency.MedicalRecordId, cancellationToken);
            if (existing is null) return Missing(clientRequestId);
            if (idempotency.Fingerprint == fingerprint) return ToResponse(existing, ["idempotency_duplicate"]);

            existing.Status = "ManualReviewRequired";
            existing.VisibleSummary = "Medical record requires manual review because the client request id was reused with different details.";
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            dbContext.OperationalMedicalEvents.Add(Event(tenantId, existing.Id, "idempotency_conflict", "system", "Client request id reused with different medical details."));
            await RecordLifecycleEvidenceAsync(tenantId, existing, "idempotency_conflict", false, true, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
        }

        var duplicate = await dbContext.OperationalMedicalRecords
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId
                && x.RecordType == recordType
                && x.StudentProfileId == studentProfileId
                && ActiveStatuses.Contains(x.Status)
                && x.VisibleSummary == visibleSummary
                && x.RestrictedDetail == restrictedDetail)
            .OrderByDescending(x => x.UpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (duplicate is not null)
        {
            dbContext.OperationalMedicalEvents.Add(Event(tenantId, duplicate.Id, "duplicate_blocked", "system", "Exact active duplicate medical command blocked."));
            await RecordLifecycleEvidenceAsync(tenantId, duplicate, "duplicate_blocked", false, true, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(duplicate, ["duplicate_blocked"]) with
            {
                Status = "DuplicateBlocked",
                VisibleSummary = "Exact active duplicate medical command was blocked and original evidence was preserved."
            };
        }

        var now = DateTimeOffset.UtcNow;
        var prefix = recordType switch
        {
            "emergency-access" => "EMG",
            "incident" => "INC",
            "notification" => "MEDN",
            _ => "MED"
        };
        var sequence = await dbContext.OperationalMedicalRecords.CountAsync(x => x.TenantId == tenantId && x.CreatedAt.Year == now.Year, cancellationToken) + 1;
        var record = new OperationalMedicalRecord
        {
            TenantId = tenantId,
            RecordReference = $"{prefix}-{now:yyyy}-{sequence:0000}",
            StudentProfileId = studentProfileId,
            RecordType = recordType,
            Status = status,
            Severity = severity,
            VisibleSummary = visibleSummary,
            RestrictedDetail = restrictedDetail,
            ClientRequestId = clientRequestId,
            ExpiresAt = expiresAt,
            CreatedAt = now,
            UpdatedAt = now
        };
        foreach (var eventType in events)
        {
            record.Events.Add(Event(tenantId, record.Id, eventType, "medical-operator", eventType));
        }

        dbContext.OperationalMedicalRecords.Add(record);
        dbContext.OperationalMedicalIdempotencyRecords.Add(new OperationalMedicalIdempotencyRecord
        {
            TenantId = tenantId,
            Command = command,
            ClientRequestId = clientRequestId,
            Fingerprint = fingerprint,
            MedicalRecordId = record.Id,
            CreatedAt = now
        });
        await RecordLifecycleEvidenceAsync(tenantId, record, events.Last(), IsNotificationEligible(record, events), IsReviewRequired(record, events), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, ["tenant-checked", "feature-checked"]);
    }

    private async Task<MedicalResponse> TransitionAsync(string tenantId, string recordId, string status, string summary, string eventType, MedicalActionRequest request, CancellationToken cancellationToken, DateTimeOffset? expiresAt = null, string? expectedRecordType = null)
    {
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            return ValidationFailed("Review or emergency action reason is required.");
        }

        var record = await FindAsync(tenantId, recordId, cancellationToken);
        if (record is null) return Missing(recordId);
        if (!string.IsNullOrWhiteSpace(expectedRecordType) && record.RecordType != expectedRecordType)
        {
            return ValidationFailed($"Action requires a {expectedRecordType} record.");
        }

        record.Status = status;
        record.VisibleSummary = summary;
        if (expiresAt is not null)
        {
            record.ExpiresAt = expiresAt;
        }

        record.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalMedicalEvents.Add(Event(tenantId, record.Id, eventType, request.ActorId, request.Reason));
        await RecordLifecycleEvidenceAsync(tenantId, record, eventType, IsNotificationEligible(record, [eventType]), IsReviewRequired(record, [eventType]), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, [request.Reason]);
    }

    private async Task<IReadOnlyList<MedicalResponse>> ListByTypeAsync(string tenantId, string recordType, CancellationToken cancellationToken)
    {
        var records = await dbContext.OperationalMedicalRecords.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && x.RecordType == recordType)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    private async Task<IReadOnlyList<MedicalResponse>> ListAllAsync(string tenantId, MedicalHistoryFilter filter, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalMedicalRecords.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId);
        if (!string.IsNullOrWhiteSpace(filter.StudentProfileId)) query = query.Where(x => x.StudentProfileId == filter.StudentProfileId);
        if (!string.IsNullOrWhiteSpace(filter.RecordType)) query = query.Where(x => x.RecordType == filter.RecordType);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.Severity)) query = query.Where(x => x.Severity == filter.Severity);

        var records = await query
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    private async Task<OperationalMedicalRecord?> FindAsync(string tenantId, string recordId, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalMedicalRecords.Include(x => x.Events).Where(x => x.TenantId == tenantId);
        if (Guid.TryParse(recordId, out var id)) return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        return await query.SingleOrDefaultAsync(x => x.RecordReference == recordId || x.ClientRequestId == recordId, cancellationToken);
    }

    private async Task<OperationalMedicalRecord?> LoadAsync(Guid recordId, CancellationToken cancellationToken) =>
        await dbContext.OperationalMedicalRecords.Include(x => x.Events).SingleOrDefaultAsync(x => x.Id == recordId, cancellationToken);

    private async Task RecordLifecycleEvidenceAsync(string tenantId, OperationalMedicalRecord record, string eventType, bool notificationEligible, bool reviewRequired, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        dbContext.OperationalMedicalStatusEvents.Add(new OperationalMedicalStatusEvent
        {
            TenantId = tenantId,
            MedicalRecordId = record.Id,
            RecordReference = record.RecordReference,
            StudentProfileId = record.StudentProfileId,
            RecordType = record.RecordType,
            Status = record.Status,
            Severity = record.Severity,
            SourceEventType = eventType,
            NotificationEligible = notificationEligible,
            ReviewRequired = reviewRequired,
            OccurredAt = now,
            AvailableForNotificationsAt = now.AddMinutes(2)
        });

        var summary = await dbContext.OperationalMedicalReviewSummaries
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.MedicalRecordId == record.Id, cancellationToken);
        if (summary is null)
        {
            summary = new OperationalMedicalReviewSummary
            {
                TenantId = tenantId,
                MedicalRecordId = record.Id,
                CreatedAt = now
            };
            dbContext.OperationalMedicalReviewSummaries.Add(summary);
        }

        summary.RecordReference = record.RecordReference;
        summary.StudentProfileId = record.StudentProfileId;
        summary.RecordType = record.RecordType;
        summary.Status = record.Status;
        summary.Severity = record.Severity;
        summary.ReviewState = reviewRequired ? eventType : "none";
        summary.LastEventType = eventType;
        summary.UpdatedAt = now;
    }

    private static MedicalResponse ToResponse(OperationalMedicalRecord record, IReadOnlyList<string>? extraAudit = null)
    {
        var audit = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraAudit ?? []).Distinct().ToList();
        return new MedicalResponse(record.Id.ToString("N"), record.RecordReference, record.StudentProfileId, record.RecordType, record.Status, record.Severity, record.VisibleSummary, record.ExpiresAt, audit);
    }

    private static MedicalResponse Missing(string recordId) => new(recordId, "unknown", "", "unknown", "NotFound", "Unknown", "Medical record was not found in this tenant.", null, ["tenant-checked", "not-found"]);
    private static MedicalResponse ValidationFailed(string message) => new("validation", "validation", "", "unknown", "ValidationFailed", "Unknown", message, null, ["validation_failed"]);
    private static MedicalResponse AccessDenied(string message) => new("denied", "denied", "", "emergency-access", "AccessDenied", "Urgent", message, null, ["access_denied"]);

    private static MedicalResponse? ValidateMedicalRecord(MedicalRecordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.StudentProfileId)) return ValidationFailed("Student profile id is required.");
        if (string.IsNullOrWhiteSpace(request.Summary)) return ValidationFailed("Medical summary is required.");
        if (string.IsNullOrWhiteSpace(request.RestrictedDetail)) return ValidationFailed("Restricted medical detail is required.");
        if (string.IsNullOrWhiteSpace(request.ClientRequestId)) return ValidationFailed("Client request id is required for idempotency.");
        return null;
    }

    private static MedicalResponse? ValidateEmergencyAccess(EmergencyAccessRequest request, bool breakGlass)
    {
        if (string.IsNullOrWhiteSpace(request.StudentProfileId)) return ValidationFailed("Student profile id is required.");
        if (string.IsNullOrWhiteSpace(request.Reason)) return ValidationFailed("Emergency reason is required.");
        if (string.IsNullOrWhiteSpace(request.ActorId)) return ValidationFailed("Emergency actor id is required.");
        if (string.IsNullOrWhiteSpace(request.ClientRequestId)) return ValidationFailed("Client request id is required for idempotency.");
        if (breakGlass && !request.Confirmed) return ValidationFailed("Break-glass access requires explicit confirmation.");
        if (breakGlass && !EmergencyRoles.Contains(request.ActorRole, StringComparer.OrdinalIgnoreCase)) return AccessDenied("Break-glass access is limited to configured emergency roles.");
        return null;
    }

    private static MedicalResponse? ValidateIncident(MedicalIncidentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.StudentProfileId)) return ValidationFailed("Student profile id is required.");
        if (string.IsNullOrWhiteSpace(request.Severity)) return ValidationFailed("Incident severity is required.");
        if (string.IsNullOrWhiteSpace(request.Observation)) return ValidationFailed("Incident observation is required.");
        if (string.IsNullOrWhiteSpace(request.CareAction)) return ValidationFailed("Care action is required.");
        if (string.IsNullOrWhiteSpace(request.ClientRequestId)) return ValidationFailed("Client request id is required for idempotency.");
        return null;
    }

    private static MedicalResponse? ValidateNotification(MedicalNotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.StudentProfileId)) return ValidationFailed("Student profile id is required.");
        if (string.IsNullOrWhiteSpace(request.SourceReference)) return ValidationFailed("Source incident or emergency reference is required.");
        if (string.IsNullOrWhiteSpace(request.Urgency)) return ValidationFailed("Notification urgency is required.");
        if (string.IsNullOrWhiteSpace(request.ClientRequestId)) return ValidationFailed("Client request id is required for idempotency.");
        if (string.IsNullOrWhiteSpace(request.Audience) && !IsHighSeverity(request.Urgency)) return ValidationFailed("Notification audience is required unless urgency is high.");
        return null;
    }

    private static bool IsHighSeverity(string severity) =>
        severity.Equals("High", StringComparison.OrdinalIgnoreCase)
        || severity.Equals("Critical", StringComparison.OrdinalIgnoreCase)
        || severity.Equals("Urgent", StringComparison.OrdinalIgnoreCase);

    private static bool IsNotificationEligible(OperationalMedicalRecord record, IReadOnlyList<string> events) =>
        record.RecordType == "notification"
        || record.RecordType == "incident" && IsHighSeverity(record.Severity)
        || events.Contains("notification-eligibility-exported");

    private static bool IsReviewRequired(OperationalMedicalRecord record, IReadOnlyList<string> events) =>
        record.Status is "PendingMedicalReview" or "ManualReviewRequired"
        || events.Contains("mandatory-review-created")
        || events.Contains("break-glass-confirmed")
        || events.Contains("idempotency_conflict")
        || events.Contains("duplicate_blocked");

    private static OperationalMedicalEvent Event(string tenantId, Guid recordId, string eventType, string actor, string reason) => new()
    {
        TenantId = tenantId,
        MedicalRecordId = recordId,
        EventType = eventType,
        ActorReference = string.IsNullOrWhiteSpace(actor) ? "system" : actor,
        Reason = reason,
        OccurredAt = DateTimeOffset.UtcNow
    };
}

public sealed class MedicalBoundaryGuard
{
    private static readonly string[] Blocked = ["diagnosis", "prescription", "attendance", "campus_gate", "transport", "wallet", "request_approval", "complaint_resolution", "broadcast", "document", "search"];
    public bool Allows(string sideEffect) => !Blocked.Contains(sideEffect, StringComparer.OrdinalIgnoreCase);
}

public static class MedicalCapabilities
{
    public const string Records = "medical.records";
    public const string EmergencyAccess = "medical.emergency_access";
    public const string IncidentLogging = "medical.incident_logging";
    public const string Notifications = "medical.notifications";
    public const string History = "medical.history";
    public const string Configuration = "medical.configuration";
    public static readonly string[] All = [Records, EmergencyAccess, IncidentLogging, Notifications, History, Configuration];
}

public static class MedicalPermissions
{
    public const string RecordsRead = "medical.records.read";
    public const string RecordsManage = "medical.records.manage";
    public const string GuardianRead = "medical.guardian_history.read";
    public const string GuardianUpdate = "medical.guardian_updates.submit";
    public const string StudentRead = "medical.student_summary.read";
    public const string EmergencyRead = "medical.emergency.read";
    public const string BreakGlass = "medical.emergency.break_glass";
    public const string IncidentsRead = "medical.incidents.read";
    public const string IncidentsCreate = "medical.incidents.create";
    public const string NotificationsRead = "medical.notifications.read";
    public const string NotificationsCreate = "medical.notifications.create";
    public const string HistoryRead = "medical.history.read";
    public const string ConfigurationManage = "medical.configuration.manage";
    public const string ReviewManage = "medical.reviews.manage";
    public const string AuditRead = "medical.audit.read";
}
