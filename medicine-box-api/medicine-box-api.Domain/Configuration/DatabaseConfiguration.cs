using System.ComponentModel.DataAnnotations;

namespace medicine_box_api.Domain.Configuration;
public class DatabaseConfiguration
{
    [Required(AllowEmptyStrings = false)]
    public string? Host { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Database { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Username { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Password { get; set; }
}
