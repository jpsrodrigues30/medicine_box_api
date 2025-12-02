using System;

namespace medicine_box_api.Domain.Dtos.Entities;
public class PatientCaregiversEntity
{
    public Guid PatientId { get; set; }
    public Guid CaregiverId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;

    public UserProfileEntity PatientProfile { get; set; } = new UserProfileEntity();
    public UserProfileEntity CaregiverProfile { get; set; } = new UserProfileEntity();
}
