namespace SafeSchool.Api.Features.Mobile;

public static class MobileSupportDiagnosticsEndpoints
{
    public static RouteGroupBuilder MapMobileSupportDiagnosticsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/support/diagnostics/{correlationId}", (string correlationId) =>
            Results.Ok(new SupportDiagnosticTimeline(correlationId, "school-demo", "guardian-demo", "device-guardian", "12.0.0", ["install allowed", "sign-in allowed", "role selected", "notification read"], "No active blocker", "Continue monitoring")));
        group.MapGet("/support/metrics", (MobileSupportMetrics metrics) => Results.Ok(metrics.Snapshot()));
        return group;
    }
}
