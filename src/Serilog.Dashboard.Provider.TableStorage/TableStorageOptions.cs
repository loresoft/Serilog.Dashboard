namespace Serilog.Dashboard.Provider.TableStorage;

public class TableStorageOptions
{
    public const string ProviderName = "TableStorage";

    public HashSet<string> TableSources { get; set; } = [];
}
