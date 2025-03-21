using CloudFlareR2Storage;
using Microsoft.Extensions.Options;
using POSV1.TenantAPI.Extensions;
using POSV1.TenantAPI.Models;
using System.Runtime;

namespace POSV1.TenantAPI.Startup
{
    public static class AppSettingsExtension
    {
        public static async Task ConfigureAppSettings(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        public static void ConfigureAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program));
        }

        public static void ConfigureR2Cloud(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<R2Settings>(builder.Configuration.GetSection("R2Settings"));

            // Register the R2StorageService
            builder.Services.AddTransient<R2StorageService>(sp =>
            {
                var r2Settings = sp.GetRequiredService<IOptions<R2Settings>>().Value;
                return new R2StorageService(
                    r2Settings.AccessKeyId,
                    r2Settings.SecretAccessKey,
                    r2Settings.BucketName,
                    r2Settings.Endpoint
                );
            });
        }

        public static void ConfigureJSONSerialization(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Clear();
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());

                // Enable case-insensitive property name matching
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        }
    }
}
