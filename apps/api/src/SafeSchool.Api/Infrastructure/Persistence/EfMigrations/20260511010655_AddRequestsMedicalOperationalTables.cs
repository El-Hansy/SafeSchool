using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeSchool.Api.Infrastructure.Persistence.EfMigrations
{
    /// <inheritdoc />
    public partial class AddRequestsMedicalOperationalTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operational_medical_idempotency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Command = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    MedicalRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_medical_idempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_medical_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    RecordReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StudentProfileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RecordType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Severity = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    VisibleSummary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RestrictedDetail = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_medical_records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_request_idempotency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    Command = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Fingerprint = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_request_idempotency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    TrackingReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StudentProfileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RequestType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Reason = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    RequestedOutcome = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ClientRequestId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SubmitterRole = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Priority = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    VisibleSummary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    StartsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EndsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_medical_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    MedicalRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    ActorReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_medical_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operational_medical_events_operational_medical_records_Medi~",
                        column: x => x.MedicalRecordId,
                        principalTable: "operational_medical_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operational_request_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    ActorReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_request_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operational_request_events_operational_requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "operational_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_events_MedicalRecordId",
                table: "operational_medical_events",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_events_TenantId_EventType_OccurredAt",
                table: "operational_medical_events",
                columns: new[] { "TenantId", "EventType", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_events_TenantId_MedicalRecordId_Occurre~",
                table: "operational_medical_events",
                columns: new[] { "TenantId", "MedicalRecordId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_idempotency_TenantId_Command_ClientRequ~",
                table: "operational_medical_idempotency",
                columns: new[] { "TenantId", "Command", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_records_TenantId_RecordReference",
                table: "operational_medical_records",
                columns: new[] { "TenantId", "RecordReference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_records_TenantId_RecordType_UpdatedAt",
                table: "operational_medical_records",
                columns: new[] { "TenantId", "RecordType", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_records_TenantId_Status_UpdatedAt",
                table: "operational_medical_records",
                columns: new[] { "TenantId", "Status", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_records_TenantId_StudentProfileId_Recor~",
                table: "operational_medical_records",
                columns: new[] { "TenantId", "StudentProfileId", "RecordType" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_events_RequestId",
                table: "operational_request_events",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_events_TenantId_EventType_OccurredAt",
                table: "operational_request_events",
                columns: new[] { "TenantId", "EventType", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_events_TenantId_RequestId_OccurredAt",
                table: "operational_request_events",
                columns: new[] { "TenantId", "RequestId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_idempotency_TenantId_Command_ClientRequ~",
                table: "operational_request_idempotency",
                columns: new[] { "TenantId", "Command", "ClientRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_requests_TenantId_RequestType_UpdatedAt",
                table: "operational_requests",
                columns: new[] { "TenantId", "RequestType", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_requests_TenantId_Status_UpdatedAt",
                table: "operational_requests",
                columns: new[] { "TenantId", "Status", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_requests_TenantId_SubmitterRole_UpdatedAt",
                table: "operational_requests",
                columns: new[] { "TenantId", "SubmitterRole", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_requests_TenantId_TrackingReference",
                table: "operational_requests",
                columns: new[] { "TenantId", "TrackingReference" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operational_medical_events");

            migrationBuilder.DropTable(
                name: "operational_medical_idempotency");

            migrationBuilder.DropTable(
                name: "operational_request_events");

            migrationBuilder.DropTable(
                name: "operational_request_idempotency");

            migrationBuilder.DropTable(
                name: "operational_medical_records");

            migrationBuilder.DropTable(
                name: "operational_requests");
        }
    }
}
