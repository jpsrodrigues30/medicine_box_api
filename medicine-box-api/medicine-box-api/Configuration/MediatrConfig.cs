using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace medicine_box_api.Api.Configuration;
[ExcludeFromCodeCoverage]
public static class MediatrConfig
{
    public static void AddMediatrConfig(this IServiceCollection services)
    {
        const string application = "medicine-box-api.Application";

        var assembly = AppDomain.CurrentDomain.Load(application);

        AssemblyScanner
            .FindValidatorsInAssembly(assembly)
            .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
    }
}
