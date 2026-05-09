using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.IdentityAccess.AccessControl;

public static class AccessControlEntityTypeConfiguration
{
    public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("identity_access_roles");
            builder.HasIndex(x => new { x.TenantId, x.RoleKey }).IsUnique();
        }
    }

    public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("identity_access_permissions");
            builder.HasIndex(x => x.PermissionKey).IsUnique();
        }
    }

    public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("identity_access_role_permissions");
            builder.HasIndex(x => new { x.TenantId, x.RoleId, x.PermissionKey }).IsUnique();
        }
    }

    public sealed class ActorRoleAssignmentConfiguration : IEntityTypeConfiguration<ActorRoleAssignment>
    {
        public void Configure(EntityTypeBuilder<ActorRoleAssignment> builder)
        {
            builder.ToTable("identity_access_actor_role_assignments");
            builder.HasIndex(x => new { x.TenantId, x.ActorReference, x.AssignmentStatus });
        }
    }
}
