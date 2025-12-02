using medicine_box_api.Domain.Configuration;

namespace medicine_box_api.Api.Dtos;
public class ApplicationConfig
{
    public DatabaseConfiguration DatabaseConfig { get; set; }
    public MqttConfiguration MqttConfig { get; set; }

}
