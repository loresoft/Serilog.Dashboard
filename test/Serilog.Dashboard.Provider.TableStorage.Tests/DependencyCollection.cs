namespace Serilog.Dashboard.Provider.TableStorage.Tests;


[CollectionDefinition(CollectionName)]
public class DependencyCollection : ICollectionFixture<DependencyFixture>
{
    public const string CollectionName = "DependencyCollection";
}
