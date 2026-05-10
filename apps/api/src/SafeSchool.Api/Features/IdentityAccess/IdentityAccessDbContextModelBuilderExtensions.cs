using Microsoft.EntityFrameworkCore;
using SafeSchool.Api.Features.IdentityAccess.AccessControl;
using SafeSchool.Api.Features.IdentityAccess.Credentials;
using SafeSchool.Api.Features.IdentityAccess.Guardians;
using SafeSchool.Api.Features.IdentityAccess.StudentProfiles;

namespace SafeSchool.Api.Features.IdentityAccess;

public static class IdentityAccessDbContextModelBuilderExtensions
{
    public static ModelBuilder ApplyIdentityAccessModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StudentProfileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccessControlEntityTypeConfiguration.RoleConfiguration());
        modelBuilder.ApplyConfiguration(new AccessControlEntityTypeConfiguration.PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new AccessControlEntityTypeConfiguration.RolePermissionConfiguration());
        modelBuilder.ApplyConfiguration(new AccessControlEntityTypeConfiguration.ActorRoleAssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new GuardianEntityTypeConfiguration.GuardianRecordConfiguration());
        modelBuilder.ApplyConfiguration(new GuardianEntityTypeConfiguration.GuardianLinkConfiguration());
        modelBuilder.ApplyConfiguration(new CredentialEntityTypeConfiguration.IdentityCredentialConfiguration());
        modelBuilder.ApplyConfiguration(new CredentialEntityTypeConfiguration.NfcCardCredentialConfiguration());
        modelBuilder.ApplyConfiguration(new CredentialEntityTypeConfiguration.QrFallbackCredentialConfiguration());
        modelBuilder.ApplyConfiguration(new CredentialEntityTypeConfiguration.CredentialStatusSnapshotConfiguration());
        return modelBuilder;
    }
}
