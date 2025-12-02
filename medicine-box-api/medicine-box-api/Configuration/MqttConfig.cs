using medicine_box_api.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace medicine_box_api.Api.Configuration;
public static class MqttConfig
{
    public static IServiceCollection AddMqttConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<MqttConfiguration>()
            .Bind(configuration.GetSection("MQTT"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
