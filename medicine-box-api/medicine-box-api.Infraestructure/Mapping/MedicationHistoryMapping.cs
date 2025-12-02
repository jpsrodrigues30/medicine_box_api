using medicine_box_api.Domain.Dtos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace medicine_box_api.Infraestructure.Mapping;
public class MedicationHistoryMapping : IEntityTypeConfiguration<MedicationHistoryEntity>
{
    public void Configure(EntityTypeBuilder<MedicationHistoryEntity> builder)
    {
        builder.ToTable("medication_history");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasOne(x => x.Medication)
            .WithMany()
            .HasForeignKey(x => x.MedicationId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.MedicationId)
            .HasColumnName("medication_id")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(x => x.ScheduledAt)
            .HasColumnName("scheduled_at")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.Dosage)
            .HasColumnName("dosage");

        builder.Property(x => x.LastStatsUpdate)
            .HasColumnName("last_status_update");

        builder.Property(x => x.Timezone)
            .HasColumnName("timezone");
    }
}
