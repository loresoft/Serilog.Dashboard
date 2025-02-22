using Azure.Data.Tables;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

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
        params IEnumerable<string> sourceTables)
    {
        var tables = new HashSet<string>(sourceTables);

        if (tables.Count == 0)
            tables.Add("LogEvent");

        services
            .AddOptions<TableStorageOptions>()
            .Configure(options => options.TableSources = tables);

        services.TryAddSingleton<ILogEventProvider, TableStorageProvider>();

        foreach (var table in tables)
        {
            services.AddTableServiceClient(nameOrConnectionString, table);

            services.TryAddKeyedSingleton<ILogEventEntityRepository>(table, (sp, key) =>
            {
                return new LogEventEntityRepository(
                    logFactory: sp.GetRequiredService<ILoggerFactory>(),
                    tableServiceClient: sp.GetRequiredKeyedService<TableServiceClient>(key),
                    tableName: table);
            });
        }

        return services;
    }
}
