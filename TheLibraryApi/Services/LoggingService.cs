using Serilog;
using Serilog.Extensions.Hosting;

namespace TheLibraryApi.Services
{
    public static class LoggingService      
    {
        public static IServiceCollection AddLoggingServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSerilog((sp, lg) =>
             {
                 lg.ReadFrom.Configuration(builder.Configuration)
                   .ReadFrom.Services(sp);
             });
            return builder.Services;
        }
        public static void UseLoggingServices(this WebApplication app) =>
        app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
        };

        // Exclude health check endpoints and Azure "Always On" keep-alive pings from request logs
        options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex != null)
                    return Serilog.Events.LogEventLevel.Error;

                if (httpContext.Request.Path.StartsWithSegments("/health"))
                    return Serilog.Events.LogEventLevel.Verbose;

                // Azure App Service "Always On" pings the root with User-Agent "AlwaysOn"
                var userAgent = httpContext.Request.Headers.UserAgent.ToString();
                if (userAgent.Contains("AlwaysOn", StringComparison.OrdinalIgnoreCase))
                    return Serilog.Events.LogEventLevel.Verbose;

                return elapsed > 500
                    ? Serilog.Events.LogEventLevel.Warning
                    : Serilog.Events.LogEventLevel.Information;
            };
    });
    }
}