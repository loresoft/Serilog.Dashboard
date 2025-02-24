using System.ComponentModel.DataAnnotations;

namespace Serilog.Dashboard.Provider.Options;

public class DashboardOptions
{
    public const string ConfigurationName = "Dashboard";
    public const string RouteName = "SerilogDashboard";

    //DASHBOARD__NAME
    public string Name { get; set; } = "Serilog Dashboard";

    //DASHBOARD__PROVIDER
    public string Provider { get; set; } = null!;

    //DASHBOARD__CONNECTIONSTRING
    public string ConnectionString { get; set; } = null!;

    //DASHBOARD__DATASOURCES
    public string? DataSources { get; set; }


    public string Route { get; set; } = "/serilog";

    public string HomeUrl { get; set; } = "/";

    public IEnumerable<string> GetSources()
    {
        return DataSources?.Split(';') ?? [];
    }
}
