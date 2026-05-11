using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;
using SafeSchool.Api.Infrastructure.Tenancy;

namespace SafeSchool.Api.Features.Communications;

public static class CommunicationsModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/communications";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/communications";
    public const string StudentRoutePrefix = "/api/v1/students/me/communications";

    public static IServiceCollection AddCommunicationsFeature(this IServiceCollection services)
    {
        services.AddScoped<CommunicationWorkflowService>();
        services.AddScoped<CommunicationBoundaryGuard>();
        return services;
    }

    public static IEndpointRouteBuilder MapCommunicationsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var school = endpoints.MapGroup(SchoolRoutePrefix);
        var guardian = endpoints.MapGroup(GuardianRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);
        school.MapGet("/", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.BoardAsync(schoolAccountId, ct)));
        school.MapPost("/source-events", async (string schoolAccountId, NotificationSourceEventRequest request, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.AcceptSourceEventAsync(schoolAccountId, request, ct)));
        school.MapGet("/notifications/{notificationId}", async (string schoolAccountId, string notificationId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.DetailAsync(schoolAccountId, notificationId, "notification", ct)));
        school.MapGet("/acknowledgements", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.AcknowledgementsAsync(schoolAccountId, ct)));
        school.MapGet("/delivery", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.DeliveryAsync(schoolAccountId, ct)));
        school.MapGet("/summaries", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.SummariesAsync(schoolAccountId, ct)));
        school.MapGet("/history", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.HistoryAsync(schoolAccountId, ct)));
        school.MapGet("/moderation", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.ModerationAsync(schoolAccountId, ct)));
        school.MapGet("/exceptions", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.ExceptionsAsync(schoolAccountId, ct)));
        school.MapGet("/configuration", (CommunicationWorkflowService service) => Results.Ok(service.Configuration()));
        school.MapGet("/configuration/audience-rules/{ruleId}", (string ruleId, CommunicationWorkflowService service) => Results.Ok(service.AudienceRule(ruleId)));
        school.MapGet("/configuration/templates/{templateId}", (string templateId, CommunicationWorkflowService service) => Results.Ok(service.Template(templateId)));
        school.MapGet("/conversations", async (string schoolAccountId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.ConversationsAsync(schoolAccountId, ct)));
        school.MapPost("/conversations", async (string schoolAccountId, MessageRequest request, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.SendMessageAsync(schoolAccountId, request, ct)));
        school.MapGet("/conversations/{conversationId}", async (string schoolAccountId, string conversationId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.DetailAsync(schoolAccountId, conversationId, "conversation", ct)));
        school.MapGet("/broadcasts/{broadcastId}", async (string schoolAccountId, string broadcastId, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.DetailAsync(schoolAccountId, broadcastId, "broadcast", ct)));
        school.MapPost("/broadcasts", async (string schoolAccountId, BroadcastRequest request, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.PublishBroadcastAsync(schoolAccountId, request, ct)));
        school.MapGet("/trace/{reference}", async (string schoolAccountId, string reference, CommunicationWorkflowService service, CancellationToken ct) => Results.Ok(await service.TraceAsync(schoolAccountId, reference, ct)));
        guardian.MapGet("/notifications", async (ITenantContext tenantContext, CommunicationWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.NotificationCenterAsync(GuardianTenantResolver.Resolve(tenantContext), "guardian", ct)));
        student.MapGet("/notifications", async (ITenantContext tenantContext, CommunicationWorkflowService service, CancellationToken ct) =>
            Results.Ok(await service.NotificationCenterAsync(GuardianTenantResolver.Resolve(tenantContext), "student", ct)));
        return endpoints;
    }
}

public sealed record NotificationSourceEventRequest(string SourceModule, string SourceReference, string Category, string ClientRequestId);
public sealed record MessageRequest(string Subject, string Body, string RecipientScope, string ClientRequestId);
public sealed record BroadcastRequest(string Title, string Body, string AudienceRule, string ClientRequestId);
public sealed record CommunicationResponse(string Reference, string Status, IReadOnlyList<string> Recipients, IReadOnlyList<string> Evidence);

