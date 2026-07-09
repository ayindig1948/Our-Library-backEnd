namespace TheLibraryApi.Mcp
{
    public static class McpServices
    {
        public static IServiceCollection AddMcpServices(this IServiceCollection services)
        {
            services.AddMcpServer().WithHttpTransport().WithTools<McpTools>();
            return services;
       
        }
        public static void MapMcpServer(this WebApplication app)
        {
            app.MapMcp("/Mcp").WithDescription("EndPoint to conect to Mcp Tools");
        }
}
}
