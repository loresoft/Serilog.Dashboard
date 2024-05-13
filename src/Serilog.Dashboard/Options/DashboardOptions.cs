namespace Serilog.Dashboard.Options;

public class DashboardOptions
{
    public const string RounteName = "SerilogDashboard";

    public string DashboardName { get; set; } = "Serilog Dashboard";
    public string RoutePrefix { get; set; } = "/serilog";
    public string HomeUrl { get; set; } = "/";

}
