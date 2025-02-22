using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

using Serilog.Dashboard.Provider;
using Serilog.Dashboard.Provider.Options;


namespace Serilog.Dashboard.Middleware.Endpoints;

public class DashboardEndpoints : IDashboardEndpoints
{
    private readonly IOptions<DashboardOptions> _options;

    public DashboardEndpoints(IOptions<DashboardOptions> options)
    {
        _options = options;
    }

    public void AddRoutes(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(_options.Value.RoutePrefix, GetLogs)
            .WithName(DashboardOptions.RouteName)
            .ExcludeFromDescription();
    }

    private static async Task<IResult> GetLogs(
        [AsParameters] LogEventRequest request,
        [FromServices] ILogEventProvider logEventProvider,
        CancellationToken cancellationToken = default)
    {
        request.Date ??= DateOnly.FromDateTime(DateTime.Now);
        request.PageSize ??= 100;

        var sources = await logEventProvider.GetSources(cancellationToken);
        var result = await logEventProvider.GetLogs(request, cancellationToken);

        var parameters = new { Request = request, Result = result, Sources = sources };

        return new RazorComponentResult(typeof(Index), parameters);
    }
}
