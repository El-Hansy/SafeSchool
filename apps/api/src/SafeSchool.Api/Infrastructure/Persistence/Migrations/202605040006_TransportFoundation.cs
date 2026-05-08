namespace SafeSchool.Api.Infrastructure.Persistence.Migrations;

public static class Migration202605040006TransportFoundation
{
    public const string MigrationId = "202605040006_TransportFoundation";
    public static readonly string[] Tables =
    [
        "transport_audit_events",
        "transport_idempotency_records",
        "transport_rule_settings"
    ];
}
