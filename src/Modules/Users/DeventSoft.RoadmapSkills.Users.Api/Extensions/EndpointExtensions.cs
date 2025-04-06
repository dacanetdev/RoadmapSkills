using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DeventSoft.RoadmapSkills.Users.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddUsersEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints();
        return services;
    }

    public static IApplicationBuilder UseUsersEndpoints(this IApplicationBuilder app)
    {
        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";
            c.Endpoints.Configurator = ep => ep.PreProcessor<ValidationPreProcessor>();
            c.Versioning.Prefix = "v";
            c.Versioning.DefaultVersion = 1;
        });
        
        return app;
    }
}

public class ValidationPreProcessor : IGlobalPreProcessor
{
    public Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        if (!context.ValidationFailures.Any())
            return Task.CompletedTask;

        context.HttpContext.Response.StatusCode = 400;
        return context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                Errors = context.ValidationFailures
                    .Select(x => new { Field = x.PropertyName, Error = x.ErrorMessage })
            }, ct);
    }
} 