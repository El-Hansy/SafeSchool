using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SafeSchool.Api.Features.Transport.Notifications;

public static class TransportNotificationEntityTypeConfiguration
{
    public sealed class NotificationRecordConfiguration : IEntityTypeConfiguration<TransportNotificationRecord>
    {
        public void Configure(EntityTypeBuilder<TransportNotificationRecord> builder)
        {
            builder.ToTable("transport_notification_records");
            builder.HasIndex(x => new { x.TenantId, x.StudentProfileId, x.NotificationStatus });
            builder.HasIndex(x => new { x.TenantId, x.GuardianRecordId });
            builder.HasIndex(x => new { x.TenantId, x.SourceEventReference });
        }
    }
}
