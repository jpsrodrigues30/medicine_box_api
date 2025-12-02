using medicine_box_api.Domain.Interface.Repositories;
using medicine_box_api.Infraestructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace medicine_box_api.Infraestructure;
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserProfilesRepository, UserProfilesRepository>();
        services.AddScoped<IMedicationsRepository, MedicationsRepository>();
        services.AddScoped<IMedicationsHistoryRepository, MedicationsHistoryRepository>();
        services.AddScoped<IPatientCaregiverRepository, PatientCaregiverRepository>();

        return services;
    }
}
