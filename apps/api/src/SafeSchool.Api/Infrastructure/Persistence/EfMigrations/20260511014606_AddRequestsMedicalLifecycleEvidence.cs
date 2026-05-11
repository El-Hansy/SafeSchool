using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeSchool.Api.Infrastructure.Persistence.EfMigrations
{
    /// <inheritdoc />
    public partial class AddRequestsMedicalLifecycleEvidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operational_medical_review_summaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    MedicalRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecordReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StudentProfileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RecordType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Severity = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    ReviewState = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    LastEventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_medical_review_summaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_medical_status_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    MedicalRecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecordReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StudentProfileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RecordType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    Severity = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    SourceEventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    NotificationEligible = table.Column<bool>(type: "boolean", nullable: false),
                    ReviewRequired = table.Column<bool>(type: "boolean", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AvailableForNotificationsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_medical_status_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_request_review_summaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StudentProfileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RequestType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    CurrentAssignee = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExceptionState = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    StarOutcome = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    LastEventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_request_review_summaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operational_request_status_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StudentProfileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RequestType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    SourceEventType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
                    NotificationEligible = table.Column<bool>(type: "boolean", nullable: false),
                    ReviewRequired = table.Column<bool>(type: "boolean", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AvailableForNotificationsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_request_status_events", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_review_summaries_TenantId_MedicalRecord~",
                table: "operational_medical_review_summaries",
                columns: new[] { "TenantId", "MedicalRecordId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_review_summaries_TenantId_RecordType_Re~",
                table: "operational_medical_review_summaries",
                columns: new[] { "TenantId", "RecordType", "ReviewState" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_review_summaries_TenantId_Severity_Upda~",
                table: "operational_medical_review_summaries",
                columns: new[] { "TenantId", "Severity", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_review_summaries_TenantId_StudentProfil~",
                table: "operational_medical_review_summaries",
                columns: new[] { "TenantId", "StudentProfileId", "RecordType" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_status_events_TenantId_MedicalRecordId_~",
                table: "operational_medical_status_events",
                columns: new[] { "TenantId", "MedicalRecordId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_status_events_TenantId_NotificationElig~",
                table: "operational_medical_status_events",
                columns: new[] { "TenantId", "NotificationEligible", "AvailableForNotificationsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_medical_status_events_TenantId_ReviewRequired_O~",
                table: "operational_medical_status_events",
                columns: new[] { "TenantId", "ReviewRequired", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_review_summaries_TenantId_ExceptionStat~",
                table: "operational_request_review_summaries",
                columns: new[] { "TenantId", "ExceptionState", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_review_summaries_TenantId_RequestId",
                table: "operational_request_review_summaries",
                columns: new[] { "TenantId", "RequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_review_summaries_TenantId_RequestType_S~",
                table: "operational_request_review_summaries",
                columns: new[] { "TenantId", "RequestType", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_review_summaries_TenantId_StudentProfil~",
                table: "operational_request_review_summaries",
                columns: new[] { "TenantId", "StudentProfileId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_status_events_TenantId_NotificationElig~",
                table: "operational_request_status_events",
                columns: new[] { "TenantId", "NotificationEligible", "AvailableForNotificationsAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_status_events_TenantId_RequestId_Occurr~",
                table: "operational_request_status_events",
                columns: new[] { "TenantId", "RequestId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_request_status_events_TenantId_ReviewRequired_O~",
                table: "operational_request_status_events",
                columns: new[] { "TenantId", "ReviewRequired", "OccurredAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operational_medical_review_summaries");

            migrationBuilder.DropTable(
                name: "operational_medical_status_events");

            migrationBuilder.DropTable(
                name: "operational_request_review_summaries");

            migrationBuilder.DropTable(
                name: "operational_request_status_events");
        }
    }
}
