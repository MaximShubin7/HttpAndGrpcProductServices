using HttpAndGrpcProductServices.Grpc.Interceptors;
using HttpAndGrpcProductServices.Grpc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace HttpAndGrpcProductServices.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddGrpcPresentation(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<LoggingInterceptor>();
        })
        .AddJsonTranscoding();

        services.AddServiceModelGrpcSwagger();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("grpc", new OpenApiInfo
            {
                Title = "Product gRPC API (via Gateway)",
                Version = "grpc"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseGrpcPresentation(this WebApplication app)
    {
        app.UseServiceModelGrpcSwaggerGateway();
        app.MapGrpcService<ProductGrpcService>();
        return app;
    }
}