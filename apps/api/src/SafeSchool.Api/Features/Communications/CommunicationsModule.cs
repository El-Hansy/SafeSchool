namespace SafeSchool.Api.Features.Communications;

public static class CommunicationsModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/communications";
    public const string GuardianRoutePrefix = "/api/v1/guardians/me/communications";
    public const string StudentRoutePrefix = "/api/v1/students/me/communications";

    public static IServiceCollection AddCommunicationsFeature(this IServiceCollection services)
    {
        services.AddScoped<CommunicationWorkflowService>();
        services.AddSingleton<CommunicationIdempotencyService>();
        services.AddScoped<CommunicationBoundaryGuard>();
        return services;
    }

    public static IEndpointRouteBuilder MapCommunicationsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var school = endpoints.MapGroup(SchoolRoutePrefix);
        var guardian = endpoints.MapGroup(GuardianRoutePrefix);
        var student = endpoints.MapGroup(StudentRoutePrefix);
        school.MapGet("/", (string schoolAccountId) => Results.Ok(CommunicationWorkflowService.DemoBoard(schoolAccountId)));
        school.MapPost("/source-events", (NotificationSourceEventRequest request, CommunicationWorkflowService service) => Results.Ok(service.AcceptSourceEvent(request)));
        school.MapGet("/notifications/{notificationId}", (string notificationId, CommunicationWorkflowService service) => Results.Ok(service.Detail(notificationId, "notification")));
        school.MapGet("/acknowledgements", (CommunicationWorkflowService service) => Results.Ok(service.Acknowledgements()));
        school.MapGet("/delivery", (CommunicationWorkflowService service) => Results.Ok(service.Delivery()));
        school.MapGet("/summaries", (CommunicationWorkflowService service) => Results.Ok(service.Summaries()));
        school.MapGet("/history", (CommunicationWorkflowService service) => Results.Ok(service.History()));
        school.MapGet("/moderation", (CommunicationWorkflowService service) => Results.Ok(service.Moderation()));
        school.MapGet("/exceptions", (CommunicationWorkflowService service) => Results.Ok(service.Exceptions()));
        school.MapGet("/configuration", (CommunicationWorkflowService service) => Results.Ok(service.Configuration()));
        school.MapGet("/configuration/audience-rules/{ruleId}", (string ruleId, CommunicationWorkflowService service) => Results.Ok(service.AudienceRule(ruleId)));
        school.MapGet("/configuration/templates/{templateId}", (string templateId, CommunicationWorkflowService service) => Results.Ok(service.Template(templateId)));
        school.MapGet("/conversations", (CommunicationWorkflowService service) => Results.Ok(service.Conversations()));
        school.MapPost("/conversations", (MessageRequest request, CommunicationWorkflowService service) => Results.Ok(service.SendMessage(request)));
        school.MapGet("/conversations/{conversationId}", (string conversationId, CommunicationWorkflowService service) => Results.Ok(service.Detail(conversationId, "conversation")));
        school.MapGet("/broadcasts/{broadcastId}", (string broadcastId, CommunicationWorkflowService service) => Results.Ok(service.Detail(broadcastId, "broadcast")));
        school.MapPost("/broadcasts", (BroadcastRequest request, CommunicationWorkflowService service) => Results.Ok(service.PublishBroadcast(request)));
        school.MapGet("/trace/{reference}", (string reference, CommunicationWorkflowService service) => Results.Ok(service.Trace(reference)));
        guardian.MapGet("/notifications", () => Results.Ok(CommunicationWorkflowService.NotificationCenter("guardian")));
        student.MapGet("/notifications", () => Results.Ok(CommunicationWorkflowService.NotificationCenter("student")));
        return endpoints;
    }
}

public sealed record NotificationSourceEventRequest(string SourceModule, string SourceReference, string Category, string ClientRequestId);
public sealed record MessageRequest(string Subject, string Body, string RecipientScope, string ClientRequestId);
public sealed record BroadcastRequest(string Title, string Body, string AudienceRule, string ClientRequestId);
public sealed record CommunicationResponse(string Reference, string Status, IReadOnlyList<string> Recipients, IReadOnlyList<string> Evidence);

