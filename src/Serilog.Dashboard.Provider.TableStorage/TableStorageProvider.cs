
using System.Collections.Concurrent;

using Azure.Data.Tables;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Serilog.Dashboard.Provider.TableStorage;

public class TableStorageProvider : ILogEventProvider
{
    private readonly ConcurrentDictionary<string, ILogEventEntityRepository> _repositories = [];
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<TableStorageOptions> _options;

    public TableStorageProvider(
        ILoggerFactory logFactory,
        IOptions<TableStorageOptions> options,
        IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }

    public async Task<LogEventResult> GetLogs(LogEventRequest request, CancellationToken cancellationToken = default)
    {
        request.Date ??= DateOnly.FromDateTime(DateTime.Now);
        request.PageSize ??= 100;

        var tableName = request.DataSource
            ?? _options.Value.TableSources.FirstOrDefault()
            ?? "LogEvent";

        var repository = _repositories.GetOrAdd(tableName, (key) =>
        {
            var logFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var tableClient = _serviceProvider.GetRequiredKeyedService<TableServiceClient>(TableStorageOptions.ProviderName);

            return new LogEventEntityRepository(logFactory, tableClient, key);
        });

        var pageResult = await repository.GetLogs(request, cancellationToken);

        return new LogEventResult
        {
            ContinuationToken = pageResult.ContinuationToken,
            Data = pageResult.Data?.Select(ToLogEvent).ToList()
        };
    }

    public Task<IReadOnlySet<string>?> GetSources(CancellationToken cancellationToken = default)
    {
        var tableSources = _options.Value.TableSources;
        return Task.FromResult<IReadOnlySet<string>?>(tableSources);
    }


    private LogEvent ToLogEvent(LogEventEntity entity)
    {
        return new LogEvent
        {
            RowKey = entity.RowKey,
            Timestamp = entity.Timestamp,
            Level = entity.Level,
            MessageTemplate = entity.MessageTemplate,
            RenderedMessage = entity.RenderedMessage,
            SourceContext = entity.SourceContext,
            TraceId = entity.TraceId,
            SpanId = entity.SpanId,
            Exception = entity.Exception,
            Data = entity.Data
        };
    }
}
