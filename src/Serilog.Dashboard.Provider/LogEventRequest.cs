namespace Serilog.Dashboard.Provider;

public class LogEventRequest
{
    public string? DataSource { get; set; }


    public string? ContinuationToken { get; set; }

    public int? Page { get; set; }


    public DateOnly? Date { get; set; }

    public string? Level { get; set; }

    public string? SourceContext { get; set; }

    public string? TraceId { get; set; }

    public string? SpanId { get; set; }


    public int? PageSize { get; set; }
}
