using Serilog.Dashboard.Models;

namespace Serilog.Dashboard.Providers;

public interface ILogEventProvider
{
    Task<LogEventResult> GetLogs(LogEventRequest request, CancellationToken cancellationToken = default);
}
