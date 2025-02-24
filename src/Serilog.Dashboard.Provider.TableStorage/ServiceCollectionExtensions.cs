using Azure.Data.Tables;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Serilog.Dashboard.Provider.Options;

using TableStorage.Abstracts;

namespace Serilog.Dashboard.Provider.TableStorage;

public static class ServiceCollectionExtensions
{
    public static DashboardOptionsBuilder AddTableStorage(
        this DashboardOptionsBuilder builder,
        string nameOrConnectionString,
        params IEnumerable<string> sourceTables)
    {
        builder.Services
            .AddDashboardTableStorage(nameOrConnectionString, sourceTables);

        return builder;
    }

    public static IServiceCollection AddDashboardTableStorage(
        this IServiceCollection services,
        string nameOrConnectionString,
        params IEnumerable<string>? sourceTables)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(nameOrConnectionString);

        var tables = new HashSet<string>(sourceTables ?? []);


        services.TryAddSingleton<ILogEventProvider, TableStorageProvider>();

        services
            .AddOptions<TableStorageOptions>()
            .Configure(options => options.TableSources = tables);

        return services;
    }

    public static IServiceCollection AddTableStorageProvider(this IServiceCollection services)
    {
        services.TryAddKeyedSingleton(
            serviceKey: TableStorageOptions.ProviderName,
            implementationFactory: (sp, _) =>
            {
                var options = sp.GetRequiredService<IOptions<DashboardOptions>>();
                var connectionString = sp.ResolveConnectionString(options.Value.ConnectionString);

                return new TableServiceClient(connectionString);
            }
        );

        services.TryAddKeyedSingleton<ILogEventProvider, TableStorageProvider>(TableStorageOptions.ProviderName);

        services.AddOptions<TableStorageOptions>()
            .Configure<IOptions<DashboardOptions>>(static (storageOptions, dashboardOptions)
                => storageOptions.TableSources = [.. dashboardOptions.Value.GetSources()]
            );

        return services;
    }
}
