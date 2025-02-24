using System.Runtime;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Serilog.Dashboard.Host.Components;
using Serilog.Dashboard.Provider;
using Serilog.Dashboard.Provider.Options;
using Serilog.Dashboard.Provider.TableStorage;

namespace Serilog.Dashboard.Host;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.TryAddSingleton<ILogEventProviderFactory, LogEventProviderFactory>();

        builder.Services.AddTableStorageProvider();

        builder.Services.AddOptions<DashboardOptions>()
            .Configure<IConfiguration>((settings, configuration) => configuration
                .GetSection(DashboardOptions.ConfigurationName)
                .Bind(settings)
            )
            .ValidateDataAnnotations()
            .ValidateOnStart();



        builder.Services.AddRazorComponents();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>();
    }
}
