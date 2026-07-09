using System.Threading.RateLimiting;
using LibraryTools;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using Serilog;
using TheLibraryApi.EndPoints;
using TheLibraryApi.Services;
using Auth0.AspNetCore.Authentication.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TheLibraryApi.Mcp;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web app");
    Serilog.Debugging.SelfLog.Enable(Console.Error);
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
  builder.AddLoggingServices();
    builder.Services.AddRateService();
    builder.Services.AddCashingService();
    builder.Services.AddDataService();
    builder.AddAuthorizationServices();
    builder.Services.AddCorsService();
    builder.Services.AddValidationServices();
    builder.Services.AddMcpServices();
    var app=builder.Build();
    Log.Information("Hello from Serilog");
         
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(o=>
        {
            o.Title = "The Library API";
            o.Theme=ScalarTheme.BluePlanet;
           
        });
    }
    
    app.UseLoggingServices();
    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();

   app.UseCashingService();
    app.MapMcpServer();

    app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
        .WithName("HealthCheck")
        .WithTags("Health");
    app.MapDataEndPoints();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}







