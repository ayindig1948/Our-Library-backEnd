namespace TheLibraryApi.Services
{
    public static class CashingService
    {
        public static IServiceCollection AddCashingService(this IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                options.AddPolicy("CacheAll", policy =>
                {
                    policy.Expire(TimeSpan.FromMinutes(4));
                    policy.SetVaryByHeader("User-Agent");
                    policy.Tag("CacheAll");
                });

                options.AddPolicy("AdminCache", policy =>
                {
                    policy.Expire(TimeSpan.FromMinutes(1));
                    policy.SetVaryByHeader("User-Agent");
                    policy.Tag("AdminCache");
                });
            });
            return services;
        }
        public static void UseCashingService(this WebApplication app) =>
                app.UseOutputCache();
    }
}