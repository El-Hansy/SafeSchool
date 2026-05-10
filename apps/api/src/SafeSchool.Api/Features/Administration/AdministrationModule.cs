namespace SafeSchool.Api.Features.Administration;

public static class AdministrationModule
{
    public const string SchoolRoutePrefix = "/api/v1/schools/{schoolAccountId}/admin";

    public static IServiceCollection AddAdministrationFeature(this IServiceCollection services)
    {
        services.AddScoped<AdministrationWorkflowService>();
        services.AddScoped<AdministrationBoundaryGuard>();
        return services;
    }

    public static IEndpointRouteBuilder MapAdministrationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var admin = endpoints.MapGroup(SchoolRoutePrefix);
        admin.MapGet("/dashboard", (string schoolAccountId, AdministrationWorkflowService service) => Results.Ok(service.Dashboard(schoolAccountId)));
        admin.MapPost("/configuration", (ConfigurationCommand request, AdministrationWorkflowService service) => Results.Ok(service.Configure(request)));
        admin.MapGet("/audit", (AdministrationWorkflowService service) => Results.Ok(service.AuditTrail()));
        admin.MapPost("/audit/export", (ExportCommand request, AdministrationWorkflowService service) => Results.Ok(service.ExportAudit(request)));
        admin.MapGet("/monitoring", (AdministrationWorkflowService service) => Results.Ok(service.Monitoring()));
        admin.MapPost("/alerts/{alertId}/incidents", (string alertId, AdministrationWorkflowService service) => Results.Ok(service.OpenIncident(alertId)));
        return endpoints;
    }
}

public sealed record ConfigurationCommand(string CapabilityKey, bool Enabled, string Reason, string ClientRequestId);
public sealed record ExportCommand(string Scope, string Reason, string ClientRequestId);

public sealed class AdministrationWorkflowService
{
    public object Dashboard(string schoolAccountId) => new { schoolAccountId, status = "demo-ready", modulesEnabled = 11, pendingReviews = 24, alerts = 5, incidents = 2, searchFreshness = "95% within target" };
    public object Configure(ConfigurationCommand request) => new { request.CapabilityKey, request.Enabled, status = "Applied", evidence = new[] { "dependency-validated", "version-preserved", request.Reason } };
    public object AuditTrail() => new { total = 316, appendOnly = true, minimizedPayloads = true, filters = new[] { "actor", "action", "target", "correlation" } };
    public object ExportAudit(ExportCommand request) => new { exportReference = $"audit-export-{request.ClientRequestId}", request.Scope, status = "Prepared", request.Reason };
    public object Monitoring() => new { metrics = 84, thresholdBreaches = 4, alerts = 5, operationalExceptions = 6 };
    public object OpenIncident(string alertId) => new { incidentReference = $"incident-{alertId}", status = "Open", evidence = new[] { "metric-threshold", "owner-assigned", "audit-written" } };
}

public sealed class AdministrationBoundaryGuard
{
    private static readonly string[] Blocked = ["attendance", "campus_gate", "scan", "transport", "wallet", "learning_reward", "request_approval", "medical", "emergency", "complaint_resolution", "communication_delivery"];
    public bool Allows(string sideEffect) => !Blocked.Contains(sideEffect, StringComparer.OrdinalIgnoreCase);
}

public static class AdministrationCapabilities
{
    public const string Dashboard = "admin.dashboard";
    public const string Configuration = "admin.configuration";
    public const string AuditTrail = "admin.audit_trail";
    public const string MetricsMonitoring = "admin.metrics_monitoring";
    public const string Alerts = "admin.alerts";
    public const string Incidents = "admin.incidents";
    public static readonly string[] All = [Dashboard, Configuration, AuditTrail, MetricsMonitoring, Alerts, Incidents];
}
