using Auth0.AspNetCore.Authentication.Api;

namespace TheLibrayApi.Services
{
    public static class AuthServices
    {
        public static void AddAuthorizationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuth0ApiAuthentication(
     builder.Configuration.GetSection("Auth0"));
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("read:books", policy => policy.RequireClaim("permissions", "read:books"));
                options.AddPolicy("write:books", policy => policy.RequireClaim("permissions", "write:books"));
            });
           
        }
        
    }
}
