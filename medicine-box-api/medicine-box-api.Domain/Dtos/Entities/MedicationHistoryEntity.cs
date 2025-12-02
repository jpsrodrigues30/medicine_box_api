using System;

namespace medicine_box_api.Domain.Dtos.Entities;
public class MedicationHistoryEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MedicationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Dosage { get; set; }
    public DateTime? LastStatsUpdate { get; set; }
    public string? Timezone { get; set; } = string.Empty;

    public UserProfileEntity? User { get; set; }
    public MedicationEntity? Medication { get; set; }
}
