namespace TheLibrayApi.Services;

public static class CorsService
{
    public static IServiceCollection AddCorsService(this IServiceCollection services) {
      services.AddCors(p =>
        {
            p.AddDefaultPolicy(o =>
            {
                o.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
            });
        });
        return services;
    }
    
}