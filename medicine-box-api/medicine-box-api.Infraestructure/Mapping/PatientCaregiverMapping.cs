using medicine_box_api.Domain.Dtos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace medicine_box_api.Infraestructure.Mapping;
public class PatientCaregiverMapping : IEntityTypeConfiguration<PatientCaregiversEntity>
{
    public void Configure(EntityTypeBuilder<PatientCaregiversEntity> builder)
    {
        builder.ToTable("patient_caregivers");

        builder.HasKey(x => new { x.PatientId, x.CaregiverId });

        builder.Property(x => x.PatientId)
            .HasColumnName("patient_id")
            .IsRequired();

        builder.Property(x => x.CaregiverId)
            .HasColumnName("caregiver_id")
            .IsRequired();

        builder.HasOne(x => x.PatientProfile)
            .WithMany()            
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.NoAction);   

        builder.HasOne(x => x.CaregiverProfile)
            .WithMany()             
            .HasForeignKey(x => x.CaregiverId)
            .OnDelete(DeleteBehavior.NoAction);     

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(x => x.PatientId);
        builder.HasIndex(x => x.CaregiverId);
    }
}
