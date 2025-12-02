using System;

namespace medicine_box_api.Domain.Dtos;
public record MedicationScheduleDate
{
    public Guid MedicationId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; }
}
