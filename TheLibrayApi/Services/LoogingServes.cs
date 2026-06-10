using Serilog;
using Serilog.Extensions.Hosting;

namespace TheLibrayApi.Serves
{
    public static class LoogingService      
    {
        public static IServiceCollection AddLoogingServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSerilog((sp, lg) =>
             {
                 lg.ReadFrom.Configuration(builder.Configuration)
                   .ReadFrom.Services(sp);
             });
            return builder.Services;
        }
        public static void UseLoogingServices(this WebApplication app) =>
        app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
        };

        // Exclude health check endpoints from request logs
        options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (httpContext.Request.Path.StartsWithSegments("/health"))
                    return Serilog.Events.LogEventLevel.Verbose;

                return elapsed > 500
                    ? Serilog.Events.LogEventLevel.Warning
                    : Serilog.Events.LogEventLevel.Information;
            };
    });
    }
}