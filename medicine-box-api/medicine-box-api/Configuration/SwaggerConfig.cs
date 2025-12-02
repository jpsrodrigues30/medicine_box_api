using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace medicine_box_api.Api.Configuration;
[ExcludeFromCodeCoverage]
public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Project Medicine Box API",
                Version = "v1.0",
                Description = "This API is part of Instituto Maua de Tecnologia's final project for the Computer Enginnering graduation. Its main goal is to controll user and medication info",
                Contact = new OpenApiContact { Name = "João Paulo", Email = "jpsrodrigues1898@gmail.com" }
            });
        });

        return services;
    }
    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Test.Apiiler.medicine_box_api.Api v1"));

        return app;
    }
}
