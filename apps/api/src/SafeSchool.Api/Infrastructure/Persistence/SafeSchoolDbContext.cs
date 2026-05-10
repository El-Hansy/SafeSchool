using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.AttendanceAccess;
using SafeSchool.Api.Features.AttendanceAccess.Anomalies;
using SafeSchool.Api.Features.AttendanceAccess.Attendance;
using SafeSchool.Api.Features.AttendanceAccess.Audit;
using SafeSchool.Api.Features.AttendanceAccess.Common;
using SafeSchool.Api.Features.AttendanceAccess.Common.Idempotency;
using SafeSchool.Api.Features.AttendanceAccess.Gates;
using SafeSchool.Api.Features.AttendanceAccess.Notifications;
using SafeSchool.Api.Features.AttendanceAccess.Reviews;
using SafeSchool.Api.Features.AttendanceAccess.Scans;
using SafeSchool.Api.Features.IdentityAccess;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Audit;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

namespace SafeSchool.Api.Infrastructure.Persistence;

public sealed class SafeSchoolDbContext(DbContextOptions<SafeSchoolDbContext> options) : DbContext(options)
{
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<GuardianRecord> GuardianRecords => Set<GuardianRecord>();
    public DbSet<GuardianLink> GuardianLinks => Set<GuardianLink>();
    public DbSet<IdentityCredential> IdentityCredentials => Set<IdentityCredential>();
    public DbSet<NfcCardCredential> NfcCardCredentials => Set<NfcCardCredential>();
    public DbSet<QrFallbackCredential> QrFallbackCredentials => Set<QrFallbackCredential>();
    public DbSet<CredentialStatusSnapshot> CredentialStatusSnapshots => Set<CredentialStatusSnapshot>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<ActorRoleAssignment> ActorRoleAssignments => Set<ActorRoleAssignment>();
    public DbSet<AccessDecision> AccessDecisions => Set<AccessDecision>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();
    public DbSet<Gate> Gates => Set<Gate>();
    public DbSet<ScanPoint> ScanPoints => Set<ScanPoint>();
    public DbSet<GateScanEvent> GateScanEvents => Set<GateScanEvent>();
    public DbSet<OfflineSyncBatch> OfflineSyncBatches => Set<OfflineSyncBatch>();
    public DbSet<CampusAccessDecision> CampusAccessDecisions => Set<CampusAccessDecision>();
    public DbSet<AttendanceAccessRuleSettings> AttendanceAccessRuleSettings => Set<AttendanceAccessRuleSettings>();
    public DbSet<AttendanceAccessAuditEvent> AttendanceAccessAuditEvents => Set<AttendanceAccessAuditEvent>();
    public DbSet<AttendanceAccessIdempotencyRecord> AttendanceAccessIdempotencyRecords => Set<AttendanceAccessIdempotencyRecord>();
    public DbSet<AttendanceSession> AttendanceSessions => Set<AttendanceSession>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<EntryExitNotificationRecord> EntryExitNotificationRecords => Set<EntryExitNotificationRecord>();
    public DbSet<AttendanceAnomaly> AttendanceAnomalies => Set<AttendanceAnomaly>();
    public DbSet<ManualReview> ManualReviews => Set<ManualReview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyIdentityAccessModel();
        modelBuilder.ApplyAttendanceAccessModel();
    }
}
