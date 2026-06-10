using System.Threading.RateLimiting;
using LibraryTools;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using Serilog;
using TheLibrayApi.EndPoints;
using TheLibrayApi.Serves;
using TheLibrayApi.Services;
using Auth0.AspNetCore.Authentication.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
  builder.AddLoogingServices();
    builder.Services.AddRateService();
    builder.Services.AddCashingService();
    builder.Services.AddDataService();
    builder.AddAuthorizationServices();
    builder.Services.AddCorsService();
    builder.Services.AddValidationServices();
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
    
    app.UseLoogingServices();
    app.UseHttpsRedirection();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();

   app.UseCashingService();

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







