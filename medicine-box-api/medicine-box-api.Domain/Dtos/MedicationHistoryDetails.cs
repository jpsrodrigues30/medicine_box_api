using System;

namespace medicine_box_api.Domain.Dtos;
public record MedicationHistoryDetails
{
    public Guid UserId { get; set; }
    public Guid MedicationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public int Dosage { get; set; }
    public string Timezone { get; set; } = string.Empty;
}
