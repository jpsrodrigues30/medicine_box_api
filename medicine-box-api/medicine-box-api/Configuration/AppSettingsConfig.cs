using medicine_box_api.Api.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace medicine_box_api.Api.Configuration;
public static class AppSettingsConfig
{
    public static void AddAppSettingsConfiguration(this IServiceCollection services)
    {
        services
            .AddOptions<ApplicationConfig>()
            .BindConfiguration(string.Empty)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
