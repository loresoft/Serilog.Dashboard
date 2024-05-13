using Microsoft.AspNetCore.Routing;


namespace Serilog.Dashboard.Endpoints;

public interface IDashboardEndpoints
{
    void AddRoutes(IEndpointRouteBuilder builder);
}
