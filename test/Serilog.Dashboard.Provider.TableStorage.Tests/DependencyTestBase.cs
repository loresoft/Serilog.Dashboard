using XUnit.Hosting;

namespace Serilog.Dashboard.Provider.TableStorage.Tests;

[Collection(DependencyCollection.CollectionName)]
public abstract class DependencyTestBase(ITestOutputHelper output, DependencyFixture DependencyFixture)
    : TestHostBase<DependencyFixture>(output, DependencyFixture);
