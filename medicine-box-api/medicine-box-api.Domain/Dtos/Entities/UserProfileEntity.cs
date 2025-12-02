using System;

namespace medicine_box_api.Domain.Dtos.Entities;
public class UserProfileEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public string? Role { get; set; } = string.Empty;
    public Guid? CaregiverId { get; set; } = Guid.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public string? Timezone { get; set; } = string.Empty;
}
