using medicine_box_api.Domain.Configuration;
using medicine_box_api.Domain.Interface;

namespace medicine_box_api.Domain.Helpers;
public class MqttTopicsProvider(MqttConfiguration cfg) : IMqttTopics
{
    public string AlarmStatus() => cfg.Topics.AlarmStatus ?? string.Empty;
}
