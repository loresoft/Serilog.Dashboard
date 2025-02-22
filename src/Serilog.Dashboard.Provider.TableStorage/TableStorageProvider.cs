
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Serilog.Dashboard.Provider.TableStorage;

public class TableStorageProvider : ILogEventProvider
{
    private readonly IOptions<TableStorageOptions> _options;
    private readonly IServiceProvider _serviceProvider;

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

        var serviceKey = request.DataSource
            ?? _options.Value.TableSources.FirstOrDefault()
            ?? "LogEvent";

        var repository = _serviceProvider.GetRequiredKeyedService<ILogEventEntityRepository>(serviceKey);

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
