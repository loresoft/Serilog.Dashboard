namespace Serilog.Dashboard;

public class TableStorageOptions
{
    public string ConnectionString { get; set; } = null!;
    public string TableName { get; set; } = "LogEvents";
}
