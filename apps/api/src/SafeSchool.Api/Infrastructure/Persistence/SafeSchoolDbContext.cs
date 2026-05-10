using Microsoft.EntityFrameworkCore;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyIdentityAccessModel();
    }
}
