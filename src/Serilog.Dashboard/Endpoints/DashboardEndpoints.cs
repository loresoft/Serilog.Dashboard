using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

using Serilog.Dashboard.Models;
using Serilog.Dashboard.Options;
using Serilog.Dashboard.Providers;


namespace Serilog.Dashboard.Endpoints;

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
            .WithName(DashboardOptions.RounteName)
            .ExcludeFromDescription();
    }

    private static async Task<IResult> GetLogs(
        [AsParameters] LogEventRequest request,
        [FromServices] ILogEventProvider logEventProvider,
        CancellationToken cancellationToken = default)
    {
        request.Date ??= DateOnly.FromDateTime(DateTime.Now);
        request.PageSize ??= 100;

        var result = await logEventProvider.GetLogs(request, cancellationToken);
        var parameters = new { Request = request, Result = result };

        return new RazorComponentResult(typeof(Dashboard), parameters);
    }
}
