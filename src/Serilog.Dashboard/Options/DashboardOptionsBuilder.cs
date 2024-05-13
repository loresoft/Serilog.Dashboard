using Microsoft.Extensions.DependencyInjection;


namespace Serilog.Dashboard.Options;

public class DashboardOptionsBuilder
{

    public DashboardOptionsBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public DashboardOptionsBuilder WithOptions(Action<DashboardOptions> configure)
    {
        if (configure != null)
            Services.Configure(configure);

        return this;
    }
}
