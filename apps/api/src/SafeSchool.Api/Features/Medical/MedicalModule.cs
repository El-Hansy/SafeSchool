using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.FeatureFlags;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Medical;

public static class MedicalModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/medical";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/medical";
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
        var student = endpoints.MapGroup(StudentRoutePrefix);

        school.MapGet("/", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct))).RequireCapability(MedicalCapabilities.Records);
        school.MapGet("/records", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "profile", ct))).RequireCapability(MedicalCapabilities.Records);
        school.MapPost("/records", async (string schoolAccountId, MedicalRecordRequest request, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.UpsertProfileAsync(schoolAccountId, request, ct))).RequireCapability(MedicalCapabilities.Records);
        school.MapGet("/records/{studentProfileId}", async (string schoolAccountId, string studentProfileId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.StudentRecordsAsync(schoolAccountId, studentProfileId, ct))).RequireCapability(MedicalCapabilities.Records);
        school.MapGet("/emergency", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "emergency-access", ct))).RequireCapability(MedicalCapabilities.EmergencyAccess);
        school.MapPost("/emergency/access", async (string schoolAccountId, EmergencyAccessRequest request, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.OpenEmergencyAccessAsync(schoolAccountId, request, false, ct))).RequireCapability(MedicalCapabilities.EmergencyAccess);
        school.MapPost("/emergency/break-glass", async (string schoolAccountId, EmergencyAccessRequest request, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.OpenEmergencyAccessAsync(schoolAccountId, request, true, ct))).RequireCapability(MedicalCapabilities.EmergencyAccess);
        school.MapGet("/incidents", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "incident", ct))).RequireCapability(MedicalCapabilities.IncidentLogging);
        school.MapPost("/incidents", async (string schoolAccountId, MedicalIncidentRequest request, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.LogIncidentAsync(schoolAccountId, request, ct))).RequireCapability(MedicalCapabilities.IncidentLogging);
        school.MapGet("/notifications", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ByTypeAsync(schoolAccountId, "notification", ct))).RequireCapability(MedicalCapabilities.Notifications);
        school.MapPost("/notifications", async (string schoolAccountId, MedicalNotificationRequest request, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.CreateNotificationAsync(schoolAccountId, request, ct))).RequireCapability(MedicalCapabilities.Notifications);
        school.MapGet("/history", async (string schoolAccountId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.HistoryAsync(schoolAccountId, ct))).RequireCapability(MedicalCapabilities.History);
        school.MapGet("/configuration", (MedicalWorkflowService service) => Results.Ok(service.Configuration())).RequireCapability(MedicalCapabilities.Configuration);
        school.MapPost("/reviews/{recordId}/resolve", async (string schoolAccountId, string recordId, MedicalActionRequest request, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.ResolveReviewAsync(schoolAccountId, recordId, request, ct))).RequireCapability(MedicalCapabilities.History);
        school.MapGet("/trace/{recordId}", async (string schoolAccountId, string recordId, MedicalWorkflowService service, CancellationToken ct) => Results.Ok(await service.TraceAsync(schoolAccountId, recordId, ct))).RequireCapability(MedicalCapabilities.History);

        guardian.MapGet("/", async (ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct))).RequireCapability(MedicalCapabilities.Records);
        guardian.MapPost("/updates", async (MedicalRecordRequest request, ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.GuardianUpdateAsync(GuardianTenantResolver.Resolve(tenantContext), request, ct))).RequireCapability(MedicalCapabilities.Records);

        student.MapGet("/", async (ITenantContext tenantContext, MedicalWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.AudienceSummaryAsync(GuardianTenantResolver.Resolve(tenantContext), "student", ct))).RequireCapability(MedicalCapabilities.Records);
        return endpoints;
    }
}

public sealed record MedicalRecordRequest(string StudentProfileId, string Summary, string RestrictedDetail, string ClientRequestId, string Severity = "Routine");
public sealed record EmergencyAccessRequest(string StudentProfileId, string Reason, string ActorId, string ClientRequestId);
public sealed record MedicalIncidentRequest(string StudentProfileId, string Severity, string Observation, string CareAction, string ClientRequestId);
public sealed record MedicalNotificationRequest(string StudentProfileId, string SourceReference, string Urgency, string Audience, string ClientRequestId);
public sealed record MedicalActionRequest(string Action, string Reason, string ActorId = "medical-reviewer", string ClientRequestId = "medical-action");
public sealed record MedicalResponse(string MedicalRecordId, string RecordReference, string StudentProfileId, string RecordType, string Status, string Severity, string VisibleSummary, DateTimeOffset? ExpiresAt, IReadOnlyList<string> AuditTrail);

