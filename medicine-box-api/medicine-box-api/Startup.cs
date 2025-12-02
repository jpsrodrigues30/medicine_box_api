using medicine_box_api.Api.Configuration;
using medicine_box_api.Configuration;
using medicine_box_api.Infraestructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace medicine_box_api.Api;
[ExcludeFromCodeCoverage]
public class Startup(IConfiguration configuration)
{
    private IConfiguration _configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddAppSettingsConfiguration();
        services.AddApiConfiguration();
        services.AddDatabaseConfiguration(_configuration);
        services.DependencyInjection(_configuration);
        services.AddMediatrConfig();
        services.AddSwaggerConfiguration();
        services.AddMqttConfiguration(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRequestLocalization();
        app.UseApiConfiguration(env);
        app.UseSwaggerConfiguration();
    }
}
