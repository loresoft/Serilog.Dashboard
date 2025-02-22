using Microsoft.AspNetCore.Routing;


namespace Serilog.Dashboard.Middleware.Endpoints;

public interface IDashboardEndpoints
{
    void AddRoutes(IEndpointRouteBuilder builder);
}
