using medicine_box_api.Application.Events;
using medicine_box_api.Application.Services;
using medicine_box_api.Domain.Configuration;
using medicine_box_api.Domain.Helpers;
using medicine_box_api.Domain.Interface;
using medicine_box_api.Domain.Interface.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace medicine_box_api.Application;
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<MqttListener>();
        services.AddSingleton<IMqttTopics, MqttTopicsProvider>();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MqttConfiguration>>().Value);

        services.AddScoped<IUserProfileService, UserProfilesService>();
        services.AddScoped<IMedicationsService, MedicationsService>();
        services.AddScoped<IMedicationsHistoryService, MedicationsHistoryService>();
        services.AddScoped<IPatientCaregiverService, PatientCaregiverService>();

        return services;
    }
}
