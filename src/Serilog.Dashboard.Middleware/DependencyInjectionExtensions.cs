using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Serilog.Dashboard.Middleware.Endpoints;
using Serilog.Dashboard.Provider.Options;

namespace Serilog.Dashboard.Middleware;

public static class DependencyInjectionExtensions
{

    public static DashboardOptionsBuilder AddSerilogDashboard(
        this IServiceCollection services,
        Action<DashboardOptions>? configure = null)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddRazorComponents();
        services.AddOptions<DashboardOptions>();
        services.TryAddSingleton<IDashboardEndpoints, DashboardEndpoints>();

        if (configure is not null)
            services.Configure(configure);

        return new DashboardOptionsBuilder(services);
    }

    public static IEndpointRouteBuilder MapSerilogDashboard(
        this IEndpointRouteBuilder builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        var dashboardEndpoints = builder.ServiceProvider.GetRequiredService<IDashboardEndpoints>();
        dashboardEndpoints.AddRoutes(builder);

        return builder;
    }
}
