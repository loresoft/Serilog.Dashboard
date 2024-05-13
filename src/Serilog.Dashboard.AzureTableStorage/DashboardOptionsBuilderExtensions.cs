using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Serilog.Dashboard.Options;
using Serilog.Dashboard.Providers;
using Serilog.Dashboard.TableStorage;

using TableStorage.Abstracts;

namespace Serilog.Dashboard;

public static class DashboardOptionsBuilderExtensions
{
    public static DashboardOptionsBuilder AddTableStorage(
        this DashboardOptionsBuilder builder,
        string nameOrConnectionString,
        string tableName = "LogEvents")
    {
        builder.Services
            .AddOptions<TableStorageOptions>()
            .Configure(options =>
            {
                options.ConnectionString = nameOrConnectionString;
                options.TableName = tableName;
            });

        builder.Services.AddTableStorageRepository(nameOrConnectionString);
        builder.Services.TryAddSingleton<ILogEventProvider, TableStorageProvider>();

        return builder;
    }
}
