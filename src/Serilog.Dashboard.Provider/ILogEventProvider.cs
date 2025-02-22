namespace Serilog.Dashboard.Provider;

public interface ILogEventProvider
{
    Task<LogEventResult> GetLogs(LogEventRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlySet<string>?> GetSources(CancellationToken cancellationToken = default);
}
