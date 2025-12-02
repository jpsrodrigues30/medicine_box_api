using System.Collections.Generic;
using System;

namespace medicine_box_api.Domain.Dtos;
public record MedicationsDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Dosage { get; set; }
    public List<string> Days { get; set; } = new List<string>();
    public List<TimeOnly> Schedules { get; set; } = new List<TimeOnly>();
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
