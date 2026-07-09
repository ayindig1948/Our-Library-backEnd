namespace TheLibraryApi.Services;

public static class CorsService
{
    public static IServiceCollection AddCorsService(this IServiceCollection services) {
      services.AddCors(p =>
        {
            p.AddDefaultPolicy(o =>
            {
                o.WithOrigins("http://localhost:5173", "http://localhost:6274").AllowAnyMethod().AllowAnyHeader();
            });
        });
        return services;
    }
    
}