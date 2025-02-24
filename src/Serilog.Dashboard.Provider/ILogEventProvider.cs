using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Serilog.Dashboard.Provider.Options;

namespace Serilog.Dashboard.Provider;

public interface ILogEventProvider
{
    Task<LogEventResult> GetLogs(LogEventRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlySet<string>?> GetSources(CancellationToken cancellationToken = default);
}
