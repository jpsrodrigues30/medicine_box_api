using System.ComponentModel.DataAnnotations;

namespace medicine_box_api.Domain.Configuration;
public class MqttConfiguration
{
    [Required(AllowEmptyStrings = false)]
    public string? ClientId { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Host { get; set; }    
    [Required(AllowEmptyStrings = false)]
    public string? Port { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Username { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Password { get; set; }
    [Required(AllowEmptyStrings = false)]
    public bool CleanSession { get; set; } = false;
    [Required(AllowEmptyStrings = false)]
    public bool TLS { get; set; } = false;
    [Required(AllowEmptyStrings = false)]
    public int KeepAliveSeconds { get; set; } = 30;
    [Required(AllowEmptyStrings = false)]
    public TopicOptions Topics { get; set; } = new TopicOptions();
}

public class TopicOptions
{
    [Required(AllowEmptyStrings = false)]
    public string? AlarmStatus { get; set; }
}
