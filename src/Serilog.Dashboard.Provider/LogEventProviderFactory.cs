using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Serilog.Dashboard.Provider.Options;

namespace Serilog.Dashboard.Provider;

public class LogEventProviderFactory : ILogEventProviderFactory
{
    private readonly IOptions<DashboardOptions> _options;
    private readonly IServiceProvider _serviceProvider;

    public LogEventProviderFactory(IServiceProvider serviceProvider, IOptions<DashboardOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options;
    }

    public ILogEventProvider CreateProvider()
    {
        var providerName = _options.Value.Provider;
        return _serviceProvider.GetRequiredKeyedService<ILogEventProvider>(providerName);
    }
}
