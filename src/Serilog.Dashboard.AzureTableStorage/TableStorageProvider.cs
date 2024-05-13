using Azure.Data.Tables;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Serilog.Dashboard.Extensions;
using Serilog.Dashboard.Models;
using Serilog.Dashboard.Providers;

using TableStorage.Abstracts;

namespace Serilog.Dashboard.TableStorage;

public class TableStorageProvider : TableRepository<LogEventEntity>, ILogEventProvider
{
    private readonly IOptions<TableStorageOptions> _options;
    public TableStorageProvider(ILoggerFactory logFactory, TableServiceClient tableServiceClient, IOptions<TableStorageOptions> options)
        : base(logFactory, tableServiceClient)
    {
        _options = options;
    }

    protected override string GetTableName() => _options.Value.TableName;

    public async Task<LogEventResult> GetLogs(LogEventRequest request, CancellationToken cancellationToken = default)
    {
        request.Date ??= DateOnly.FromDateTime(DateTime.Now);
        request.PageSize ??= 100;

        var filter = KeyGenerator.GeneratePartitionKeyQuery(request.Date.Value);

        if (request.Level.HasValue())
            filter += $" and (Level eq '{request.Level}')";

        if (request.TraceId.HasValue())
            filter += $" and (TraceId eq '{request.TraceId}')";

        if (request.SpanId.HasValue())
            filter += $" and (SpanId eq '{request.SpanId}')";

        var pageResult = await FindPageAsync(filter, request.ContinuationToken, request.PageSize, cancellationToken);

        return new LogEventResult
        {
            ContinuationToken = pageResult.ContinuationToken,
            Data = pageResult.Data?.Select(ToLogEvent).ToList()
        };
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
            TraceId = entity.TraceId,
            SpanId = entity.SpanId,
            Exception = entity.Exception,
            Data = entity.Data
        };
    }
}
