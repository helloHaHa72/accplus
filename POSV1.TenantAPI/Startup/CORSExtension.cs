namespace POSV1.TenantAPI.Startup
{

    public static class CORSExtension
    {
        public static void ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
        public static void EnableCors(this WebApplication app)
        {
            app.UseCors("AllowAll");
        }
    }
}
