using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeSchool.Api.Infrastructure.Persistence.EfMigrations
{
    /// <inheritdoc />
    public partial class InitialSafeSchoolSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessDecisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    AttemptedAction = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<string>(type: "text", nullable: false),
                    TargetReference = table.Column<string>(type: "text", nullable: false),
                    Decision = table.Column<int>(type: "integer", nullable: false),
                    DecisionReason = table.Column<string>(type: "text", nullable: false),
                    RoleSources = table.Column<string[]>(type: "text[]", nullable: false),
                    FeatureCapabilityKey = table.Column<string>(type: "text", nullable: true),
                    DecidedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessDecisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_anomalies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnomalyType = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    EvidenceReference = table.Column<string>(type: "text", nullable: false),
                    AssignedTo = table.Column<string>(type: "text", nullable: false),
                    ResolutionReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_anomalies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_audit_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventCategory = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    SubjectType = table.Column<string>(type: "text", nullable: false),
                    SubjectReference = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    EventTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_audit_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_campus_decisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GateScanEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    Decision = table.Column<int>(type: "integer", nullable: false),
                    DecisionReason = table.Column<string>(type: "text", nullable: false),
                    FeatureCapabilityKey = table.Column<string>(type: "text", nullable: false),
                    PermissionKey = table.Column<string>(type: "text", nullable: false),
                    CredentialStatusUsed = table.Column<int>(type: "integer", nullable: false),
                    CampusStateAfter = table.Column<int>(type: "integer", nullable: false),
                    DecidedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_campus_decisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_entry_exit_notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GateScanEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttendanceRecordId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    GuardianReference = table.Column<string>(type: "text", nullable: false),
                    EligibilityStatus = table.Column<int>(type: "integer", nullable: false),
                    SuppressionReason = table.Column<string>(type: "text", nullable: false),
                    GuardianVisibleAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_entry_exit_notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_gates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GateCode = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    CampusReference = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AllowsEntry = table.Column<bool>(type: "boolean", nullable: false),
                    AllowsExit = table.Column<bool>(type: "boolean", nullable: false),
                    OfflineAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_gates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_idempotency_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    RequestHash = table.Column<string>(type: "text", nullable: false),
                    ResultReference = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_idempotency_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_manual_reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<string>(type: "text", nullable: false),
                    TargetReference = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReviewerReference = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_manual_reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_offline_sync_batches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientBatchId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ScanCount = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_offline_sync_batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttendanceSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SourceScanEventId = table.Column<Guid>(type: "uuid", nullable: true),
                    RuleVersion = table.Column<string>(type: "text", nullable: false),
                    CorrectionReason = table.Column<string>(type: "text", nullable: false),
                    ManualReviewId = table.Column<Guid>(type: "uuid", nullable: true),
                    AnomalyId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_rule_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryWindowStart = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EntryWindowEnd = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    LateAfter = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EarlyExitBefore = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    OfflineClockDriftToleranceMinutes = table.Column<int>(type: "integer", nullable: false),
                    GuardianNotificationsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_rule_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_scan_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GateId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScanPointId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    CredentialReference = table.Column<string>(type: "text", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    Method = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ClientScanId = table.Column<string>(type: "text", nullable: false),
                    ClientBatchId = table.Column<string>(type: "text", nullable: false),
                    LocalScanTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DecisionReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_scan_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_scan_points",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GateId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceReference = table.Column<string>(type: "text", nullable: false),
                    AssignedActorReference = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OfflineAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    AllowsEntry = table.Column<bool>(type: "boolean", nullable: false),
                    AllowsExit = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_scan_points", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendance_access_sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionName = table.Column<string>(type: "text", nullable: false),
                    AttendanceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CampusReference = table.Column<string>(type: "text", nullable: false),
                    ExpectedPopulationRule = table.Column<string>(type: "text", nullable: false),
                    EntryWindowStart = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EntryWindowEnd = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    LateAfter = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EarlyExitBefore = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    GenerationStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance_access_sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventCategory = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    SubjectType = table.Column<string>(type: "text", nullable: false),
                    SubjectReference = table.Column<string>(type: "text", nullable: false),
                    PreviousValueSummary = table.Column<string>(type: "text", nullable: true),
                    NewValueSummary = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    AccessDecisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    EventTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_actor_role_assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentStatus = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AssignedBy = table.Column<string>(type: "text", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_actor_role_assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_credential_status_snapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityCredentialId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialType = table.Column<int>(type: "integer", nullable: false),
                    CredentialStatus = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SnapshotGeneratedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SnapshotExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_credential_status_snapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_credentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialType = table.Column<int>(type: "integer", nullable: false),
                    CredentialReference = table.Column<string>(type: "text", nullable: false),
                    CredentialStatus = table.Column<int>(type: "integer", nullable: false),
                    IssuedBy = table.Column<string>(type: "text", nullable: false),
                    IssuedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReplacedByCredentialId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_credentials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_guardian_links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    GuardianId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipType = table.Column<string>(type: "text", nullable: false),
                    AccessScope = table.Column<string>(type: "jsonb", nullable: false),
                    LinkStatus = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RequestedBy = table.Column<string>(type: "text", nullable: false),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_guardian_links", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_guardians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    ContactMethods = table.Column<string[]>(type: "text[]", nullable: false),
                    IdentityReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    GuardianStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_guardians", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_nfc_card_credentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityCredentialId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardReference = table.Column<string>(type: "text", nullable: false),
                    CardLabel = table.Column<string>(type: "text", nullable: true),
                    ProvisioningStatus = table.Column<string>(type: "text", nullable: false),
                    LastVerifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_nfc_card_credentials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionKey = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PermissionScope = table.Column<string>(type: "text", nullable: false),
                    SensitiveAction = table.Column<bool>(type: "boolean", nullable: false),
                    PermissionStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_qr_fallback_credentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityCredentialId = table.Column<Guid>(type: "uuid", nullable: false),
                    QrReference = table.Column<string>(type: "text", nullable: false),
                    RotationSequence = table.Column<int>(type: "integer", nullable: false),
                    RotationReason = table.Column<string>(type: "text", nullable: false),
                    LastPresentedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_qr_fallback_credentials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_role_permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionKey = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_role_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleKey = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    RoleScope = table.Column<string>(type: "text", nullable: false),
                    RoleStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_access_student_profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolStudentNumber = table.Column<string>(type: "text", nullable: false),
                    ExternalIdentityReferences = table.Column<string[]>(type: "text[]", nullable: false),
                    LegalName = table.Column<string>(type: "text", nullable: false),
                    PreferredName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    GradeLevel = table.Column<string>(type: "text", nullable: false),
                    CampusOrDivision = table.Column<string>(type: "text", nullable: true),
                    EnrollmentStatus = table.Column<string>(type: "text", nullable: false),
                    ProfileStatus = table.Column<int>(type: "integer", nullable: false),
                    DuplicateReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    tenant_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_access_student_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_assignment_submissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    SubmissionStatus = table.Column<int>(type: "integer", nullable: false),
                    SubmittedEvidenceReference = table.Column<string>(type: "text", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    GradeValue = table.Column<string>(type: "text", nullable: false),
                    FeedbackSummary = table.Column<string>(type: "text", nullable: false),
                    ReviewerActorId = table.Column<string>(type: "text", nullable: false),
                    ClientRequestId = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_assignment_submissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false),
                    RequiredEvidence = table.Column<string>(type: "text", nullable: false),
                    AssignedToScope = table.Column<string>(type: "text", nullable: false),
                    DueAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OpensAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosesAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LatePolicy = table.Column<int>(type: "integer", nullable: false),
                    ReviewPolicy = table.Column<int>(type: "integer", nullable: false),
                    AssignmentStatus = table.Column<int>(type: "integer", nullable: false),
                    ConfigurationRevision = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_audit_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceReference = table.Column<string>(type: "text", nullable: false),
                    PayloadSummary = table.Column<string>(type: "text", nullable: false),
                    SensitivePayloadRedacted = table.Column<bool>(type: "boolean", nullable: false),
                    EventTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_audit_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_behavior_categorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryCode = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Classification = table.Column<int>(type: "integer", nullable: false),
                    DefaultSeverity = table.Column<int>(type: "integer", nullable: false),
                    DefaultVisibility = table.Column<string>(type: "text", nullable: false),
                    StarRuleSettingId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryStatus = table.Column<int>(type: "integer", nullable: false),
                    SensitiveByDefault = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_behavior_categorys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_behavior_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BehaviorCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    Classification = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    SourceContext = table.Column<string>(type: "text", nullable: false),
                    StaffNote = table.Column<string>(type: "text", nullable: false),
                    VisibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    ReviewState = table.Column<int>(type: "integer", nullable: false),
                    RelatedStarLedgerReference = table.Column<string>(type: "text", nullable: false),
                    ClientRequestId = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_behavior_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseCode = table.Column<string>(type: "text", nullable: false),
                    CourseName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CourseStatus = table.Column<int>(type: "integer", nullable: false),
                    VisibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedByActorId = table.Column<string>(type: "text", nullable: false),
                    UpdatedByActorId = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_idempotency_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdempotencyKind = table.Column<string>(type: "text", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    Fingerprint = table.Column<string>(type: "text", nullable: false),
                    Outcome = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_idempotency_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_learning_content_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ResourceReference = table.Column<string>(type: "text", nullable: false),
                    ContentStatus = table.Column<int>(type: "integer", nullable: false),
                    ReleaseAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletionPolicy = table.Column<int>(type: "integer", nullable: false),
                    StudentVisibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    GuardianVisibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    ContentRevision = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_learning_content_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_learning_exceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceReference = table.Column<string>(type: "text", nullable: false),
                    AffectedStudentProfileId = table.Column<string>(type: "text", nullable: false),
                    ExceptionType = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReviewerActorId = table.Column<string>(type: "text", nullable: false),
                    ResolutionReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_learning_exceptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_learning_group_memberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LearningGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    MembershipStatus = table.Column<int>(type: "integer", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_learning_group_memberships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_learning_groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    GroupName = table.Column<string>(type: "text", nullable: false),
                    GroupStatus = table.Column<int>(type: "integer", nullable: false),
                    VisibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_learning_groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_learning_progress_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceId = table.Column<string>(type: "text", nullable: false),
                    ProgressStatus = table.Column<int>(type: "integer", nullable: false),
                    ProgressPercent = table.Column<int>(type: "integer", nullable: true),
                    SourceEventReference = table.Column<string>(type: "text", nullable: false),
                    VisibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_learning_progress_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_learning_review_summarys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SummaryScope = table.Column<int>(type: "integer", nullable: false),
                    ScopeReference = table.Column<string>(type: "text", nullable: false),
                    SummaryStatus = table.Column<int>(type: "integer", nullable: false),
                    LatestEvidenceReference = table.Column<string>(type: "text", nullable: false),
                    StaffOnlyDetailsHidden = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_learning_review_summarys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_learning_rule_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RuleArea = table.Column<int>(type: "integer", nullable: false),
                    RulePayload = table.Column<string>(type: "text", nullable: false),
                    RuleVersion = table.Column<int>(type: "integer", nullable: false),
                    RuleStatus = table.Column<int>(type: "integer", nullable: false),
                    ChangeReason = table.Column<string>(type: "text", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_learning_rule_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_manual_learning_reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceReference = table.Column<string>(type: "text", nullable: false),
                    ReviewAction = table.Column<int>(type: "integer", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    ReviewerActorId = table.Column<string>(type: "text", nullable: false),
                    OriginalStatus = table.Column<string>(type: "text", nullable: false),
                    ResultingStatus = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_manual_learning_reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_quiz_attempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    AttemptStatus = table.Column<int>(type: "integer", nullable: false),
                    ClientRequestId = table.Column<string>(type: "text", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    QuestionSetRevision = table.Column<int>(type: "integer", nullable: false),
                    ScoreStatus = table.Column<int>(type: "integer", nullable: false),
                    ScoreValue = table.Column<decimal>(type: "numeric", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_quiz_attempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_quiz_questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionSetRevision = table.Column<int>(type: "integer", nullable: false),
                    QuestionType = table.Column<int>(type: "integer", nullable: false),
                    PromptReference = table.Column<string>(type: "text", nullable: false),
                    AnswerKeyReference = table.Column<string>(type: "text", nullable: false),
                    PointsPossible = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_quiz_questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_quiz_responses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizQuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseReference = table.Column<string>(type: "text", nullable: false),
                    AwardedPoints = table.Column<decimal>(type: "numeric", nullable: true),
                    RequiresManualReview = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_quiz_responses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_quizs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    QuizTitle = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    QuizStatus = table.Column<int>(type: "integer", nullable: false),
                    OpensAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosesAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AttemptLimit = table.Column<int>(type: "integer", nullable: false),
                    TimeLimitMinutes = table.Column<int>(type: "integer", nullable: true),
                    ScoringPolicy = table.Column<int>(type: "integer", nullable: false),
                    FeedbackVisibility = table.Column<int>(type: "integer", nullable: false),
                    QuestionSetRevision = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_quizs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_reward_catalog_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RewardCode = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    StarCost = table.Column<int>(type: "integer", nullable: false),
                    EligibilityScope = table.Column<string>(type: "text", nullable: false),
                    InventoryLimit = table.Column<int>(type: "integer", nullable: true),
                    RedemptionLimit = table.Column<int>(type: "integer", nullable: true),
                    RewardStatus = table.Column<int>(type: "integer", nullable: false),
                    AvailableFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AvailableUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_reward_catalog_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_reward_redemptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RewardCatalogItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    StarCostSnapshot = table.Column<int>(type: "integer", nullable: false),
                    RedemptionStatus = table.Column<int>(type: "integer", nullable: false),
                    FulfillmentStatus = table.Column<int>(type: "integer", nullable: false),
                    LedgerReference = table.Column<string>(type: "text", nullable: false),
                    ClientRequestId = table.Column<string>(type: "text", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_reward_redemptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_school_account_feature_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CapabilityKey = table.Column<string>(type: "text", nullable: false),
                    FeatureStatus = table.Column<int>(type: "integer", nullable: false),
                    DefaultGuardianVisibility = table.Column<string>(type: "text", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_school_account_feature_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_staff_learning_assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorId = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    LearningGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignmentRole = table.Column<int>(type: "integer", nullable: false),
                    AssignmentStatus = table.Column<int>(type: "integer", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_staff_learning_assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_star_balance_snapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    AvailableStars = table.Column<int>(type: "integer", nullable: false),
                    ReservedStars = table.Column<int>(type: "integer", nullable: false),
                    ConsumedStars = table.Column<int>(type: "integer", nullable: false),
                    PendingReviewStars = table.Column<int>(type: "integer", nullable: false),
                    CalculatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_star_balance_snapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_star_ledger_entrys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceReference = table.Column<string>(type: "text", nullable: false),
                    RuleVersion = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_star_ledger_entrys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_star_rule_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceRuleArea = table.Column<int>(type: "integer", nullable: false),
                    EligibleScope = table.Column<string>(type: "text", nullable: false),
                    StarAmount = table.Column<int>(type: "integer", nullable: false),
                    AwardCap = table.Column<int>(type: "integer", nullable: true),
                    RuleStatus = table.Column<int>(type: "integer", nullable: false),
                    RuleVersion = table.Column<int>(type: "integer", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_star_rule_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "learning_status_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusEventType = table.Column<int>(type: "integer", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceId = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    ReadyForNotification = table.Column<bool>(type: "boolean", nullable: false),
                    ReadyForStarEvidence = table.Column<bool>(type: "boolean", nullable: false),
                    ExportedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_status_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileApkReleases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionName = table.Column<string>(type: "text", nullable: false),
                    VersionCode = table.Column<int>(type: "integer", nullable: false),
                    Environment = table.Column<int>(type: "integer", nullable: false),
                    ReleaseStatus = table.Column<int>(type: "integer", nullable: false),
                    ArtifactUri = table.Column<string>(type: "text", nullable: false),
                    ArtifactChecksum = table.Column<string>(type: "text", nullable: false),
                    ReleaseNotesKey = table.Column<string>(type: "text", nullable: false),
                    MinimumSupportedVersionCode = table.Column<int>(type: "integer", nullable: false),
                    SupportContact = table.Column<string>(type: "text", nullable: false),
                    ApprovedByUserId = table.Column<string>(type: "text", nullable: false),
                    ApprovedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileApkReleases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileAuditEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    ActorUserId = table.Column<string>(type: "text", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<string>(type: "text", nullable: false),
                    TargetId = table.Column<string>(type: "text", nullable: false),
                    RoleCode = table.Column<string>(type: "text", nullable: false),
                    PermissionCode = table.Column<string>(type: "text", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Result = table.Column<string>(type: "text", nullable: false),
                    ReasonCode = table.Column<string>(type: "text", nullable: false),
                    CorrelationId = table.Column<string>(type: "text", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileAuditEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileDeviceSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    DeviceLabel = table.Column<string>(type: "text", nullable: false),
                    AppVersion = table.Column<string>(type: "text", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActiveRoleCode = table.Column<string>(type: "text", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    SessionStatus = table.Column<int>(type: "integer", nullable: false),
                    SignedInAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastSeenAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SignedOutAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileDeviceSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileInstallOrUpgradeEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionName = table.Column<string>(type: "text", nullable: false),
                    VersionCode = table.Column<int>(type: "integer", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    EventResult = table.Column<int>(type: "integer", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileInstallOrUpgradeEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileLanguagePreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    TextDirection = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileLanguagePreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileOfflineActionQueues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    SourceFeatureCode = table.Column<string>(type: "text", nullable: false),
                    SourceActionCode = table.Column<string>(type: "text", nullable: false),
                    ClientActionId = table.Column<string>(type: "text", nullable: false),
                    LocalOccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ServerReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SyncStatus = table.Column<int>(type: "integer", nullable: false),
                    ConflictReason = table.Column<string>(type: "text", nullable: true),
                    AuditEventId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileOfflineActionQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobilePermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    GrantType = table.Column<string>(type: "text", nullable: false),
                    SubjectId = table.Column<string>(type: "text", nullable: false),
                    WorkspaceCode = table.Column<string>(type: "text", nullable: false),
                    PermissionCode = table.Column<string>(type: "text", nullable: false),
                    GrantStatus = table.Column<int>(type: "integer", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EffectiveUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilePermissionGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileReleaseAudiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    AudienceType = table.Column<int>(type: "integer", nullable: false),
                    AudienceRef = table.Column<string>(type: "text", nullable: false),
                    AudienceStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileReleaseAudiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileRoleWorkspaceActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceCode = table.Column<string>(type: "text", nullable: false),
                    ActionCode = table.Column<string>(type: "text", nullable: false),
                    SourceFeatureCode = table.Column<string>(type: "text", nullable: false),
                    RequiredPermissionCode = table.Column<string>(type: "text", nullable: false),
                    OfflinePolicy = table.Column<int>(type: "integer", nullable: false),
                    RequiresOnlineConfirmation = table.Column<bool>(type: "boolean", nullable: false),
                    AuditLevel = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileRoleWorkspaceActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileRoleWorkspaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    WorkspaceCode = table.Column<string>(type: "text", nullable: false),
                    DisplayNameKey = table.Column<string>(type: "text", nullable: false),
                    RequiredRoleCode = table.Column<string>(type: "text", nullable: false),
                    RequiredPermissionCodes = table.Column<string>(type: "text", nullable: false),
                    EnabledFeatureCodes = table.Column<string>(type: "text", nullable: false),
                    WorkspaceStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileRoleWorkspaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileUserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ActiveTenantId = table.Column<string>(type: "text", nullable: false),
                    ActiveRoleCode = table.Column<string>(type: "text", nullable: false),
                    AvailableTenantIds = table.Column<string>(type: "text", nullable: false),
                    AvailableRoleCodes = table.Column<string>(type: "text", nullable: false),
                    LinkedStudentIds = table.Column<string>(type: "text", nullable: false),
                    MobileAccessStatus = table.Column<int>(type: "integer", nullable: false),
                    LastResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileUserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_certificate_idempotency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Command = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CertificateId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_certificate_idempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Reference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CertificateType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    SubjectReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SourceReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    VerificationState = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_certificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_communication_idempotency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Command = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CommunicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_communication_idempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_communications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Kind = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Reference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Subject = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Body = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    RecipientScope = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_communications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_complaint_idempotency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Command = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    ComplaintId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_complaint_idempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_complaints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    TrackingReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StudentProfileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    RequestedOutcome = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SubmitterRole = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Priority = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    VisibleSummary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_complaints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_document_idempotency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Command = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_document_idempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_document_search_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    QueryLogReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Scope = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    ResultCount = table.Column<int>(type: "integer", nullable: false),
                    SuppressedReasons = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_document_search_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Reference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    SubjectReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SourceModule = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Visibility = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "student_transport_assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    TransportRouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportVehicleId = table.Column<Guid>(type: "uuid", nullable: true),
                    PickupRouteStopSequenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    DropRouteStopSequenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ServiceDirection = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    VisibilityState = table.Column<int>(type: "integer", nullable: false),
                    AssignmentStatus = table.Column<int>(type: "integer", nullable: false),
                    ReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_transport_assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantMobileFeatureAvailabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    MobileEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ApkReleaseEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    EnabledFeatureCodes = table.Column<string>(type: "text", nullable: false),
                    EnabledWorkspaceCodes = table.Column<string>(type: "text", nullable: false),
                    ConfigurationStatus = table.Column<string>(type: "text", nullable: false),
                    LastReviewedByUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantMobileFeatureAvailabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_anomalies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportTripId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    TransportRouteId = table.Column<Guid>(type: "uuid", nullable: true),
                    RouteStopSequenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceEventReference = table.Column<string>(type: "text", nullable: false),
                    AnomalyType = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AssignedTo = table.Column<string>(type: "text", nullable: false),
                    DetectedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResolutionReason = table.Column<string>(type: "text", nullable: false),
                    ResolvedBy = table.Column<string>(type: "text", nullable: false),
                    ResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_anomalies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_audit_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventCategory = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    SubjectType = table.Column<string>(type: "text", nullable: false),
                    SubjectReference = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    EventTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_audit_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_boarding_drop_scan_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientScanId = table.Column<string>(type: "text", nullable: false),
                    TransportTripId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportRouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteStopSequenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    StudentTransportAssignmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CredentialReference = table.Column<string>(type: "text", nullable: false),
                    ScanDirection = table.Column<int>(type: "integer", nullable: false),
                    ScanMethod = table.Column<int>(type: "integer", nullable: false),
                    LocalScanTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OfflineCaptured = table.Column<bool>(type: "boolean", nullable: false),
                    SyncStatus = table.Column<int>(type: "integer", nullable: false),
                    ScanDecision = table.Column<int>(type: "integer", nullable: false),
                    DecisionReason = table.Column<string>(type: "text", nullable: false),
                    TransportStatusAfter = table.Column<int>(type: "integer", nullable: false),
                    ReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_boarding_drop_scan_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_eta_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportTripId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportRouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteStopSequenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    EstimatedArrivalTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EtaState = table.Column<int>(type: "integer", nullable: false),
                    ConfidenceState = table.Column<int>(type: "integer", nullable: false),
                    FreshnessStatus = table.Column<int>(type: "integer", nullable: false),
                    SourceLocationUpdateId = table.Column<Guid>(type: "uuid", nullable: true),
                    MaterialChange = table.Column<bool>(type: "boolean", nullable: false),
                    ReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    CalculatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_eta_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_idempotency_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdempotencyKind = table.Column<string>(type: "text", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    RequestHash = table.Column<string>(type: "text", nullable: false),
                    ResultReference = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_idempotency_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_location_updates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientLocationId = table.Column<string>(type: "text", nullable: false),
                    TransportTripId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingDeviceReference = table.Column<string>(type: "text", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    ReportedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LocationReference = table.Column<string>(type: "text", nullable: false),
                    ProgressState = table.Column<int>(type: "integer", nullable: false),
                    NearestRouteStopSequenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    FreshnessStatus = table.Column<int>(type: "integer", nullable: false),
                    AcceptanceStatus = table.Column<int>(type: "integer", nullable: false),
                    RetentionState = table.Column<int>(type: "integer", nullable: false),
                    SuppressionReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_location_updates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_manual_reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceRecordType = table.Column<string>(type: "text", nullable: false),
                    SourceRecordReference = table.Column<string>(type: "text", nullable: false),
                    ReviewAction = table.Column<int>(type: "integer", nullable: false),
                    CorrectedStatus = table.Column<string>(type: "text", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    ReviewedBy = table.Column<string>(type: "text", nullable: false),
                    ReviewedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_manual_reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_notification_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuardianRecordId = table.Column<string>(type: "text", nullable: false),
                    GuardianLinkId = table.Column<string>(type: "text", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    TransportTripId = table.Column<Guid>(type: "uuid", nullable: true),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    SourceEventReference = table.Column<string>(type: "text", nullable: false),
                    NotificationStatus = table.Column<int>(type: "integer", nullable: false),
                    SuppressionReason = table.Column<string>(type: "text", nullable: false),
                    VisibleStatus = table.Column<string>(type: "text", nullable: false),
                    ClientEventId = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_notification_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_offline_scan_sync_batches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientBatchId = table.Column<string>(type: "text", nullable: false),
                    TransportTripId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingDeviceReference = table.Column<string>(type: "text", nullable: false),
                    BatchStatus = table.Column<int>(type: "integer", nullable: false),
                    AcceptedCount = table.Column<int>(type: "integer", nullable: false),
                    NeedsReviewCount = table.Column<int>(type: "integer", nullable: false),
                    DuplicateCount = table.Column<int>(type: "integer", nullable: false),
                    RejectedCount = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_offline_scan_sync_batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_route_stop_sequences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportRouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportStopId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceDirection = table.Column<int>(type: "integer", nullable: false),
                    SequenceNumber = table.Column<int>(type: "integer", nullable: false),
                    PlannedArrivalOffset = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PlannedDepartureOffset = table.Column<TimeSpan>(type: "interval", nullable: true),
                    RouteVersion = table.Column<string>(type: "text", nullable: false),
                    SequenceStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_route_stop_sequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteName = table.Column<string>(type: "text", nullable: false),
                    RouteCode = table.Column<string>(type: "text", nullable: false),
                    ServiceDirection = table.Column<int>(type: "integer", nullable: false),
                    CampusReference = table.Column<string>(type: "text", nullable: false),
                    PlannedStartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    PlannedEndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    RouteStatus = table.Column<int>(type: "integer", nullable: false),
                    RouteVersion = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_rule_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentEligibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    PickupWindow = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DropWindow = table.Column<TimeSpan>(type: "interval", nullable: false),
                    RouteDeviationThresholdMeters = table.Column<int>(type: "integer", nullable: false),
                    LocationStalenessThreshold = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EtaChangeThreshold = table.Column<TimeSpan>(type: "interval", nullable: false),
                    NotificationEligibilityPolicy = table.Column<string>(type: "text", nullable: false),
                    ScanClockDriftTolerance = table.Column<TimeSpan>(type: "interval", nullable: false),
                    RetryHandlingPolicy = table.Column<string>(type: "text", nullable: false),
                    LocationDetailRetentionDays = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ChangeReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_rule_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_stops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StopName = table.Column<string>(type: "text", nullable: false),
                    StopCode = table.Column<string>(type: "text", nullable: false),
                    StopReference = table.Column<string>(type: "text", nullable: false),
                    PickupAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    DropAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    StopStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_stops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_trips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransportRouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteVersion = table.Column<string>(type: "text", nullable: false),
                    TransportVehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingDeviceReference = table.Column<string>(type: "text", nullable: false),
                    ServiceDirection = table.Column<int>(type: "integer", nullable: false),
                    TripDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PlannedStartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ActualStartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ActualEndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DriverReference = table.Column<string>(type: "text", nullable: false),
                    AttendantReference = table.Column<string>(type: "text", nullable: false),
                    SupervisorReference = table.Column<string>(type: "text", nullable: false),
                    TripStatus = table.Column<int>(type: "integer", nullable: false),
                    ReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_trips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transport_vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleName = table.Column<string>(type: "text", nullable: false),
                    VehicleCode = table.Column<string>(type: "text", nullable: false),
                    PlateReference = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    VehicleStatus = table.Column<int>(type: "integer", nullable: false),
                    DefaultSupervisorReference = table.Column<string>(type: "text", nullable: false),
                    DefaultDriverReference = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport_vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_anomalies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedSourceType = table.Column<string>(type: "text", nullable: false),
                    RelatedSourceReference = table.Column<string>(type: "text", nullable: false),
                    AnomalyType = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReviewerAssignment = table.Column<string>(type: "text", nullable: false),
                    ResolutionReason = table.Column<string>(type: "text", nullable: false),
                    ResolutionHistory = table.Column<string>(type: "text", nullable: false),
                    DetectedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_anomalies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_audit_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventCategory = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    SubjectType = table.Column<string>(type: "text", nullable: false),
                    SubjectReference = table.Column<string>(type: "text", nullable: false),
                    ActorReference = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    CorrelationReference = table.Column<string>(type: "text", nullable: false),
                    EventTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_audit_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_canteen_item_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCategoryCode = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_canteen_item_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_canteen_merchants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchantName = table.Column<string>(type: "text", nullable: false),
                    MerchantCode = table.Column<string>(type: "text", nullable: false),
                    MerchantStatus = table.Column<int>(type: "integer", nullable: false),
                    AllowedCategoryCodes = table.Column<string>(type: "text", nullable: false),
                    SettlementReferencePolicy = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_canteen_merchants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_canteen_purchase_transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    CanteenMerchantId = table.Column<Guid>(type: "uuid", nullable: false),
                    PosTerminalId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfflinePosSyncBatchId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClientPurchaseId = table.Column<string>(type: "text", nullable: false),
                    CredentialReference = table.Column<string>(type: "text", nullable: false),
                    ItemCategoryCode = table.Column<string>(type: "text", nullable: false),
                    ItemSummary = table.Column<string>(type: "text", nullable: false),
                    AmountMinor = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    PurchaseMode = table.Column<int>(type: "integer", nullable: false),
                    Decision = table.Column<int>(type: "integer", nullable: false),
                    DecisionReason = table.Column<string>(type: "text", nullable: false),
                    RuleSnapshotReference = table.Column<string>(type: "text", nullable: false),
                    ReserveSnapshotReference = table.Column<string>(type: "text", nullable: false),
                    LedgerDebitEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocalCapturedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_canteen_purchase_transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_idempotency_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdempotencyKind = table.Column<string>(type: "text", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    SourceReference = table.Column<string>(type: "text", nullable: false),
                    OutcomeReference = table.Column<string>(type: "text", nullable: false),
                    RequestHash = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_idempotency_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_ledger_entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryType = table.Column<int>(type: "integer", nullable: false),
                    EntryStatus = table.Column<int>(type: "integer", nullable: false),
                    AmountMinor = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    BalanceAvailableAfterMinor = table.Column<long>(type: "bigint", nullable: false),
                    BalancePendingAfterMinor = table.Column<long>(type: "bigint", nullable: false),
                    BalanceHeldAfterMinor = table.Column<long>(type: "bigint", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceReference = table.Column<string>(type: "text", nullable: false),
                    OriginalEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    RuleSnapshotReference = table.Column<string>(type: "text", nullable: false),
                    PostedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_ledger_entries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_manual_reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewScope = table.Column<string>(type: "text", nullable: false),
                    ScopeReference = table.Column<string>(type: "text", nullable: false),
                    ReviewAction = table.Column<int>(type: "integer", nullable: false),
                    ReviewStatus = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    ReviewerActor = table.Column<string>(type: "text", nullable: false),
                    ResultingLedgerEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClientRequestId = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_manual_reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_offline_pos_sync_batches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PosTerminalId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientBatchId = table.Column<string>(type: "text", nullable: false),
                    SyncStatus = table.Column<int>(type: "integer", nullable: false),
                    LocalCreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AcceptedCount = table.Column<int>(type: "integer", nullable: false),
                    HeldCount = table.Column<int>(type: "integer", nullable: false),
                    DuplicateCount = table.Column<int>(type: "integer", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_offline_pos_sync_batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_payment_confirmations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletTopUpId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProviderReference = table.Column<string>(type: "text", nullable: false),
                    ProviderEventId = table.Column<string>(type: "text", nullable: false),
                    ConfirmationStatus = table.Column<int>(type: "integer", nullable: false),
                    AmountMinor = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    SafePaymentMethodSummary = table.Column<string>(type: "text", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProviderEventTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RawPayloadReference = table.Column<string>(type: "text", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_payment_confirmations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_pos_terminals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CanteenMerchantId = table.Column<Guid>(type: "uuid", nullable: false),
                    TerminalCode = table.Column<string>(type: "text", nullable: false),
                    DeviceReference = table.Column<string>(type: "text", nullable: false),
                    OperatorReference = table.Column<string>(type: "text", nullable: false),
                    OfflineEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    PerTerminalReserveMinor = table.Column<long>(type: "bigint", nullable: false),
                    LastSyncAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_pos_terminals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_purchase_eligibility_rules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CanteenMerchantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCategoryCode = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DenialReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_purchase_eligibility_rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_reconciliation_mismatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReconciliationRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    MismatchType = table.Column<string>(type: "text", nullable: false),
                    SourceReference = table.Column<string>(type: "text", nullable: false),
                    DifferenceMinor = table.Column<long>(type: "bigint", nullable: false),
                    ReviewState = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_reconciliation_mismatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_reconciliation_runs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RunScope = table.Column<string>(type: "text", nullable: false),
                    DateFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    DateUntil = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LedgerTotalMinor = table.Column<long>(type: "bigint", nullable: false),
                    SourceTotalMinor = table.Column<long>(type: "bigint", nullable: false),
                    DifferenceMinor = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_reconciliation_runs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_refunds_reversals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrectionType = table.Column<int>(type: "integer", nullable: false),
                    OriginalSourceType = table.Column<string>(type: "text", nullable: false),
                    OriginalSourceReference = table.Column<string>(type: "text", nullable: false),
                    AmountMinor = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    ReviewerActor = table.Column<string>(type: "text", nullable: false),
                    ResultingLedgerEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClientRequestId = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_refunds_reversals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_review_summaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SummaryScope = table.Column<string>(type: "text", nullable: false),
                    ScopeReference = table.Column<string>(type: "text", nullable: false),
                    WalletCount = table.Column<int>(type: "integer", nullable: false),
                    TransactionCount = table.Column<int>(type: "integer", nullable: false),
                    SpendingLimitCount = table.Column<int>(type: "integer", nullable: false),
                    SettlementCount = table.Column<int>(type: "integer", nullable: false),
                    AnomalyCount = table.Column<int>(type: "integer", nullable: false),
                    ReviewCount = table.Column<int>(type: "integer", nullable: false),
                    LatestEvidenceTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TraceReferences = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_review_summaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_rule_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    MinTopUpMinor = table.Column<long>(type: "bigint", nullable: false),
                    MaxTopUpMinor = table.Column<long>(type: "bigint", nullable: false),
                    CashierThresholdMinor = table.Column<long>(type: "bigint", nullable: false),
                    OfflinePosEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    OfflinePerStudentReserveMinor = table.Column<long>(type: "bigint", nullable: false),
                    OfflinePerTerminalReserveMinor = table.Column<long>(type: "bigint", nullable: false),
                    ChargebackPolicy = table.Column<string>(type: "text", nullable: false),
                    DuplicateRetryPolicy = table.Column<string>(type: "text", nullable: false),
                    DetailedRetentionDays = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_rule_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_settlement_references",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SettlementScope = table.Column<string>(type: "text", nullable: false),
                    ExpectedTotalMinor = table.Column<long>(type: "bigint", nullable: false),
                    LedgerTotalMinor = table.Column<long>(type: "bigint", nullable: false),
                    DifferenceMinor = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ClosedBy = table.Column<string>(type: "text", nullable: false),
                    ClosedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    SourceEvidenceLinks = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_settlement_references", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_spending_limits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerType = table.Column<int>(type: "integer", nullable: false),
                    GuardianLinkId = table.Column<string>(type: "text", nullable: false),
                    LimitType = table.Column<int>(type: "integer", nullable: false),
                    AmountMinor = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    MerchantCode = table.Column<string>(type: "text", nullable: false),
                    ItemCategoryCode = table.Column<string>(type: "text", nullable: false),
                    WindowStart = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    WindowEnd = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    ActiveFrom = table.Column<DateOnly>(type: "date", nullable: true),
                    ActiveUntil = table.Column<DateOnly>(type: "date", nullable: true),
                    PrecedenceRank = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RuleVersion = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_spending_limits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_student_wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    WalletCode = table.Column<string>(type: "text", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    AvailableBalanceMinor = table.Column<long>(type: "bigint", nullable: false),
                    PendingBalanceMinor = table.Column<long>(type: "bigint", nullable: false),
                    HeldBalanceMinor = table.Column<long>(type: "bigint", nullable: false),
                    SettledBalanceMinor = table.Column<long>(type: "bigint", nullable: false),
                    PendingRecoveryMinor = table.Column<long>(type: "bigint", nullable: false),
                    WalletStatus = table.Column<int>(type: "integer", nullable: false),
                    RestrictionReason = table.Column<string>(type: "text", nullable: false),
                    CurrentRuleSettingId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_student_wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_top_ups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentProfileId = table.Column<string>(type: "text", nullable: false),
                    InitiatedByActorId = table.Column<string>(type: "text", nullable: false),
                    GuardianLinkId = table.Column<string>(type: "text", nullable: false),
                    TopUpSource = table.Column<int>(type: "integer", nullable: false),
                    AmountMinor = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    FeeMinor = table.Column<long>(type: "bigint", nullable: false),
                    NetCreditMinor = table.Column<long>(type: "bigint", nullable: false),
                    TopUpStatus = table.Column<int>(type: "integer", nullable: false),
                    PaymentProviderReference = table.Column<string>(type: "text", nullable: false),
                    CashierReference = table.Column<string>(type: "text", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "text", nullable: false),
                    InitiatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConfirmedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreditedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReviewReason = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_top_ups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_certificate_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    CertificateId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_certificate_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operational_certificate_events_operational_certificates_Cer~",
                        column: x => x.CertificateId,
                        principalTable: "operational_certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operational_communication_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    CommunicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_communication_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operational_communication_events_operational_communications~",
                        column: x => x.CommunicationId,
                        principalTable: "operational_communications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operational_complaint_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    ComplaintId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    ActorReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_complaint_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operational_complaint_events_operational_complaints_Complai~",
                        column: x => x.ComplaintId,
                        principalTable: "operational_complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operational_document_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_document_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operational_document_events_operational_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "operational_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_anomalies_TenantId_EvidenceReference_Anom~",
                table: "attendance_access_anomalies",
                columns: new[] { "TenantId", "EvidenceReference", "AnomalyType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_anomalies_TenantId_Status_Severity",
                table: "attendance_access_anomalies",
                columns: new[] { "TenantId", "Status", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_audit_events_TenantId_EventType_EventTime",
                table: "attendance_access_audit_events",
                columns: new[] { "TenantId", "EventType", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_campus_decisions_TenantId_Decision_Decide~",
                table: "attendance_access_campus_decisions",
                columns: new[] { "TenantId", "Decision", "DecidedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_campus_decisions_TenantId_GateScanEventId",
                table: "attendance_access_campus_decisions",
                columns: new[] { "TenantId", "GateScanEventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_entry_exit_notifications_TenantId_GateSca~",
                table: "attendance_access_entry_exit_notifications",
                columns: new[] { "TenantId", "GateScanEventId", "GuardianReference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_entry_exit_notifications_TenantId_Guardia~",
                table: "attendance_access_entry_exit_notifications",
                columns: new[] { "TenantId", "GuardianReference", "EligibilityStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_entry_exit_notifications_TenantId_Student~",
                table: "attendance_access_entry_exit_notifications",
                columns: new[] { "TenantId", "StudentProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_gates_TenantId_GateCode",
                table: "attendance_access_gates",
                columns: new[] { "TenantId", "GateCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_gates_TenantId_Status",
                table: "attendance_access_gates",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_idempotency_records_TenantId_IdempotencyK~",
                table: "attendance_access_idempotency_records",
                columns: new[] { "TenantId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_manual_reviews_TenantId_Status",
                table: "attendance_access_manual_reviews",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_manual_reviews_TenantId_TargetType_Target~",
                table: "attendance_access_manual_reviews",
                columns: new[] { "TenantId", "TargetType", "TargetReference" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_offline_sync_batches_TenantId_ClientBatch~",
                table: "attendance_access_offline_sync_batches",
                columns: new[] { "TenantId", "ClientBatchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_records_TenantId_AttendanceSessionId_Stud~",
                table: "attendance_access_records",
                columns: new[] { "TenantId", "AttendanceSessionId", "StudentProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_records_TenantId_Status",
                table: "attendance_access_records",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_rule_settings_TenantId",
                table: "attendance_access_rule_settings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_scan_events_TenantId_ClientScanId",
                table: "attendance_access_scan_events",
                columns: new[] { "TenantId", "ClientScanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_scan_events_TenantId_Status_Direction",
                table: "attendance_access_scan_events",
                columns: new[] { "TenantId", "Status", "Direction" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_scan_events_TenantId_StudentProfileId_Loc~",
                table: "attendance_access_scan_events",
                columns: new[] { "TenantId", "StudentProfileId", "LocalScanTime" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_scan_points_TenantId_DeviceReference",
                table: "attendance_access_scan_points",
                columns: new[] { "TenantId", "DeviceReference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_scan_points_TenantId_GateId_Status",
                table: "attendance_access_scan_points",
                columns: new[] { "TenantId", "GateId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_sessions_TenantId_AttendanceDate_CampusRe~",
                table: "attendance_access_sessions",
                columns: new[] { "TenantId", "AttendanceDate", "CampusReference" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_access_sessions_TenantId_GenerationStatus",
                table: "attendance_access_sessions",
                columns: new[] { "TenantId", "GenerationStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_actor_role_assignments_TenantId_ActorRefere~",
                table: "identity_access_actor_role_assignments",
                columns: new[] { "TenantId", "ActorReference", "AssignmentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_credential_status_snapshots_TenantId_Studen~",
                table: "identity_access_credential_status_snapshots",
                columns: new[] { "TenantId", "StudentProfileId", "SnapshotExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_credentials_TenantId_CredentialReference_Cr~",
                table: "identity_access_credentials",
                columns: new[] { "TenantId", "CredentialReference", "CredentialStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_credentials_TenantId_StudentProfileId_Crede~",
                table: "identity_access_credentials",
                columns: new[] { "TenantId", "StudentProfileId", "CredentialType" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_guardian_links_TenantId_StudentProfileId_Gu~",
                table: "identity_access_guardian_links",
                columns: new[] { "TenantId", "StudentProfileId", "GuardianId", "LinkStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_guardians_TenantId_DisplayName",
                table: "identity_access_guardians",
                columns: new[] { "TenantId", "DisplayName" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_nfc_card_credentials_TenantId_CardReference",
                table: "identity_access_nfc_card_credentials",
                columns: new[] { "TenantId", "CardReference" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_permissions_PermissionKey",
                table: "identity_access_permissions",
                column: "PermissionKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_qr_fallback_credentials_TenantId_QrReferenc~",
                table: "identity_access_qr_fallback_credentials",
                columns: new[] { "TenantId", "QrReference", "RotationSequence" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_role_permissions_TenantId_RoleId_Permission~",
                table: "identity_access_role_permissions",
                columns: new[] { "TenantId", "RoleId", "PermissionKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_access_roles_TenantId_RoleKey",
                table: "identity_access_roles",
                columns: new[] { "TenantId", "RoleKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_student_profiles_tenant_number_status",
                table: "identity_access_student_profiles",
                columns: new[] { "tenant_id", "SchoolStudentNumber", "ProfileStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_learning_assignment_submissions_TenantId",
                table: "learning_assignment_submissions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_assignment_submissions_TenantId_AssignmentId_Stude~",
                table: "learning_assignment_submissions",
                columns: new[] { "TenantId", "AssignmentId", "StudentProfileId", "ClientRequestId" });

            migrationBuilder.CreateIndex(
                name: "IX_learning_assignments_TenantId",
                table: "learning_assignments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_audit_events_TenantId_EventType_EventTime",
                table: "learning_audit_events",
                columns: new[] { "TenantId", "EventType", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_learning_behavior_categorys_TenantId",
                table: "learning_behavior_categorys",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_behavior_events_TenantId",
                table: "learning_behavior_events",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_courses_TenantId",
                table: "learning_courses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_courses_TenantId_CourseCode",
                table: "learning_courses",
                columns: new[] { "TenantId", "CourseCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_learning_idempotency_records_TenantId_IdempotencyKind_Idemp~",
                table: "learning_idempotency_records",
                columns: new[] { "TenantId", "IdempotencyKind", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_content_items_TenantId",
                table: "learning_learning_content_items",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_exceptions_TenantId",
                table: "learning_learning_exceptions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_group_memberships_TenantId",
                table: "learning_learning_group_memberships",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_group_memberships_TenantId_LearningGroupI~",
                table: "learning_learning_group_memberships",
                columns: new[] { "TenantId", "LearningGroupId", "StudentProfileId", "MembershipStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_groups_TenantId",
                table: "learning_learning_groups",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_progress_events_TenantId",
                table: "learning_learning_progress_events",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_review_summarys_TenantId",
                table: "learning_learning_review_summarys",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_learning_rule_settings_TenantId",
                table: "learning_learning_rule_settings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_manual_learning_reviews_TenantId",
                table: "learning_manual_learning_reviews",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_quiz_attempts_TenantId",
                table: "learning_quiz_attempts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_quiz_attempts_TenantId_QuizId_StudentProfileId_Cli~",
                table: "learning_quiz_attempts",
                columns: new[] { "TenantId", "QuizId", "StudentProfileId", "ClientRequestId" });

            migrationBuilder.CreateIndex(
                name: "IX_learning_quiz_questions_TenantId",
                table: "learning_quiz_questions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_quiz_responses_TenantId",
                table: "learning_quiz_responses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_quizs_TenantId",
                table: "learning_quizs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_reward_catalog_items_TenantId",
                table: "learning_reward_catalog_items",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_reward_redemptions_TenantId",
                table: "learning_reward_redemptions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_school_account_feature_settings_TenantId",
                table: "learning_school_account_feature_settings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_staff_learning_assignments_TenantId",
                table: "learning_staff_learning_assignments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_star_balance_snapshots_TenantId",
                table: "learning_star_balance_snapshots",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_star_ledger_entrys_TenantId",
                table: "learning_star_ledger_entrys",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_star_ledger_entrys_TenantId_SourceType_SourceRefer~",
                table: "learning_star_ledger_entrys",
                columns: new[] { "TenantId", "SourceType", "SourceReference", "IdempotencyKey" });

            migrationBuilder.CreateIndex(
                name: "IX_learning_star_rule_settings_TenantId",
                table: "learning_star_rule_settings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_learning_status_events_TenantId_StatusEventType_SourceType_~",
                table: "learning_status_events",
                columns: new[] { "TenantId", "StatusEventType", "SourceType", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_MobileApkReleases_VersionCode",
                table: "MobileApkReleases",
                column: "VersionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileAuditEvents_TenantId_ActorUserId_EventType_TargetType~",
                table: "MobileAuditEvents",
                columns: new[] { "TenantId", "ActorUserId", "EventType", "TargetType", "TargetId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MobileDeviceSessions_TenantId_UserId_DeviceId_SessionStatus",
                table: "MobileDeviceSessions",
                columns: new[] { "TenantId", "UserId", "DeviceId", "SessionStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_MobileInstallOrUpgradeEvents_TenantId_UserId_DeviceId_Versi~",
                table: "MobileInstallOrUpgradeEvents",
                columns: new[] { "TenantId", "UserId", "DeviceId", "VersionCode", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MobileLanguagePreferences_TenantId_UserId",
                table: "MobileLanguagePreferences",
                columns: new[] { "TenantId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileOfflineActionQueues_TenantId_DeviceId_SourceFeatureCo~",
                table: "MobileOfflineActionQueues",
                columns: new[] { "TenantId", "DeviceId", "SourceFeatureCode", "ClientActionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobilePermissionGrants_TenantId_SubjectId_WorkspaceCode_Per~",
                table: "MobilePermissionGrants",
                columns: new[] { "TenantId", "SubjectId", "WorkspaceCode", "PermissionCode", "GrantStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_MobileReleaseAudiences_ReleaseId_TenantId_AudienceType_Audi~",
                table: "MobileReleaseAudiences",
                columns: new[] { "ReleaseId", "TenantId", "AudienceType", "AudienceRef" });

            migrationBuilder.CreateIndex(
                name: "IX_MobileRoleWorkspaceActions_TenantId_WorkspaceCode_ActionCode",
                table: "MobileRoleWorkspaceActions",
                columns: new[] { "TenantId", "WorkspaceCode", "ActionCode" });

            migrationBuilder.CreateIndex(
                name: "IX_MobileRoleWorkspaces_TenantId_WorkspaceCode",
                table: "MobileRoleWorkspaces",
                columns: new[] { "TenantId", "WorkspaceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileUserProfiles_TenantId_UserId",
                table: "MobileUserProfiles",
                columns: new[] { "TenantId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_certificate_events_CertificateId",
                table: "operational_certificate_events",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_operational_certificate_events_TenantId_CertificateId_Occur~",
                table: "operational_certificate_events",
                columns: new[] { "TenantId", "CertificateId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_certificate_events_TenantId_EventType_OccurredAt",
                table: "operational_certificate_events",
                columns: new[] { "TenantId", "EventType", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_certificate_idempotency_TenantId_Command_Client~",
                table: "operational_certificate_idempotency",
                columns: new[] { "TenantId", "Command", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_certificates_TenantId_Reference",
                table: "operational_certificates",
                columns: new[] { "TenantId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_certificates_TenantId_SubjectReference_UpdatedAt",
                table: "operational_certificates",
                columns: new[] { "TenantId", "SubjectReference", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_certificates_TenantId_VerificationState_Updated~",
                table: "operational_certificates",
                columns: new[] { "TenantId", "VerificationState", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_communication_events_CommunicationId",
                table: "operational_communication_events",
                column: "CommunicationId");

            migrationBuilder.CreateIndex(
                name: "IX_operational_communication_events_TenantId_CommunicationId_O~",
                table: "operational_communication_events",
                columns: new[] { "TenantId", "CommunicationId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_communication_events_TenantId_EventType_Occurre~",
                table: "operational_communication_events",
                columns: new[] { "TenantId", "EventType", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_communication_idempotency_TenantId_Command_Clie~",
                table: "operational_communication_idempotency",
                columns: new[] { "TenantId", "Command", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_communications_TenantId_Kind_UpdatedAt",
                table: "operational_communications",
                columns: new[] { "TenantId", "Kind", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_communications_TenantId_Reference",
                table: "operational_communications",
                columns: new[] { "TenantId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_communications_TenantId_Status_UpdatedAt",
                table: "operational_communications",
                columns: new[] { "TenantId", "Status", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_complaint_events_ComplaintId",
                table: "operational_complaint_events",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_operational_complaint_events_TenantId_ComplaintId_OccurredAt",
                table: "operational_complaint_events",
                columns: new[] { "TenantId", "ComplaintId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_complaint_events_TenantId_EventType_OccurredAt",
                table: "operational_complaint_events",
                columns: new[] { "TenantId", "EventType", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_complaint_idempotency_TenantId_Command_ClientRe~",
                table: "operational_complaint_idempotency",
                columns: new[] { "TenantId", "Command", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_complaints_TenantId_Status_UpdatedAt",
                table: "operational_complaints",
                columns: new[] { "TenantId", "Status", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_complaints_TenantId_SubmitterRole_UpdatedAt",
                table: "operational_complaints",
                columns: new[] { "TenantId", "SubmitterRole", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_complaints_TenantId_TrackingReference",
                table: "operational_complaints",
                columns: new[] { "TenantId", "TrackingReference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_document_events_DocumentId",
                table: "operational_document_events",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_operational_document_events_TenantId_DocumentId_OccurredAt",
                table: "operational_document_events",
                columns: new[] { "TenantId", "DocumentId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_document_events_TenantId_EventType_OccurredAt",
                table: "operational_document_events",
                columns: new[] { "TenantId", "EventType", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_document_idempotency_TenantId_Command_ClientReq~",
                table: "operational_document_idempotency",
                columns: new[] { "TenantId", "Command", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_document_search_logs_TenantId_CreatedAt",
                table: "operational_document_search_logs",
                columns: new[] { "TenantId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_document_search_logs_TenantId_QueryLogReference",
                table: "operational_document_search_logs",
                columns: new[] { "TenantId", "QueryLogReference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_documents_TenantId_CategoryCode_UpdatedAt",
                table: "operational_documents",
                columns: new[] { "TenantId", "CategoryCode", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_documents_TenantId_Reference",
                table: "operational_documents",
                columns: new[] { "TenantId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_documents_TenantId_Visibility_UpdatedAt",
                table: "operational_documents",
                columns: new[] { "TenantId", "Visibility", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_student_transport_assignments_TenantId_StudentProfileId_Ass~",
                table: "student_transport_assignments",
                columns: new[] { "TenantId", "StudentProfileId", "AssignmentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_student_transport_assignments_TenantId_TransportRouteId",
                table: "student_transport_assignments",
                columns: new[] { "TenantId", "TransportRouteId" });

            migrationBuilder.CreateIndex(
                name: "IX_student_transport_assignments_TenantId_TransportVehicleId",
                table: "student_transport_assignments",
                columns: new[] { "TenantId", "TransportVehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantMobileFeatureAvailabilities_TenantId",
                table: "TenantMobileFeatureAvailabilities",
                column: "TenantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_anomalies_TenantId_StudentProfileId_Status",
                table: "transport_anomalies",
                columns: new[] { "TenantId", "StudentProfileId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_anomalies_TenantId_TransportTripId_Status",
                table: "transport_anomalies",
                columns: new[] { "TenantId", "TransportTripId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_audit_events_TenantId_EventType_EventTime",
                table: "transport_audit_events",
                columns: new[] { "TenantId", "EventType", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_boarding_drop_scan_events_TenantId_ClientScanId",
                table: "transport_boarding_drop_scan_events",
                columns: new[] { "TenantId", "ClientScanId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_boarding_drop_scan_events_TenantId_CredentialRefe~",
                table: "transport_boarding_drop_scan_events",
                columns: new[] { "TenantId", "CredentialReference" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_boarding_drop_scan_events_TenantId_TransportTripI~",
                table: "transport_boarding_drop_scan_events",
                columns: new[] { "TenantId", "TransportTripId", "ScanDirection" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_eta_records_TenantId_FreshnessStatus_CalculatedAt",
                table: "transport_eta_records",
                columns: new[] { "TenantId", "FreshnessStatus", "CalculatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_eta_records_TenantId_StudentProfileId_EtaState",
                table: "transport_eta_records",
                columns: new[] { "TenantId", "StudentProfileId", "EtaState" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_eta_records_TenantId_TransportTripId_RouteStopSeq~",
                table: "transport_eta_records",
                columns: new[] { "TenantId", "TransportTripId", "RouteStopSequenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_idempotency_records_TenantId_IdempotencyKind_Idem~",
                table: "transport_idempotency_records",
                columns: new[] { "TenantId", "IdempotencyKind", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_location_updates_TenantId_ClientLocationId",
                table: "transport_location_updates",
                columns: new[] { "TenantId", "ClientLocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_location_updates_TenantId_RetentionState_Reported~",
                table: "transport_location_updates",
                columns: new[] { "TenantId", "RetentionState", "ReportedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_location_updates_TenantId_TransportTripId_Reporte~",
                table: "transport_location_updates",
                columns: new[] { "TenantId", "TransportTripId", "ReportedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_manual_reviews_TenantId_SourceRecordType_SourceRe~",
                table: "transport_manual_reviews",
                columns: new[] { "TenantId", "SourceRecordType", "SourceRecordReference" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_notification_records_TenantId_GuardianRecordId",
                table: "transport_notification_records",
                columns: new[] { "TenantId", "GuardianRecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_notification_records_TenantId_SourceEventReference",
                table: "transport_notification_records",
                columns: new[] { "TenantId", "SourceEventReference" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_notification_records_TenantId_StudentProfileId_No~",
                table: "transport_notification_records",
                columns: new[] { "TenantId", "StudentProfileId", "NotificationStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_offline_scan_sync_batches_TenantId_ClientBatchId",
                table: "transport_offline_scan_sync_batches",
                columns: new[] { "TenantId", "ClientBatchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_route_stop_sequences_TenantId_TransportRouteId_Se~",
                table: "transport_route_stop_sequences",
                columns: new[] { "TenantId", "TransportRouteId", "ServiceDirection", "RouteVersion", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_route_stop_sequences_TenantId_TransportStopId",
                table: "transport_route_stop_sequences",
                columns: new[] { "TenantId", "TransportStopId" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_routes_TenantId_RouteCode",
                table: "transport_routes",
                columns: new[] { "TenantId", "RouteCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_routes_TenantId_RouteStatus",
                table: "transport_routes",
                columns: new[] { "TenantId", "RouteStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_rule_settings_TenantId_Status",
                table: "transport_rule_settings",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_stops_TenantId_StopCode",
                table: "transport_stops",
                columns: new[] { "TenantId", "StopCode" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_stops_TenantId_StopStatus",
                table: "transport_stops",
                columns: new[] { "TenantId", "StopStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_trips_TenantId_TrackingDeviceReference_TripStatus",
                table: "transport_trips",
                columns: new[] { "TenantId", "TrackingDeviceReference", "TripStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_trips_TenantId_TransportRouteId_TripStatus",
                table: "transport_trips",
                columns: new[] { "TenantId", "TransportRouteId", "TripStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_trips_TenantId_TransportVehicleId_TripStatus",
                table: "transport_trips",
                columns: new[] { "TenantId", "TransportVehicleId", "TripStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_transport_vehicles_TenantId_VehicleCode",
                table: "transport_vehicles",
                columns: new[] { "TenantId", "VehicleCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transport_vehicles_TenantId_VehicleStatus",
                table: "transport_vehicles",
                columns: new[] { "TenantId", "VehicleStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_anomalies_TenantId_Status_AnomalyType",
                table: "wallet_anomalies",
                columns: new[] { "TenantId", "Status", "AnomalyType" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_audit_events_TenantId_EventType_EventTime",
                table: "wallet_audit_events",
                columns: new[] { "TenantId", "EventType", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_canteen_item_categories_TenantId_ItemCategoryCode",
                table: "wallet_canteen_item_categories",
                columns: new[] { "TenantId", "ItemCategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_canteen_merchants_TenantId_MerchantCode",
                table: "wallet_canteen_merchants",
                columns: new[] { "TenantId", "MerchantCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_canteen_purchase_transactions_TenantId_ClientPurchas~",
                table: "wallet_canteen_purchase_transactions",
                columns: new[] { "TenantId", "ClientPurchaseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_idempotency_records_TenantId_IdempotencyKind_Idempot~",
                table: "wallet_idempotency_records",
                columns: new[] { "TenantId", "IdempotencyKind", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_ledger_entries_TenantId_IdempotencyKey",
                table: "wallet_ledger_entries",
                columns: new[] { "TenantId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_ledger_entries_TenantId_SourceType_SourceReference",
                table: "wallet_ledger_entries",
                columns: new[] { "TenantId", "SourceType", "SourceReference" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_ledger_entries_TenantId_StudentWalletId_PostedAt",
                table: "wallet_ledger_entries",
                columns: new[] { "TenantId", "StudentWalletId", "PostedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_manual_reviews_TenantId_ReviewScope_ScopeReference",
                table: "wallet_manual_reviews",
                columns: new[] { "TenantId", "ReviewScope", "ScopeReference" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_offline_pos_sync_batches_TenantId_ClientBatchId",
                table: "wallet_offline_pos_sync_batches",
                columns: new[] { "TenantId", "ClientBatchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_payment_confirmations_TenantId_ProviderEventId",
                table: "wallet_payment_confirmations",
                columns: new[] { "TenantId", "ProviderEventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_payment_confirmations_TenantId_ProviderReference",
                table: "wallet_payment_confirmations",
                columns: new[] { "TenantId", "ProviderReference" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_pos_terminals_TenantId_TerminalCode",
                table: "wallet_pos_terminals",
                columns: new[] { "TenantId", "TerminalCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_purchase_eligibility_rules_TenantId_CanteenMerchantI~",
                table: "wallet_purchase_eligibility_rules",
                columns: new[] { "TenantId", "CanteenMerchantId", "ItemCategoryCode" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_reconciliation_mismatches_TenantId_ReconciliationRun~",
                table: "wallet_reconciliation_mismatches",
                columns: new[] { "TenantId", "ReconciliationRunId" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_reconciliation_runs_TenantId_Status_DateFrom_DateUnt~",
                table: "wallet_reconciliation_runs",
                columns: new[] { "TenantId", "Status", "DateFrom", "DateUntil" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_refunds_reversals_TenantId_ClientRequestId",
                table: "wallet_refunds_reversals",
                columns: new[] { "TenantId", "ClientRequestId" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_review_summaries_TenantId_SummaryScope_ScopeReference",
                table: "wallet_review_summaries",
                columns: new[] { "TenantId", "SummaryScope", "ScopeReference" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_rule_settings_TenantId_Status",
                table: "wallet_rule_settings",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_settlement_references_TenantId_Status",
                table: "wallet_settlement_references",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_spending_limits_TenantId_StudentWalletId_Status",
                table: "wallet_spending_limits",
                columns: new[] { "TenantId", "StudentWalletId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_student_wallets_TenantId_StudentProfileId_WalletStat~",
                table: "wallet_student_wallets",
                columns: new[] { "TenantId", "StudentProfileId", "WalletStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_wallet_student_wallets_TenantId_WalletCode",
                table: "wallet_student_wallets",
                columns: new[] { "TenantId", "WalletCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_top_ups_TenantId_IdempotencyKey",
                table: "wallet_top_ups",
                columns: new[] { "TenantId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_top_ups_TenantId_StudentWalletId_TopUpStatus",
                table: "wallet_top_ups",
                columns: new[] { "TenantId", "StudentWalletId", "TopUpStatus" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessDecisions");

            migrationBuilder.DropTable(
                name: "attendance_access_anomalies");

            migrationBuilder.DropTable(
                name: "attendance_access_audit_events");

            migrationBuilder.DropTable(
                name: "attendance_access_campus_decisions");

            migrationBuilder.DropTable(
                name: "attendance_access_entry_exit_notifications");

            migrationBuilder.DropTable(
                name: "attendance_access_gates");

            migrationBuilder.DropTable(
                name: "attendance_access_idempotency_records");

            migrationBuilder.DropTable(
                name: "attendance_access_manual_reviews");

            migrationBuilder.DropTable(
                name: "attendance_access_offline_sync_batches");

            migrationBuilder.DropTable(
                name: "attendance_access_records");

            migrationBuilder.DropTable(
                name: "attendance_access_rule_settings");

            migrationBuilder.DropTable(
                name: "attendance_access_scan_events");

            migrationBuilder.DropTable(
                name: "attendance_access_scan_points");

            migrationBuilder.DropTable(
                name: "attendance_access_sessions");

            migrationBuilder.DropTable(
                name: "AuditEvents");

            migrationBuilder.DropTable(
                name: "identity_access_actor_role_assignments");

            migrationBuilder.DropTable(
                name: "identity_access_credential_status_snapshots");

            migrationBuilder.DropTable(
                name: "identity_access_credentials");

            migrationBuilder.DropTable(
                name: "identity_access_guardian_links");

            migrationBuilder.DropTable(
                name: "identity_access_guardians");

            migrationBuilder.DropTable(
                name: "identity_access_nfc_card_credentials");

            migrationBuilder.DropTable(
                name: "identity_access_permissions");

            migrationBuilder.DropTable(
                name: "identity_access_qr_fallback_credentials");

            migrationBuilder.DropTable(
                name: "identity_access_role_permissions");

            migrationBuilder.DropTable(
                name: "identity_access_roles");

            migrationBuilder.DropTable(
                name: "identity_access_student_profiles");

            migrationBuilder.DropTable(
                name: "learning_assignment_submissions");

            migrationBuilder.DropTable(
                name: "learning_assignments");

            migrationBuilder.DropTable(
                name: "learning_audit_events");

            migrationBuilder.DropTable(
                name: "learning_behavior_categorys");

            migrationBuilder.DropTable(
                name: "learning_behavior_events");

            migrationBuilder.DropTable(
                name: "learning_courses");

            migrationBuilder.DropTable(
                name: "learning_idempotency_records");

            migrationBuilder.DropTable(
                name: "learning_learning_content_items");

            migrationBuilder.DropTable(
                name: "learning_learning_exceptions");

            migrationBuilder.DropTable(
                name: "learning_learning_group_memberships");

            migrationBuilder.DropTable(
                name: "learning_learning_groups");

            migrationBuilder.DropTable(
                name: "learning_learning_progress_events");

            migrationBuilder.DropTable(
                name: "learning_learning_review_summarys");

            migrationBuilder.DropTable(
                name: "learning_learning_rule_settings");

            migrationBuilder.DropTable(
                name: "learning_manual_learning_reviews");

            migrationBuilder.DropTable(
                name: "learning_quiz_attempts");

            migrationBuilder.DropTable(
                name: "learning_quiz_questions");

            migrationBuilder.DropTable(
                name: "learning_quiz_responses");

            migrationBuilder.DropTable(
                name: "learning_quizs");

            migrationBuilder.DropTable(
                name: "learning_reward_catalog_items");

            migrationBuilder.DropTable(
                name: "learning_reward_redemptions");

            migrationBuilder.DropTable(
                name: "learning_school_account_feature_settings");

            migrationBuilder.DropTable(
                name: "learning_staff_learning_assignments");

            migrationBuilder.DropTable(
                name: "learning_star_balance_snapshots");

            migrationBuilder.DropTable(
                name: "learning_star_ledger_entrys");

            migrationBuilder.DropTable(
                name: "learning_star_rule_settings");

            migrationBuilder.DropTable(
                name: "learning_status_events");

            migrationBuilder.DropTable(
                name: "MobileApkReleases");

            migrationBuilder.DropTable(
                name: "MobileAuditEvents");

            migrationBuilder.DropTable(
                name: "MobileDeviceSessions");

            migrationBuilder.DropTable(
                name: "MobileInstallOrUpgradeEvents");

            migrationBuilder.DropTable(
                name: "MobileLanguagePreferences");

            migrationBuilder.DropTable(
                name: "MobileOfflineActionQueues");

            migrationBuilder.DropTable(
                name: "MobilePermissionGrants");

            migrationBuilder.DropTable(
                name: "MobileReleaseAudiences");

            migrationBuilder.DropTable(
                name: "MobileRoleWorkspaceActions");

            migrationBuilder.DropTable(
                name: "MobileRoleWorkspaces");

            migrationBuilder.DropTable(
                name: "MobileUserProfiles");

            migrationBuilder.DropTable(
                name: "operational_certificate_events");

            migrationBuilder.DropTable(
                name: "operational_certificate_idempotency");

            migrationBuilder.DropTable(
                name: "operational_communication_events");

            migrationBuilder.DropTable(
                name: "operational_communication_idempotency");

            migrationBuilder.DropTable(
                name: "operational_complaint_events");

            migrationBuilder.DropTable(
                name: "operational_complaint_idempotency");

            migrationBuilder.DropTable(
                name: "operational_document_events");

            migrationBuilder.DropTable(
                name: "operational_document_idempotency");

            migrationBuilder.DropTable(
                name: "operational_document_search_logs");

            migrationBuilder.DropTable(
                name: "student_transport_assignments");

            migrationBuilder.DropTable(
                name: "TenantMobileFeatureAvailabilities");

            migrationBuilder.DropTable(
                name: "transport_anomalies");

            migrationBuilder.DropTable(
                name: "transport_audit_events");

            migrationBuilder.DropTable(
                name: "transport_boarding_drop_scan_events");

            migrationBuilder.DropTable(
                name: "transport_eta_records");

            migrationBuilder.DropTable(
                name: "transport_idempotency_records");

            migrationBuilder.DropTable(
                name: "transport_location_updates");

            migrationBuilder.DropTable(
                name: "transport_manual_reviews");

            migrationBuilder.DropTable(
                name: "transport_notification_records");

            migrationBuilder.DropTable(
                name: "transport_offline_scan_sync_batches");

            migrationBuilder.DropTable(
                name: "transport_route_stop_sequences");

            migrationBuilder.DropTable(
                name: "transport_routes");

            migrationBuilder.DropTable(
                name: "transport_rule_settings");

            migrationBuilder.DropTable(
                name: "transport_stops");

            migrationBuilder.DropTable(
                name: "transport_trips");

            migrationBuilder.DropTable(
                name: "transport_vehicles");

            migrationBuilder.DropTable(
                name: "wallet_anomalies");

            migrationBuilder.DropTable(
                name: "wallet_audit_events");

            migrationBuilder.DropTable(
                name: "wallet_canteen_item_categories");

            migrationBuilder.DropTable(
                name: "wallet_canteen_merchants");

            migrationBuilder.DropTable(
                name: "wallet_canteen_purchase_transactions");

            migrationBuilder.DropTable(
                name: "wallet_idempotency_records");

            migrationBuilder.DropTable(
                name: "wallet_ledger_entries");

            migrationBuilder.DropTable(
                name: "wallet_manual_reviews");

            migrationBuilder.DropTable(
                name: "wallet_offline_pos_sync_batches");

            migrationBuilder.DropTable(
                name: "wallet_payment_confirmations");

            migrationBuilder.DropTable(
                name: "wallet_pos_terminals");

            migrationBuilder.DropTable(
                name: "wallet_purchase_eligibility_rules");

            migrationBuilder.DropTable(
                name: "wallet_reconciliation_mismatches");

            migrationBuilder.DropTable(
                name: "wallet_reconciliation_runs");

            migrationBuilder.DropTable(
                name: "wallet_refunds_reversals");

            migrationBuilder.DropTable(
                name: "wallet_review_summaries");

            migrationBuilder.DropTable(
                name: "wallet_rule_settings");

            migrationBuilder.DropTable(
                name: "wallet_settlement_references");

            migrationBuilder.DropTable(
                name: "wallet_spending_limits");

            migrationBuilder.DropTable(
                name: "wallet_student_wallets");

            migrationBuilder.DropTable(
                name: "wallet_top_ups");

            migrationBuilder.DropTable(
                name: "operational_certificates");

            migrationBuilder.DropTable(
                name: "operational_communications");

            migrationBuilder.DropTable(
                name: "operational_complaints");

            migrationBuilder.DropTable(
                name: "operational_documents");
        }
    }
}
