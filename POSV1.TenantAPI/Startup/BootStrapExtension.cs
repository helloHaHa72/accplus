using POSV1.TenantModel.Modules;
using POSV1.TenantModel;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Middleware;

namespace POSV1.TenantAPI.Startup
{
    public static class BootStrapExtension
    {
        public static async Task AppStart(this WebApplication app,
            IConfiguration configuration,
            WebApplicationBuilder builder)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.EnableCors();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            // Register the custom JWT Middleware
            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.UseDBMigrationAsync(builder);

            //await app.SeedMasterDataAsync();

            await app.SeedMasterDataAsyncV2();
        }
        public static async Task Startup(
            this WebApplicationBuilder builder,
            IConfiguration configuration)
        {
            //wrapper for appsetting data
            AppSettingsWrapper.AppSettings = configuration
                .GetSection("AppSettings").Get<AppSettings>();


            // Configure Serilog
            builder.ConfigureSeriLog(configuration);

            // Configure Swagger
            await builder.ConfigureSwagger();

            // Configure CORS
            builder.ConfigureCors();

            // Configure JWT
            builder.ConfigureJWT(configuration);

            // Configure Database
            builder.ConfigureDatabase(configuration);

            // Configure Dependency Injection
            builder.ConfigureServices();

            // Configure Tenant
            //await builder.ConfigureTenant();
            builder.Services.ConfigureTenantModelServices();

            // Configure AutoMapper
            builder.ConfigureAutoMapper();

            //ConfigureAwaitOptions R2Cloud
            builder.ConfigureR2Cloud();

            //// Configure SignalR
            //builder.ConfigureSignalR();

            //// Configure Health Checks
            //builder.ConfigureHealthChecks();

            //// Configure Hangfire
            //builder.ConfigureHangfire();

            //// Configure Background Services
            //builder.ConfigureBackgroundServices();

            //// Configure MVC
            //builder.ConfigureMVC();

            //// Configure Response Compression
            //builder.ConfigureResponseCompression();

            //// Configure Rate Limiting
            //builder.ConfigureRateLimit();

            //Configure JSON Serialization
            builder.ConfigureJSONSerialization();
        }
    }
}
