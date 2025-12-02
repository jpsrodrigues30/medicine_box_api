using medicine_box_api.Application;
using medicine_box_api.Domain.Helpers;
using medicine_box_api.Domain.Interface;
using medicine_box_api.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace medicine_box_api.Configuration;
[ExcludeFromCodeCoverage]
public static class DependencyInjectionConfig
{
    public static IServiceCollection DependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDependencyInjection(configuration);
        services.AddInfraestructureDependencyInjection(configuration);

        return services;
    }
}
