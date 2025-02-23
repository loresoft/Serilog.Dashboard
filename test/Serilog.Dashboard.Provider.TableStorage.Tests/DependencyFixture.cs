﻿using Microsoft.Extensions.Hosting;

using XUnit.Hosting;

namespace Serilog.Dashboard.Provider.TableStorage.Tests;

public class DependencyFixture : TestApplicationFixture
{
    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        var services = builder.Services;

        services
            .AddDashboardTableStorage("StorageAccount", "TestLogs");
    }
}
