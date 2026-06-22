using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace TheLibraryApi.Services
{
    public  static class RateService
    {
        public static void AddRateService(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // Configure default rejection behavior (HTTP 429 Too Many Requests)
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Define a Fixed Window Policy named "FixedPolicy"
                options.AddFixedWindowLimiter(policyName: "FixedPolicy", fixedOptions =>
                {
                    fixedOptions.PermitLimit = 10;
                    fixedOptions.Window = TimeSpan.FromSeconds(10);
                    fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    fixedOptions.QueueLimit = 2;
                });
            });
      
            
        }
    }
}
