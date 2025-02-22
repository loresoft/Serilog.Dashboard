using TableStorage.Abstracts;

namespace Serilog.Dashboard.Provider.TableStorage;

public interface ILogEventEntityRepository : ITableRepository<LogEventEntity>
{
    Task<PagedResult<LogEventEntity>> GetLogs(LogEventRequest request, CancellationToken cancellationToken = default);
}
