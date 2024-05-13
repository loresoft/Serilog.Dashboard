namespace Serilog.Dashboard.Models;

public class LogEventRequest
{
    public string? ContinuationToken { get; set; }

    public DateOnly? Date { get; set; }

    public string? Level { get; set; }

    public string? TraceId { get; set; }

    public string? SpanId { get; set; }

    public int? PageSize { get; set; }
}
