namespace medicine_box_api.Domain.Dtos.Requests;
public class ProfileCreateRequest
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Timezone { get; set; } = string.Empty;
}
