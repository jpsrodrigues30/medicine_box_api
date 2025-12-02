using medicine_box_api.Domain.Dtos.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace medicine_box_api.Infraestructure;
[ExcludeFromCodeCoverage]
public class dbContext : DbContext
{
    public dbContext(DbContextOptions<dbContext> options) : base(options)
    {
    }

    public virtual DbSet<UserProfileEntity> Profiles => Set<UserProfileEntity>();
    public virtual DbSet<MedicationEntity> Medications => Set<MedicationEntity>();
    public virtual DbSet<MedicationHistoryEntity> MedicationsHistory => Set<MedicationHistoryEntity>();
    public virtual DbSet<PatientCaregiversEntity> PatientCaregiver => Set<PatientCaregiversEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(dbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
