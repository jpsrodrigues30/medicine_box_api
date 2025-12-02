using medicine_box_api.Domain.Dtos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace medicine_box_api.Infraestructure.Mapping;
public class MedicationsMapping : IEntityTypeConfiguration<MedicationEntity>
{
    public void Configure(EntityTypeBuilder<MedicationEntity> builder)
    {
        builder.ToTable("medications");

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
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Dosage)
            .HasColumnName("dosage");

        builder.Property(x => x.Days)
            .HasColumnName("days");

        builder.Property(x => x.Schedules)
            .HasColumnName("schedules");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("date");

        builder.Property(x => x.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("date");

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");
    }
}
