namespace Serilog.Dashboard.Models;

public class LogEvent
{
    public string RowKey { get; set; } = Guid.NewGuid().ToString("N");

    public DateTimeOffset? Timestamp { get; set; }

    public string Level { get; set; } = "Information";

    public string? MessageTemplate { get; set; }

    public string RenderedMessage { get; set; } = string.Empty;

    public string? TraceId { get; set; }

    public string? SpanId { get; set; }

    public string? Exception { get; set; }

    public string? Data { get; set; }
}
