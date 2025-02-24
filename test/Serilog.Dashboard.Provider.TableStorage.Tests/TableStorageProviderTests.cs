using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Dashboard.Provider.TableStorage.Tests;

[Collection(DependencyCollection.CollectionName)]
public class TableStorageProviderTests(ITestOutputHelper output, DependencyFixture DependencyFixture)
    : DependencyTestBase(output, DependencyFixture)
{
    [Fact]
    public void ResolveRepository()
    {
        var repository = Services.GetRequiredService<ILogEventProviderFactory>();
        Assert.NotNull(repository);
    }
}