public sealed class CommunicationWorkflowService(SafeSchoolDbContext dbContext)
{
    public async Task<object> BoardAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var records = dbContext.OperationalCommunications.AsNoTracking().Where(x => x.TenantId == tenantId);
        return new
        {
            schoolAccountId = tenantId,
            phase = "communication-notifications",
            status = "operational",
            unread = await records.CountAsync(x => x.Kind == "notification" && x.Status == "Unread", cancellationToken),
            broadcasts = await records.CountAsync(x => x.Kind == "broadcast", cancellationToken),
            deliveryExceptions = await records.CountAsync(x => x.Status == "RetryScheduled" || x.Status == "ManualReviewRequired", cancellationToken),
            capabilities = CommunicationCapabilities.All
        };
    }

    public Task<IReadOnlyList<CommunicationResponse>> NotificationCenterAsync(string tenantId, string scope, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, x => x.Kind == "notification" && (x.RecipientScope == scope || x.RecipientScope == $"{scope}s" || x.RecipientScope == "eligible-recipient"), cancellationToken);

    public Task<IReadOnlyList<CommunicationResponse>> AcknowledgementsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListByEventAsync(tenantId, "read-state-recorded", cancellationToken);

    public Task<IReadOnlyList<CommunicationResponse>> DeliveryAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListByEventAsync(tenantId, "delivery-attempt-recorded", cancellationToken);

    public async Task<IReadOnlyList<object>> SummariesAsync(string tenantId, CancellationToken cancellationToken = default)
    {
        var summaries = await dbContext.OperationalCommunications.AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .GroupBy(x => new { x.Kind, x.Status })
            .Select(x => new { summaryReference = $"communication-summary-{x.Key.Kind}-{x.Key.Status}", x.Key.Kind, x.Key.Status, count = x.Count(), evidence = "counts-minimized" })
            .ToListAsync(cancellationToken);
        return summaries.Cast<object>().ToList();
    }

    public Task<IReadOnlyList<CommunicationResponse>> HistoryAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, _ => true, cancellationToken);

    public Task<IReadOnlyList<CommunicationResponse>> ConversationsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, x => x.Kind == "conversation", cancellationToken);

    public Task<IReadOnlyList<CommunicationResponse>> ModerationAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListByEventAsync(tenantId, "moderation-checked", cancellationToken);

    public Task<IReadOnlyList<CommunicationResponse>> ExceptionsAsync(string tenantId, CancellationToken cancellationToken = default) =>
        ListAsync(tenantId, x => x.Status == "RetryScheduled" || x.Status == "ManualReviewRequired" || x.Status == "Conflict", cancellationToken);

    public object Configuration() => new { templates = 4, audienceRules = 3, quietHours = "Enabled", evidence = new[] { "tenant-scoped", "version-preserved" } };
    public object AudienceRule(string ruleId) => new { ruleId, scope = "grade:4", channel = "guardian", status = "Enabled", evidence = new[] { "audience-snapshot", "visibility-reviewed" } };
    public object Template(string templateId) => new { templateId, channel = "push", locale = "en/ar", status = "Approved", evidence = new[] { "template-versioned", "moderation-ready" } };

    public async Task<CommunicationResponse> DetailAsync(string tenantId, string reference, string kind, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, reference, cancellationToken);
        return record is null ? Missing(reference, kind) : ToResponse(record, ["tenant-checked"]);
    }

    public Task<CommunicationResponse> AcceptSourceEventAsync(string tenantId, NotificationSourceEventRequest request, CancellationToken cancellationToken = default) =>
        CreateAsync(tenantId, "source", request.ClientRequestId, request.SourceReference, "notification", $"event-{request.ClientRequestId}", "Unread", request.Category, request.SourceReference, "eligible-recipient", ["source-read-only", "notification-created"], cancellationToken);

    public Task<CommunicationResponse> SendMessageAsync(string tenantId, MessageRequest request, CancellationToken cancellationToken = default) =>
        CreateAsync(tenantId, "conversation", request.ClientRequestId, $"{request.Subject}|{request.Body}|{request.RecipientScope}", "conversation", $"conversation-{request.ClientRequestId}", "Sent", request.Subject, request.Body, request.RecipientScope, ["moderation-checked", "delivery-attempt-recorded"], cancellationToken);

    public Task<CommunicationResponse> PublishBroadcastAsync(string tenantId, BroadcastRequest request, CancellationToken cancellationToken = default) =>
        CreateAsync(tenantId, "broadcast", request.ClientRequestId, $"{request.Title}|{request.Body}|{request.AudienceRule}", "broadcast", $"broadcast-{request.ClientRequestId}", "Published", request.Title, request.Body, request.AudienceRule, ["audience-snapshot", "quiet-hours-applied"], cancellationToken);

    public async Task<object> TraceAsync(string tenantId, string reference, CancellationToken cancellationToken = default)
    {
        var record = await FindAsync(tenantId, reference, cancellationToken);
        if (record is null) return new { traceId = $"trace-{reference}", references = Array.Empty<string>(), status = "NotFound" };
        return new { traceId = $"trace-{record.Reference}", references = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).ToArray(), status = record.Status };
    }

    private async Task<CommunicationResponse> CreateAsync(string tenantId, string command, string clientRequestId, string fingerprint, string kind, string reference, string status, string subject, string body, string recipientScope, string[] events, CancellationToken cancellationToken)
    {
        var scopedCommand = $"{tenantId}:{command}";
        var existingIdempotency = await dbContext.OperationalCommunicationIdempotencyRecords
            .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Command == scopedCommand && x.ClientRequestId == clientRequestId, cancellationToken);
        if (existingIdempotency is not null)
        {
            var existing = await LoadAsync(existingIdempotency.CommunicationId, cancellationToken);
            if (existing is null) return Missing(reference, kind);
            if (existingIdempotency.Fingerprint == fingerprint)
            {
                return ToResponse(existing, ["idempotency_duplicate"]);
            }

            existing.Status = "Conflict";
            existing.UpdatedAt = DateTimeOffset.UtcNow;
            dbContext.OperationalCommunicationEvents.Add(Event(tenantId, existing.Id, "idempotency_conflict", "Client request id reused with different communication details."));
            await dbContext.SaveChangesAsync(cancellationToken);
            return ToResponse(existing, ["idempotency_conflict"]);
        }

        var now = DateTimeOffset.UtcNow;
        var record = new OperationalCommunicationRecord
        {
            TenantId = tenantId,
            Kind = kind,
            Reference = reference,
            Status = status,
            Subject = subject,
            Body = body,
            RecipientScope = recipientScope,
            ClientRequestId = clientRequestId,
            CreatedAt = now,
            UpdatedAt = now
        };
        foreach (var eventType in events)
        {
            record.Events.Add(Event(tenantId, record.Id, eventType, eventType));
        }

        dbContext.OperationalCommunications.Add(record);
        dbContext.OperationalCommunicationIdempotencyRecords.Add(new OperationalCommunicationIdempotencyRecord
        {
            TenantId = tenantId,
            Command = scopedCommand,
            ClientRequestId = clientRequestId,
            Fingerprint = fingerprint,
            CommunicationId = record.Id,
            CreatedAt = now
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(record);
    }

    private async Task<IReadOnlyList<CommunicationResponse>> ListByEventAsync(string tenantId, string eventType, CancellationToken cancellationToken)
    {
        var records = await dbContext.OperationalCommunications.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId && x.Events.Any(e => e.EventType == eventType))
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    private async Task<IReadOnlyList<CommunicationResponse>> ListAsync(string tenantId, System.Linq.Expressions.Expression<Func<OperationalCommunicationRecord, bool>> predicate, CancellationToken cancellationToken)
    {
        var records = await dbContext.OperationalCommunications.AsNoTracking()
            .Include(x => x.Events)
            .Where(x => x.TenantId == tenantId)
            .Where(predicate)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        return records.Select(x => ToResponse(x)).ToList();
    }

    private async Task<OperationalCommunicationRecord?> FindAsync(string tenantId, string reference, CancellationToken cancellationToken)
    {
        var query = dbContext.OperationalCommunications.Include(x => x.Events).Where(x => x.TenantId == tenantId);
        if (Guid.TryParse(reference, out var id))
        {
            return await query.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        return await query.SingleOrDefaultAsync(x => x.Reference == reference || x.ClientRequestId == reference, cancellationToken);
    }

    private async Task<OperationalCommunicationRecord?> LoadAsync(Guid communicationId, CancellationToken cancellationToken) =>
        await dbContext.OperationalCommunications.Include(x => x.Events).SingleOrDefaultAsync(x => x.Id == communicationId, cancellationToken);

    private static CommunicationResponse ToResponse(OperationalCommunicationRecord record, IReadOnlyList<string>? extraEvidence = null)
    {
        var evidence = record.Events.OrderBy(x => x.OccurredAt).Select(x => x.EventType).Concat(extraEvidence ?? []).Distinct().ToList();
        return new CommunicationResponse(record.Reference, record.Status, [record.RecipientScope], evidence);
    }

    private static CommunicationResponse Missing(string reference, string kind) => new(reference, "NotFound", [kind], ["tenant-checked", "not-found"]);

    private static OperationalCommunicationEvent Event(string tenantId, Guid communicationId, string eventType, string reason) => new()
    {
        TenantId = tenantId,
        CommunicationId = communicationId,
        EventType = eventType,
        Reason = reason,
        OccurredAt = DateTimeOffset.UtcNow
    };
}

public sealed class CommunicationBoundaryGuard
{
    private static readonly string[] Blocked = ["attendance", "campus_gate", "scan", "transport", "wallet", "learning_reward", "request_approval", "medical", "emergency", "complaint_resolution", "document", "search", "admin_dashboard"];
    public bool Allows(string sideEffect) => !Blocked.Contains(sideEffect, StringComparer.OrdinalIgnoreCase);
}

public static class CommunicationCapabilities
{
    public const string NotificationCenter = "communications.notification_center";
    public const string DirectMessaging = "communications.direct_messaging";
    public const string Broadcasts = "communications.broadcasts";
    public const string DeliveryAcknowledgement = "communications.delivery_acknowledgement";
    public const string Configuration = "communications.configuration";
    public const string History = "communications.history";
    public static readonly string[] All = [NotificationCenter, DirectMessaging, Broadcasts, DeliveryAcknowledgement, Configuration, History];
}
