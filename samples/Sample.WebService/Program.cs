using Microsoft.Extensions.DependencyInjection.Extensions;

using Sample.Shared;

using Serilog;
using Serilog.Dashboard.Middleware;
using Serilog.Dashboard.Provider.TableStorage;
using Serilog.Events;

namespace Sample.WebService;

public static class Program
{
    private const string OutputTemplate = "{Timestamp:HH:mm:ss.fff} [{Level:u1}] {Message:lj}{NewLine}{Exception}";

    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: OutputTemplate)
            .CreateBootstrapLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureLogging(builder);

            ConfigureServices(builder);

            var app = builder.Build();

            ConfigureMiddleware(app);

            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        var homeDirectory = Environment.GetEnvironmentVariable("HOME") ?? ".";
        var logDirectory = Path.Combine(homeDirectory, "LogFiles");

        builder.Host
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName)
                .Enrich.WithProperty("ApplicationVersion", ThisAssembly.InformationalVersion)
                .Enrich.WithProperty("EnvironmentName", builder.Environment.EnvironmentName)
                .WriteTo.Console(outputTemplate: OutputTemplate)
                .WriteTo.File(
                    path: $"{logDirectory}/log.txt",
                    outputTemplate: OutputTemplate,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10
                )
                .WriteTo.AzureTableStorage(
                    connectionString: context.Configuration.GetConnectionString("StorageAccount"),
                    storageTableName: "SampleLogs",
                    propertyColumns: ["SourceContext", "RequestId", "RequestPath", "ConnectionId", "ApplicationName", "ApplicationVersion", "EnvironmentName"]
                )
            );
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.TryAddSingleton<WeatherForecastService>();

        builder.Services
            .AddSerilogDashboard(options => options.HomeUrl = "/swagger")
            .AddTableStorage("StorageAccount", "ApplicationLogsDevelopment", "ApplicationLogsStaging", "ApplicationLogsProduction");
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapSerilogDashboard();

        app.MapControllers();
    }

}
