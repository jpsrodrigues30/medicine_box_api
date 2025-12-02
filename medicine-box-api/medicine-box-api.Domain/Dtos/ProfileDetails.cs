using System;

namespace medicine_box_api.Domain.Dtos;
public record ProfileDetails
{
    public Guid Id { get; set; }
    public Guid? CaregiverId { get; set; } = null;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Timezone { get; set; } = string.Empty;
}
