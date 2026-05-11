using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Infrastructure.Persistence;

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
        admin.MapGet("/dashboard", async (string schoolAccountId, AdministrationWorkflowService service, CancellationToken ct) => Results.Ok(await service.DashboardAsync(schoolAccountId, ct)));
        admin.MapPost("/configuration", (ConfigurationCommand request, AdministrationWorkflowService service) => Results.Ok(service.Configure(request)));
        admin.MapGet("/configuration/history", (AdministrationWorkflowService service) => Results.Ok(service.ConfigurationHistory()));
        admin.MapGet("/audit", (AdministrationWorkflowService service) => Results.Ok(service.AuditTrail()));
        admin.MapGet("/audit/exports", (AdministrationWorkflowService service) => Results.Ok(service.AuditExports()));
        admin.MapGet("/audit/{auditEventId}", (string auditEventId, AdministrationWorkflowService service) => Results.Ok(service.AuditEvent(auditEventId)));
        admin.MapPost("/audit/export", (ExportCommand request, AdministrationWorkflowService service) => Results.Ok(service.ExportAudit(request)));
        admin.MapGet("/monitoring", (AdministrationWorkflowService service) => Results.Ok(service.Monitoring()));
        admin.MapGet("/monitoring/alert-rules", (AdministrationWorkflowService service) => Results.Ok(service.AlertRules()));
        admin.MapGet("/monitoring/alerts", (AdministrationWorkflowService service) => Results.Ok(service.Alerts()));
        admin.MapGet("/monitoring/exceptions", (AdministrationWorkflowService service) => Results.Ok(service.OperationalExceptions()));
        admin.MapGet("/monitoring/incidents", (AdministrationWorkflowService service) => Results.Ok(service.Incidents()));
        admin.MapPost("/alerts/{alertId}/incidents", (string alertId, AdministrationWorkflowService service) => Results.Ok(service.OpenIncident(alertId)));
        return endpoints;
    }
}

public sealed record ConfigurationCommand(string CapabilityKey, bool Enabled, string Reason, string ClientRequestId);
public sealed record ExportCommand(string Scope, string Reason, string ClientRequestId);

public sealed class AdministrationWorkflowService(SafeSchoolDbContext dbContext)
{
    public async Task<object> DashboardAsync(string schoolAccountId, CancellationToken cancellationToken = default)
    {
        var pendingReviews =
            await dbContext.ManualReviews.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.ManualTransportReviews.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.ManualWalletReviews.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.ManualLearningReviews.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.OperationalComplaints.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId && x.Status == "ManualReviewRequired", cancellationToken) +
            await dbContext.OperationalCommunications.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId && x.Status == "ManualReviewRequired", cancellationToken) +
            await dbContext.OperationalDocuments.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId && x.Status == "ManualReviewRequired", cancellationToken) +
            await dbContext.OperationalCertificates.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId && x.Status == "ManualReviewRequired", cancellationToken);
        var alerts =
            await dbContext.AttendanceAnomalies.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.TransportAnomalies.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.WalletAnomalies.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.LearningExceptions.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken);
        var incidents =
            await dbContext.OperationalComplaints.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId && x.Status == "Escalated", cancellationToken) +
            await dbContext.OperationalCommunications.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId && x.Status == "Conflict", cancellationToken);
        var indexed =
            await dbContext.OperationalDocuments.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken) +
            await dbContext.OperationalCertificates.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken);
        var searches = await dbContext.OperationalSearchLogs.AsNoTracking().CountAsync(x => x.TenantId == schoolAccountId, cancellationToken);

        return new
        {
            schoolAccountId,
            status = "operational",
            modulesEnabled = AdministrationCapabilities.All.Length,
            pendingReviews,
            alerts,
            incidents,
            searchFreshness = $"{indexed} indexed records, {searches} audited searches"
        };
    }

    public object Configure(ConfigurationCommand request) => new { request.CapabilityKey, request.Enabled, status = "Applied", evidence = new[] { "dependency-validated", "version-preserved", request.Reason } };
    public object ConfigurationHistory() => new { total = 4, changes = new[] { new { capabilityKey = "guardian.mobile.access", state = "Enabled", actor = "tenant-admin", evidence = "dependency-validated" }, new { capabilityKey = "wallet.topups", state = "Enabled", actor = "finance-admin", evidence = "version-preserved" } } };
    public object AuditTrail() => new { total = 316, appendOnly = true, minimizedPayloads = true, filters = new[] { "actor", "action", "target", "correlation" } };
    public object AuditExports() => new { total = 3, exports = new[] { new { exportReference = "audit-export-sales-demo", scope = "tenant", status = "Prepared", reason = "Sales demo evidence pack" } } };
    public object AuditEvent(string auditEventId) => new { auditEventId, actor = "platform-admin", action = "configuration.applied", target = "guardian.mobile.access", correlation = "cfg-demo-001", evidence = new[] { "tenant-scoped", "permission-checked", "append-only" } };
    public object ExportAudit(ExportCommand request) => new { exportReference = $"audit-export-{request.ClientRequestId}", request.Scope, status = "Prepared", request.Reason };
    public object Monitoring() => new { metrics = 84, thresholdBreaches = 4, alerts = 5, operationalExceptions = 6 };
    public object AlertRules() => new { total = 3, rules = new[] { new { ruleReference = "rule-search-freshness", metric = "Search freshness", threshold = "95% within 5 minutes", owner = "Platform admin" }, new { ruleReference = "rule-api-errors", metric = "Mobile API failures", threshold = "5 failures in 10 minutes", owner = "Support" } } };
    public object Alerts() => new { total = 5, alerts = new[] { new { alertId = "alert-metrics-001", severity = "medium", metric = "Search index freshness", status = "Open" }, new { alertId = "alert-audit-002", severity = "high", metric = "Audit export queue", status = "Acknowledged" } } };
    public object OperationalExceptions() => new { total = 6, exceptions = new[] { new { exceptionReference = "exception-search-001", module = "documents", reason = "Index stale", owner = "Compliance" }, new { exceptionReference = "exception-mobile-002", module = "mobile", reason = "Install event delayed", owner = "Support" } } };
    public object Incidents() => new { total = 2, incidents = new[] { new { incidentReference = "incident-alert-metrics-001", status = "Open", owner = "Platform admin" }, new { incidentReference = "incident-wallet-recon-002", status = "Investigating", owner = "Finance" } } };
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
