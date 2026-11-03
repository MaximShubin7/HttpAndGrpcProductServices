using HttpAndGrpcProductServices.Http.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace HttpAndGrpcProductServices.Http;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("http", new OpenApiInfo
            {
                Title = "Product HTTP API",
                Version = "http",
                Description = "HTTP-слой представления сервиса товаров"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseHttpPresentation(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/http/swagger.json", "Product HTTP API");
        });

        app.MapControllers();
        return app;
    }
}