public sealed class MedicalWorkflowService(SafeSchoolDbContext dbContext)
{
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
            notifications = await records.CountAsync(x => x.RecordType == "notification", cancellationToken)
        };
    }

    public Task<IReadOnlyList<MedicalResponse>> ByTypeAsync(string tenantId, string recordType, CancellationToken cancellationToken = default) =>
        ListByTypeAsync(tenantId, recordType, cancellationToken);

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

    public async Task<IReadOnlyList<MedicalResponse>> AudienceSummaryAsync(string tenantId, string audience, CancellationToken cancellationToken = default)
    {
        var allowedTypes = audience == "guardian" ? new[] { "profile", "notification" } : ["profile"];
        var records = await dbContext.OperationalMedicalRecords.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && allowedTypes.Contains(x.RecordType))
            .OrderByDescending(x => x.UpdatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x, ["minimum-necessary-view"])).ToList();
    }

    public Task<IReadOnlyList<MedicalResponse>> HistoryAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListAllAsync(tenantId, cancellationToken);

    public object Configuration() => new { emergencyAccessMinutes = 30, offlineCacheHours = 24, breakGlassRoles = new[] { "school-nurse", "emergency-authorized-staff" }, evidence = new[] { "tenant-scoped", "privacy-reviewed", "audit-required" } };

    public Task<MedicalResponse> UpsertProfileAsync(string tenantId, MedicalRecordRequest request, CancellationToken cancellationToken = default) =>
        CreateAsync(tenantId, "profile", request.ClientRequestId, request.StudentProfileId, "Verified", request.Severity, request.Summary, request.RestrictedDetail, ["medical-profile-updated", "guardian-visibility-reviewed"], null, cancellationToken);

    public Task<MedicalResponse> GuardianUpdateAsync(string tenantId, MedicalRecordRequest request, CancellationToken cancellationToken = default) =>
        CreateAsync(tenantId, "profile", request.ClientRequestId, request.StudentProfileId, "PendingMedicalReview", request.Severity, "Guardian medical update submitted for school medical review.", request.RestrictedDetail, ["guardian-update-submitted", "review-required"], null, cancellationToken);

    public Task<MedicalResponse> OpenEmergencyAccessAsync(string tenantId, EmergencyAccessRequest request, bool breakGlass, CancellationToken cancellationToken = default)
    {
        var events = breakGlass
            ? new[] { "break-glass-confirmed", "minimum-necessary-data-opened", "mandatory-review-created" }
            : ["emergency-access-opened", "minimum-necessary-data-opened"];
        return CreateAsync(tenantId, "emergency-access", request.ClientRequestId, request.StudentProfileId, "Open", "Urgent", $"Emergency access opened for {request.StudentProfileId}.", request.Reason, events, DateTimeOffset.UtcNow.AddMinutes(30), cancellationToken);
    }

    public Task<MedicalResponse> LogIncidentAsync(string tenantId, MedicalIncidentRequest request, CancellationToken cancellationToken = default) =>
        CreateAsync(tenantId, "incident", request.ClientRequestId, request.StudentProfileId, "Open", request.Severity, $"Medical incident logged: {request.CareAction}.", request.Observation, ["incident-logged", "care-action-recorded", "no-diagnosis-created"], null, cancellationToken);

    public Task<MedicalResponse> CreateNotificationAsync(string tenantId, MedicalNotificationRequest request, CancellationToken cancellationToken = default) =>
        CreateAsync(tenantId, "notification", request.ClientRequestId, request.StudentProfileId, "Queued", request.Urgency, $"Medical notification queued for {request.Audience}.", request.SourceReference, ["audience-minimized", "contact-attempt-required"], null, cancellationToken);

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
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
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
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record, ["tenant-checked", "feature-checked"]);
    }

    private async Task<MedicalResponse> TransitionAsync(string tenantId, string recordId, string status, string summary, string eventType, MedicalActionRequest request, CancellationToken cancellationToken)
    {
        var record = await FindAsync(tenantId, recordId, cancellationToken);
        if (record is null) return Missing(recordId);
        record.Status = status;
        record.VisibleSummary = summary;
        record.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.OperationalMedicalEvents.Add(Event(tenantId, record.Id, eventType, request.ActorId, request.Reason));
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

    private async Task<IReadOnlyList<MedicalResponse>> ListAllAsync(string tenantId, CancellationToken cancellationToken)
    {
        var records = await dbContext.OperationalMedicalRecords.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId)
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

    private static MedicalResponse ToResponse(OperationalMedicalRecord record, IReadOnlyList<string>? extraAudit = null)
    {
        var audit = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraAudit ?? []).Distinct().ToList();
        return new MedicalResponse(record.Id.ToString("N"), record.RecordReference, record.StudentProfileId, record.RecordType, record.Status, record.Severity, record.VisibleSummary, record.ExpiresAt, audit);
    }

    private static MedicalResponse Missing(string recordId) => new(recordId, "unknown", "", "unknown", "NotFound", "Unknown", "Medical record was not found in this tenant.", null, ["tenant-checked", "not-found"]);

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