public sealed class CommunicationWorkflowService(CommunicationIdempotencyService idempotency)
{
    public static object DemoBoard(string schoolAccountId) => new { schoolAccountId, phase = "communication-notifications", status = "demo-ready", unread = 42, broadcasts = 6, deliveryExceptions = 4, capabilities = CommunicationCapabilities.All };
    public static IReadOnlyList<CommunicationResponse> NotificationCenter(string scope) => [new($"{scope}-notification-1", "Unread", [scope], ["recipient-snapshot", "read-state-pending"] )];
    public IReadOnlyList<CommunicationResponse> Acknowledgements() => [new("ack-guardian-1", "Acknowledged", ["guardian-amina"], ["read-state-recorded", "audit-written"])];
    public IReadOnlyList<CommunicationResponse> Delivery() => [new("delivery-guardian-1", "Delivered", ["guardian-amina"], ["attempt-recorded", "provider-reference-minimized"])];
    public IReadOnlyList<CommunicationResponse> Summaries() => [new("summary-daily-1", "Ready", ["school-admin"], ["counts-minimized", "suppression-included"])];
    public IReadOnlyList<CommunicationResponse> History() => [new("history-message-1", "Sent", ["guardian-amina"], ["lifecycle-trace", "audit-written"])];
    public IReadOnlyList<CommunicationResponse> Conversations() => [new("conversation-1", "Sent", ["guardian-amina"], ["moderation-checked", "delivery-attempt-recorded"])];
    public IReadOnlyList<CommunicationResponse> Moderation() => [new("moderation-1", "Approved", ["guardian-amina"], ["policy-checked", "reviewer-audit"])];
    public IReadOnlyList<CommunicationResponse> Exceptions() => [new("communication-exception-1", "RetryScheduled", ["support"], ["delivery-failed", "owner-assigned"])];
    public object Configuration() => new { templates = 4, audienceRules = 3, quietHours = "Enabled", evidence = new[] { "tenant-scoped", "version-preserved" } };
    public object AudienceRule(string ruleId) => new { ruleId, scope = "grade:4", channel = "guardian", status = "Enabled", evidence = new[] { "audience-snapshot", "visibility-reviewed" } };
    public object Template(string templateId) => new { templateId, channel = "push", locale = "en/ar", status = "Approved", evidence = new[] { "template-versioned", "moderation-ready" } };
    public CommunicationResponse Detail(string reference, string kind) => new(reference, "Ready", [kind], ["tenant-checked", "recipient-snapshot", "audit-written"]);
    public CommunicationResponse AcceptSourceEvent(NotificationSourceEventRequest request) => new($"event-{request.ClientRequestId}", idempotency.Record("source", request.ClientRequestId, request.SourceReference).ToString(), ["eligible-recipient"], ["source-read-only", "notification-created"]);
    public CommunicationResponse SendMessage(MessageRequest request) => new($"conversation-{request.ClientRequestId}", "Sent", [request.RecipientScope], ["moderation-checked", "delivery-attempt-recorded"]);
    public CommunicationResponse PublishBroadcast(BroadcastRequest request) => new($"broadcast-{request.ClientRequestId}", "Published", [request.AudienceRule], ["audience-snapshot", "quiet-hours-applied"]);
    public object Trace(string reference) => new { traceId = $"trace-{reference}", references = new[] { "source-event", "template", "recipient-snapshot", "delivery-attempt", "read-state", "acknowledgement", "audit" } };
}

public enum CommunicationIdempotencyOutcome { Accepted, Duplicate, Conflict }

public sealed class CommunicationIdempotencyService
{
    private readonly Dictionary<string, string> _fingerprints = [];
    public CommunicationIdempotencyOutcome Record(string command, string clientRequestId, string fingerprint)
    {
        var key = $"{command}:{clientRequestId}";
        if (!_fingerprints.TryGetValue(key, out var existing)) { _fingerprints[key] = fingerprint; return CommunicationIdempotencyOutcome.Accepted; }
        return existing == fingerprint ? CommunicationIdempotencyOutcome.Duplicate : CommunicationIdempotencyOutcome.Conflict;
    }
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
