namespace Serilog.Dashboard.Models;

public class LogEventResult
{
    public string? ContinuationToken { get; set; }

    public long? Total { get; set; }

    public IReadOnlyCollection<LogEvent>? Data { get; set; }
}
