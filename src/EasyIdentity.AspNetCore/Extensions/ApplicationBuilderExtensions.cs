using EasyIdentity.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEasyIdentity(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var routeBuilder = new RouteBuilder(app);

        foreach (var endpoint in scope.ServiceProvider.GetServices<IEndpoint>())
        {
            routeBuilder.MapVerb(endpoint.Method, endpoint.Path, async (context) =>
            {
                var handler = (IEndpointHandler)context.RequestServices.GetRequiredService(endpoint.Type);
                var result = await handler.ProcessRequestAsync(context);

                await result.ExecuteAsync(context);
            });
        }

        app.UseRouter(routeBuilder.Build());

        return app;
    }
}
