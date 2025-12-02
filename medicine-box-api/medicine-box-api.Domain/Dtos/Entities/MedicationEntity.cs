using System;
using System.Collections.Generic;

namespace medicine_box_api.Domain.Dtos.Entities;
public class MedicationEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Dosage { get; set; }
    public List<string> Days { get; set; } = new List<string>();
    public List<TimeOnly> Schedules { get; set; } = new List<TimeOnly>();
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public UserProfileEntity? User { get; set; }
}
