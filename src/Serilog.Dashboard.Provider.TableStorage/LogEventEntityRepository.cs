using Azure.Data.Tables;

using Microsoft.Extensions.Logging;

using Serilog.Dashboard.Provider.Extensions;

using TableStorage.Abstracts;

namespace Serilog.Dashboard.Provider.TableStorage;

public class LogEventEntityRepository : TableRepository<LogEventEntity>, ILogEventEntityRepository
{
    private readonly string _tableName;

    public LogEventEntityRepository(
        ILoggerFactory logFactory,
        TableServiceClient tableServiceClient,
        string tableName)
        : base(logFactory, tableServiceClient)
    {
        _tableName = tableName;
    }

    protected override string GetTableName() => _tableName;

    public async Task<PagedResult<LogEventEntity>> GetLogs(LogEventRequest request, CancellationToken cancellationToken = default)
    {
        request.Date ??= DateOnly.FromDateTime(DateTime.Now);
        request.PageSize ??= 100;

        var filter = KeyGenerator.GeneratePartitionKeyQuery(request.Date.Value);

        if (request.Level.HasValue())
            filter += $" and (Level eq '{request.Level}')";

        if (request.SourceContext.HasValue())
            filter += $" and (SourceContext eq '{request.SourceContext}')";

        if (request.TraceId.HasValue())
            filter += $" and (TraceId eq '{request.TraceId}')";

        if (request.SpanId.HasValue())
            filter += $" and (SpanId eq '{request.SpanId}')";

        return await FindPageAsync(filter, request.ContinuationToken, request.PageSize, cancellationToken);
    }
}
