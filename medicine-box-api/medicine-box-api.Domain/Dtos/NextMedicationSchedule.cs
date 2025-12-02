using System;
using System.Collections.Generic;

namespace medicine_box_api.Domain.Dtos;
public record NextMedicationSchedule
{
    public Guid UserId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public List<NextMedicationSchedDetails> NextMedicationDetails { get; set; } = new List<NextMedicationSchedDetails>();
}

public record NextMedicationSchedDetails
{
    public Guid AlarmId { get; set; }
    public Guid MedicationId { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public int Dosage { get; set; }
}
