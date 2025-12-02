using medicine_box_api.Domain.Dtos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace medicine_box_api.Infraestructure.Mapping;
public class UserProfileMapping : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.ToTable("profiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(50);

        builder.Property(x => x.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(50);

        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasMaxLength(20);

        builder.Property(x => x.CaregiverId)
            .HasColumnName("caregiver_id");

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(15);

        builder.Property(x => x.Timezone)
            .HasColumnName("user_timezone")
            .HasMaxLength(30);
    }
}
