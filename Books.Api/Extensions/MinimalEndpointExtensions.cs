using Books.Api.Abstractions.Endpoints;
using Books.Api.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Books.Api.Extensions;

public static class MinimalEndpointExtensions
{
    public static IServiceCollection AddMinimalEndpoints(this IServiceCollection services)
    {
        var serviceDescriptors = typeof(Program).Assembly
        .DefinedTypes
        .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                       type.IsAssignableTo(typeof(IMinimalEndpoint)))
        .Select(type => ServiceDescriptor.Transient(typeof(IMinimalEndpoint), type))
        .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapMinimalEndpoints(this WebApplication app)
    {
        IEnumerable<IMinimalEndpoint> endpoints = app.Services
            .GetRequiredService<IEnumerable<IMinimalEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }

    public static RouteHandlerBuilder AddValidationFilter<T>(this RouteHandlerBuilder builder)
    {
        return builder
            .AddEndpointFilter<ValidationFilter<T>>()
            .ProducesValidationProblem();
    }
}

